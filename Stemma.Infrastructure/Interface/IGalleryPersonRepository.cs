using Stemma.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Interface
{
    public interface IGalleryPersonRepository
    {
        Task<IEnumerable<GalleryPerson>> Get(long galleryPersonId);
        Task<GalleryPerson> GetByGallery(long galleryId);
        Task<GalleryPerson> GetByPerson(long personId);
        Task<long> Save(GalleryPerson galleryPerson, IDatabaseTransaction transaction);
        Task<bool> Delete(long galleryPersonId, string deletedByIdentityId, IDatabaseTransaction transaction);

        Task<long> TotalPerson(long galleryId);
    }
}
