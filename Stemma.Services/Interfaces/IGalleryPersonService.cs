using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface IGalleryPersonService
    {
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>> GetByGallery(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Create(DTOs.Request.GalleryPerson model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Update(DTOs.Request.GalleryPerson model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Delete(long id);
    }
}
