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
    public class PersonRepository : IPersonRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public PersonRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<PagedResult<Person>> Filter(string searchText, int pageNo, int pageSize)
        {
            var result = await GetCachedList();

            return result.Where(
                x => !x.IsDeleted
                && (x.FirstName + " " + x.LastName).ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : (x.FirstName + " " + x.LastName).ToLower().Trim())
                ).OrderByDescending(x => x.PersonId).ToList().GetPaged(pageNo, pageSize);
        }

        public async Task<IEnumerable<Person>> Get(long personId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.PersonId == (personId == 0 ? x.PersonId : personId)).ToList();
        }

        public async Task<Person> GetByIdentityId(string identityId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.IdentityUserIDF == identityId).LastOrDefault();
        }

        public async Task<long> Save(Person person, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (person.PersonId > 0)
                    {
                        var result = await GetCachedList();
                        var oldPerson = result.Where(x => x.PersonId == person.PersonId).ToList().LastOrDefault();

                        if (oldPerson != null)
                        {
                            person.CreatedBy = oldPerson.CreatedBy;
                            person.CreatedOn = oldPerson.CreatedOn;
                            person.IsActive = oldPerson.IsActive;
                            person.IsDeleted = oldPerson.IsDeleted;

                            person.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(person).State = EntityState.Modified;
                            unitOfWork.Context.Persons.Update(person);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(person).State = EntityState.Detached;
                        }
                        else person.PersonId = 0;
                    }
                    else
                    {
                        person.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<Person>().Add(person);
                        unitOfWork.Commit();
                    }

                    if (person.PersonId > 0) RemoveCache();

                    return person.PersonId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }
        }

        public async Task<bool> Activate(long PersonId, bool isActive, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Person person = result.Where(x => !x.IsDeleted && x.PersonId == PersonId).ToList().LastOrDefault();

            if (person != null)
            {
                using (transaction)
                {
                    try
                    {
                        person.IsActive = isActive;
                        unitOfWork.Context.Entry(person).State = EntityState.Modified;
                        unitOfWork.Context.Persons.Update(person);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(person).State = EntityState.Detached;

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

        public async Task<bool> Delete(long personId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Person person = result.Where(x => !x.IsDeleted && x.PersonId == personId).ToList().LastOrDefault();

            if (person != null)
            {
                using (transaction)
                {
                    try
                    {
                        person.UpdatedOn = DateTime.Now;
                        person.IsDeleted = true;
                        unitOfWork.Context.Entry(person).State = EntityState.Modified;
                        unitOfWork.Context.Persons.Update(person);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(person).State = EntityState.Detached;

                        await binRepository.Add(personId, "Persons", person.FirstName + " " + person.LastName, deletedByIdentityId);

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

        public async Task<long> TotalBySurname(long surnameId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.SurnameIDF ==surnameId).LongCount();
        }


        private async Task<IEnumerable<Person>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "Persons",
                () => unitOfWork.Context.Persons.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("Persons");
        }

    }
}
