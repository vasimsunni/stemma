﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.DTOs;
using Stemma.Infrastructure.Interface;
using Stemma.Infrastructure.UtilityHelper;

namespace Stemma.Infrastructure.Repository
{
    public class SurnameRepository : ISurnameRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public SurnameRepository(IUnitOfWork unitOfWork, ICachingService cachingService)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
        }

        public async Task<PagedResult<Surname>> Filter(string searchText, int pageNo, int pageSize)
        {
            var result = await GetCachedList();

            return result.Where(
                x => !x.IsDeleted
                && (x.Title).ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : (x.Title).ToLower().Trim())
                ).OrderByDescending(x => x.SurnameId).ToList().GetPaged(pageNo, pageSize);
        }

        public async Task<IEnumerable<Surname>> Get(long SurnameId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.SurnameId == (SurnameId == 0 ? x.SurnameId : SurnameId)).ToList();
        }

        public async Task<long> Save(Surname surname, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (surname.SurnameId > 0)
                    {
                        var result = await GetCachedList();
                        var oldSurname = result.Where(x => x.SurnameId == surname.SurnameId).ToList().LastOrDefault();

                        if (oldSurname != null)
                        {
                            surname.CreatedBy = oldSurname.CreatedBy;
                            surname.CreatedOn = oldSurname.CreatedOn;
                            surname.IsDeleted = oldSurname.IsDeleted;

                            surname.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(surname).State = EntityState.Modified;
                            unitOfWork.Context.Surnames.Update(surname);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(surname).State = EntityState.Detached;
                        }
                        else surname.SurnameId = 0;
                    }
                    else
                    {
                        surname.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<Surname>().Add(surname);
                        unitOfWork.Commit();
                    }

                    if (surname.SurnameId > 0) RemoveCache();

                    return surname.SurnameId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }
        }

       public async Task<bool> Delete(long surnameId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Surname surname = result.Where(x => !x.IsDeleted && x.SurnameId == surnameId).ToList().LastOrDefault();

            if (surname != null)
            {
                using (transaction)
                {
                    try
                    {
                        surname.UpdatedOn = DateTime.Now;
                        surname.IsDeleted = true;
                        unitOfWork.Context.Entry(surname).State = EntityState.Modified;
                        unitOfWork.Context.Surnames.Update(surname);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(surname).State = EntityState.Detached;

                        await binRepository.Add(surnameId, "Surnames", surname.Title, deletedByIdentityId);

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

        private async Task<IEnumerable<Surname>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "Surnames",
                () => unitOfWork.Context.Surnames.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("Surnames");
        }

    }
}
