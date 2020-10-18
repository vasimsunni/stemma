using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.DTOs;
using Stemma.Infrastructure.Interface;
using Stemma.Infrastructure.UtilityHelper;

namespace Stemma.Infrastructure.Repository
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public AdministratorRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<PagedResult<Administrator>> Filter(string searchText, int pageNo, int pageSize)
        {
            try
            {
                var result = await GetCachedList();

                return result.Where(
                    x => !x.IsDeleted
                    && (x.FirstName + " " + x.LastName).ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : (x.FirstName + " " + x.LastName).ToLower().Trim())
                    ).OrderByDescending(x => x.AdminId).ToList().GetPaged(pageNo, pageSize);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Administrator>> Get(long administratorId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.AdminId == (administratorId == 0 ? x.AdminId : administratorId)).ToList();
        }

        public async Task<Administrator> GetByIdentityId(string identityId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.IdentityUserIdf == identityId).LastOrDefault();
        }

        public async Task<long> Save(Administrator administrator, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (administrator.AdminId > 0)
                    {
                        var result = await GetCachedList();
                        var oldAdministrator = result.Where(x => x.AdminId == administrator.AdminId).ToList().LastOrDefault();

                        if (oldAdministrator != null)
                        {
                            administrator.CreatedBy = oldAdministrator.CreatedBy;
                            administrator.CreatedOn = oldAdministrator.CreatedOn;
                            administrator.IsActive = oldAdministrator.IsActive;
                            administrator.IsDeleted = oldAdministrator.IsDeleted;

                            administrator.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(administrator).State = EntityState.Modified;
                            unitOfWork.Context.Administrators.Update(administrator);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(administrator).State = EntityState.Detached;
                        }
                        else administrator.AdminId = 0;
                    }
                    else
                    {
                        administrator.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<Administrator>().Add(administrator);
                        unitOfWork.Commit();
                    }

                    if (administrator.AdminId > 0) RemoveCache();

                    return administrator.AdminId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }


        }

        public async Task<bool> Activate(long administratorId, bool isActive, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Administrator administrator = result.Where(x => !x.IsDeleted && x.AdminId == administratorId).ToList().LastOrDefault();

            if (administrator != null)
            {
                using (transaction)
                {
                    try
                    {
                        administrator.UpdatedOn = DateTime.Now;
                        administrator.IsActive = isActive;
                        unitOfWork.Context.Entry(administrator).State = EntityState.Modified;
                        unitOfWork.Context.Administrators.Update(administrator);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(administrator).State = EntityState.Detached;

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

        public async Task<bool> Delete(long administratorId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Administrator administrator = result.Where(x => !x.IsDeleted && x.AdminId == administratorId).ToList().LastOrDefault();

            if (administrator != null)
            {
                using (transaction)
                {
                    try
                    {
                        administrator.UpdatedOn = DateTime.Now;
                        administrator.IsDeleted = true;
                        unitOfWork.Context.Entry(administrator).State = EntityState.Modified;
                        unitOfWork.Context.Administrators.Update(administrator);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(administrator).State = EntityState.Detached;

                        await binRepository.Add(administratorId, "Administrators", administrator.FirstName + " " + administrator.LastName, deletedByIdentityId);

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

        private async Task<IEnumerable<Administrator>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "Administrators",
                () => unitOfWork.Context.Administrators.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("Administrators");
        }

    }
}
