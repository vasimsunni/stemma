using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface IGalleryService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> GetByGalleryType(long galleryTypeId);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> GetByPerson(long personId);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Create(DTOs.Request.Gallery model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Update(DTOs.Request.Gallery model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Delete(long id);
    }
}
