using Microsoft.EntityFrameworkCore;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.DTOs;
using Stemma.Infrastructure.Interface;
using Stemma.Infrastructure.UtilityHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Repository
{
    public class SpouseRelationRepository : ISpouseRelationRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public SpouseRelationRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<PagedResult<SpouseRelation>> Filter(string searchText, int pageNo, int pageSize)
        {
            var result = await GetCachedList();

            return result.Where(
                x => !x.IsDeleted
                && (x.Relation).ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : (x.Relation).ToLower().Trim())
                ).OrderByDescending(x => x.SpouseRelationId).ToList().GetPaged(pageNo, pageSize);
        }

        public async Task<IEnumerable<SpouseRelation>> Get(long spouseRelationId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.SpouseRelationId == (spouseRelationId == 0 ? x.SpouseRelationId : spouseRelationId)).ToList();
        }

        public async Task<long> Save(SpouseRelation spouseRelation, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (spouseRelation.SpouseRelationId > 0)
                    {
                        var result = await GetCachedList();
                        var oldSpouseRelation = result.Where(x => x.SpouseRelationId == spouseRelation.SpouseRelationId).ToList().LastOrDefault();

                        if (oldSpouseRelation != null)
                        {
                            spouseRelation.CreatedBy = oldSpouseRelation.CreatedBy;
                            spouseRelation.CreatedOn = oldSpouseRelation.CreatedOn;
                            spouseRelation.IsDeleted = oldSpouseRelation.IsDeleted;

                            spouseRelation.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(spouseRelation).State = EntityState.Modified;
                            unitOfWork.Context.SpouseRelations.Update(spouseRelation);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(spouseRelation).State = EntityState.Detached;
                        }
                        else spouseRelation.SpouseRelationId = 0;
                    }
                    else
                    {
                        spouseRelation.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<SpouseRelation>().Add(spouseRelation);
                        unitOfWork.Commit();
                    }

                    if (spouseRelation.SpouseRelationId > 0) RemoveCache();

                    return spouseRelation.SpouseRelationId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }
        }

        public async Task<bool> Delete(long spouseRelationId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            SpouseRelation spouseRelation = result.Where(x => !x.IsDeleted && x.SpouseRelationId == spouseRelationId).ToList().LastOrDefault();

            if (spouseRelation != null)
            {
                using (transaction)
                {
                    try
                    {
                        spouseRelation.IsDeleted = true;
                        unitOfWork.Context.Entry(spouseRelation).State = EntityState.Modified;
                        unitOfWork.Context.SpouseRelations.Update(spouseRelation);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(spouseRelation).State = EntityState.Detached;

                        await binRepository.Add(spouseRelationId, "SpouseRelations", spouseRelation.Relation, deletedByIdentityId);

                        RemoveCache();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }

                    return false;
                }
            }

            return false;
        }

        private async Task<IEnumerable<SpouseRelation>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "SpouseRelations",
                () => unitOfWork.Context.SpouseRelations.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("SpouseRelations");
        }

    }
}
