﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.Interface;

namespace Stemma.Infrastructure.Repository
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public GalleryRepository(IUnitOfWork unitOfWork, ICachingService cachingService, IBinRepository binRepository)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
            this.binRepository = binRepository;
        }

        public async Task<IEnumerable<Gallery>> Get(long GalleryId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.Id == (GalleryId == 0 ? x.Id : GalleryId)).ToList();
        }

        public async Task<Gallery> GetByGalleryType(long galleryTypeId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.GalleryTypeIDF == galleryTypeId).LastOrDefault();
        }

        public async Task<long> Save(Gallery gallery, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (gallery.Id > 0)
                    {
                        var result = await GetCachedList();
                        var oldGallery = result.Where(x => x.Id == gallery.Id).ToList().LastOrDefault();

                        if (oldGallery != null)
                        {
                            gallery.CreatedBy = oldGallery.CreatedBy;
                            gallery.CreatedOn = oldGallery.CreatedOn;
                            gallery.IsDeleted = oldGallery.IsDeleted;

                            gallery.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(gallery).State = EntityState.Modified;
                            unitOfWork.Context.Galleries.Update(gallery);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(gallery).State = EntityState.Detached;
                        }
                        else gallery.Id = 0;
                    }
                    else
                    {
                        gallery.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<Gallery>().Add(gallery);
                        unitOfWork.Commit();
                    }

                    if (gallery.Id > 0) RemoveCache();

                    return gallery.Id;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }

            
        }

        public async Task<bool> Delete(long galleryId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            Gallery gallery = result.Where(x => !x.IsDeleted && x.Id == galleryId).ToList().LastOrDefault();

            if (gallery != null)
            {
                using (transaction)
                {
                    try
                    {
                        gallery.UpdatedOn = DateTime.Now;
                        gallery.IsDeleted = true;
                        unitOfWork.Context.Entry(gallery).State = EntityState.Modified;
                        unitOfWork.Context.Galleries.Update(gallery);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(gallery).State = EntityState.Detached;

                        await binRepository.Add(galleryId, "Galleries", gallery.ItemName, deletedByIdentityId);

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

        private async Task<IEnumerable<Gallery>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "Galleries",
                () => unitOfWork.Context.Galleries.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("Galleries");
        }

    }
}