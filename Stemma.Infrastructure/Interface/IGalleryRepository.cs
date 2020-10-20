using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface IGalleryRepository
    {
        Task<PagedResult<Gallery>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<Gallery>> Get(long galleryId);
        Task<IEnumerable<Gallery>> GetByGalleryType(long galleryTypeId);
        Task<IEnumerable<Gallery>> GetByPerson(long personId);
        Task<Gallery> GetPersonProfilePicture(long personId);
        Task<long> Save(Gallery gallery, IDatabaseTransaction transaction);
        Task<bool> Delete(long galleryId, string deletedByIdentityId, IDatabaseTransaction transaction);

        Task<long> TotalByGalleryType(long galleryTypeId);
    }
}
