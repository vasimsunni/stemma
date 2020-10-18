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
        Task<IEnumerable<Gallery>> Get(long galleryId);
        Task<Gallery> GetByGalleryType(long galleryTypeId);
        Task<long> Save(Gallery gallery, IDatabaseTransaction transaction);
        Task<bool> Delete(long galleryId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
