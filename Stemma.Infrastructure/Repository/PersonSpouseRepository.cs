using Microsoft.EntityFrameworkCore;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Repository
{
    public class PersonSpouseRepository : IPersonSpouseRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public PersonSpouseRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<IEnumerable<PersonSpouse>> Get(long PersonSpouseId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.PersonSpouseId == (PersonSpouseId == 0 ? x.PersonSpouseId : PersonSpouseId)).ToList();
        }

        public async Task<PersonSpouse> GetByPerson(long personId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.PersonIdF == personId || x.SpousePersonIDF==personId).LastOrDefault();
        }

        public async Task<long> Save(PersonSpouse personSpouse, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (personSpouse.PersonSpouseId > 0)
                    {
                        var result = await GetCachedList();
                        var oldPersonSpouse = result.Where(x => x.PersonSpouseId == personSpouse.PersonSpouseId).ToList().LastOrDefault();

                        if (oldPersonSpouse != null)
                        {
                            personSpouse.CreatedBy = oldPersonSpouse.CreatedBy;
                            personSpouse.CreatedOn = oldPersonSpouse.CreatedOn;
                            personSpouse.IsDeleted = oldPersonSpouse.IsDeleted;

                            personSpouse.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(personSpouse).State = EntityState.Modified;
                            unitOfWork.Context.PersonSpouses.Update(personSpouse);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(personSpouse).State = EntityState.Detached;
                        }
                        else personSpouse.PersonSpouseId = 0;
                    }
                    else
                    {
                        personSpouse.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<PersonSpouse>().Add(personSpouse);
                        unitOfWork.Commit();
                    }

                    if (personSpouse.PersonSpouseId > 0) RemoveCache();

                    return personSpouse.PersonSpouseId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }
        }

        public async Task<bool> Delete(long personSpouseId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            PersonSpouse personSpouse = result.Where(x => !x.IsDeleted && x.PersonSpouseId == personSpouseId).ToList().LastOrDefault();

            if (personSpouse != null)
            {
                using (transaction)
                {
                    try
                    {
                        personSpouse.UpdatedOn = DateTime.Now;
                        personSpouse.IsDeleted = true;
                        unitOfWork.Context.Entry(personSpouse).State = EntityState.Modified;
                        unitOfWork.Context.PersonSpouses.Update(personSpouse);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(personSpouse).State = EntityState.Detached;

                        await binRepository.Add(personSpouseId, "PersonSpouses", "", deletedByIdentityId);

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

        private async Task<IEnumerable<PersonSpouse>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "PersonSpouses",
                () => unitOfWork.Context.PersonSpouses.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("PersonSpouses");
        }
    }
}
