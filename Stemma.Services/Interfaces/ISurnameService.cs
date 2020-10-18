using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
   public interface ISurnameService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Surname>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Create(DTOs.Request.Surname model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Update(DTOs.Request.Surname model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Delete(long id);
    }
}
