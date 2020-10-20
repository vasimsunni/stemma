using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.Interface;

namespace Stemma.Infrastructure.Repository
{
    public class GalleryPersonRepository : IGalleryPersonRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;
        private readonly IBinRepository binRepository;

        public GalleryPersonRepository(IUnitOfWork unitOfWork, ICachingService cachingService)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
        }

        public async Task<IEnumerable<GalleryPerson>> Get(long galleryPersonId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.GalleryPersonId == (galleryPersonId == 0 ? x.GalleryPersonId : galleryPersonId)).ToList();
        }

        public async Task<GalleryPerson> GetByGallery(long galleryId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.GalleryIDF == galleryId).LastOrDefault();
        }

        public async Task<GalleryPerson> GetByPerson(long personId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.PersonIDF ==personId).LastOrDefault();
        }

        public async Task<long> Save(GalleryPerson galleryPerson, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (galleryPerson.GalleryPersonId > 0)
                    {
                        var result = await GetCachedList();
                        var oldGalleryPerson = result.Where(x => x.GalleryPersonId == galleryPerson.GalleryPersonId).ToList().LastOrDefault();

                        if (oldGalleryPerson != null)
                        {
                            galleryPerson.CreatedBy = oldGalleryPerson.CreatedBy;
                            galleryPerson.CreatedOn = oldGalleryPerson.CreatedOn;
                            galleryPerson.IsDeleted = oldGalleryPerson.IsDeleted;

                            galleryPerson.UpdatedOn = DateTime.Now;
                            unitOfWork.Context.Entry(galleryPerson).State = EntityState.Modified;
                            unitOfWork.Context.GalleryPeople.Update(galleryPerson);
                            unitOfWork.Commit();
                            unitOfWork.Context.Entry(galleryPerson).State = EntityState.Detached;
                        }
                        else galleryPerson.GalleryPersonId = 0;
                    }
                    else
                    {
                        galleryPerson.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<GalleryPerson>().Add(galleryPerson);
                        unitOfWork.Commit();
                    }

                    if (galleryPerson.GalleryPersonId > 0) RemoveCache();

                    return galleryPerson.GalleryPersonId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return 0;
            }            
        }

        public async Task<bool> Delete(long galleryPersonId, string deletedByIdentityId, IDatabaseTransaction transaction)
        {
            var result = await GetCachedList();
            GalleryPerson galleryPerson = result.Where(x => !x.IsDeleted && x.GalleryPersonId == galleryPersonId).ToList().LastOrDefault();

            if (galleryPerson != null)
            {
                using (transaction)
                {
                    try
                    {
                        galleryPerson.UpdatedOn = DateTime.Now;
                        galleryPerson.IsDeleted = true;
                        unitOfWork.Context.Entry(galleryPerson).State = EntityState.Modified;
                        unitOfWork.Context.GalleryPeople.Update(galleryPerson);
                        unitOfWork.Commit();
                        unitOfWork.Context.Entry(galleryPerson).State = EntityState.Detached;

                        await binRepository.Add(galleryPersonId, "GalleryPeople", "", deletedByIdentityId);

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

        public async Task<long> TotalPerson(long galleryId)
        {
            var result = await GetCachedList();
            return result.Where(x => !x.IsDeleted && x.GalleryIDF == galleryId).LongCount();
        }

        private async Task<IEnumerable<GalleryPerson>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "GalleryPeople",
                () => unitOfWork.Context.GalleryPeople.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("GalleryPeople");
        }

    }
}
