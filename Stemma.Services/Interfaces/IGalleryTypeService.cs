using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface IGalleryTypeService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryType>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Create(DTOs.Request.GalleryType model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Update(DTOs.Request.GalleryType model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Delete(long id);
    }
}
