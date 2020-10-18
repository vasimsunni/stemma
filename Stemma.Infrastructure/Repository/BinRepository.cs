using Microsoft.EntityFrameworkCore;
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
    public class BinRepository : IBinRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;

        public BinRepository(IUnitOfWork unitOfWork, ICachingService cachingService)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
        }

        public async Task<PagedResult<Bin>> Filter(string searchText, int pageNo, int pageSize)
        {
            try
            {
                var result = await GetCachedList();

                return result.Where(x => x.Entity.ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : x.Entity.ToLower().Trim())
                    ).OrderByDescending(x => x.Id).ToList().GetPaged(pageNo, pageSize);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<long> Add(long entityId, string entity, string title, string deletedByIdentity)
        {
            var bin = new Bin()
            {
                EntityId = entityId,
                Entity = entity,
                Title = title,
                DeletedByIdentityId = deletedByIdentity,
                DeletedOn = DateTime.Now
            };

            unitOfWork.Context.Set<Bin>().Add(bin);
            unitOfWork.Commit();

            if (bin.Id > 0) RemoveCache();

            return bin.Id;
        }

        private async Task<IEnumerable<Bin>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "Bin",
                () => unitOfWork.Context.Bin.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("Bin");
        }
    }
}
