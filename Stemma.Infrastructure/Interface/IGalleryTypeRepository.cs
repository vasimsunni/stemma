using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface IGalleryTypeRepository
    {
        Task<PagedResult<GalleryType>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<GalleryType>> Get(long galleryTypeId);
        Task<long> Save(GalleryType galleryType, IDatabaseTransaction transaction);
        Task<bool> Delete(long galleryTypeId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
