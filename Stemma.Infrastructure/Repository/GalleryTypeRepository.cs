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
    public class GalleryTypeRepository : IGalleryTypeRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public GalleryTypeRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<PagedResult<GalleryType>> Filter(string searchText, int pageNo, int pageSize)
        {
            var result = await GetCachedList();

            return result.Where(
                x => !x.IsDeleted
                && (x.Type).ToLower().Trim().Contains(!string.IsNullOrEmpty(searchText) ? searchText : (x.Type).ToLower().Trim())
                ).OrderByDescending(x => x.Id).ToList().GetPaged(pageNo, pageSize);
        }

        public async Task<IEnumerable<GalleryType>> Get(long galleryTypeId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.Id == (galleryTypeId == 0 ? x.Id : galleryTypeId)).ToList();
        }

        public async Task<long> Save(GalleryType galleryType, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (galleryType.Id > 0)
                    {
                        var result = await GetCachedList();
                        var oldGalleryType = result.Where(x => x.Id == galleryType.Id).ToList().LastOrDefault();

                        if (oldGalleryType != null)
                        {
                            galleryType.CreatedBy = oldGalleryType.CreatedBy;
                            galleryType.CreatedOn = oldGalleryType.CreatedOn;
                            galleryType.IsDeleted = oldGalleryType.IsDeleted;

                            galleryType.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(galleryType).State = EntityState.Modified;
                            unitOfWork.Context.GalleryTypes.Update(galleryType);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(galleryType).State = EntityState.Detached;
                        }
                        else galleryType.Id = 0;
                    }
                    else
                    {
                        galleryType.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<GalleryType>().Add(galleryType);
                        unitOfWork.Commit();
                    }

                    if (galleryType.Id > 0) RemoveCache();

                    return galleryType.Id;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }
        }

        public async Task<bool> Delete(long galleryTypeId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            GalleryType galleryType = result.Where(x => !x.IsDeleted && x.Id == galleryTypeId).ToList().LastOrDefault();

            if (galleryType != null)
            {
                using (transaction)
                {
                    try
                    {
                        galleryType.UpdatedOn = DateTime.Now;
                        galleryType.IsDeleted = true;
                        unitOfWork.Context.Entry(galleryType).State = EntityState.Modified;
                        unitOfWork.Context.GalleryTypes.Update(galleryType);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(galleryType).State = EntityState.Detached;

                        await binRepository.Add(galleryTypeId, "GalleryTypes", galleryType.Type, deletedByIdentityId);

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

        private async Task<IEnumerable<GalleryType>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "GalleryTypes",
                () => unitOfWork.Context.GalleryTypes.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("GalleryTypes");
        }

    }
}
