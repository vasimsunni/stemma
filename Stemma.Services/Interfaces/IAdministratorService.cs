using System.Collections.Generic;
using System.Threading.Tasks;
using Stemma.Core;

namespace Stemma.Services.Interfaces
{
    public interface IAdministratorService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Administrator>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Create(DTOs.Request.Administrator model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Update(DTOs.Request.Administrator model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Activate(long id, bool activate);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Delete(long id);
    }
}
