using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface IPersonService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Person>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Create(DTOs.Request.Person model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Update(DTOs.Request.Person model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Delete(long id);
    }
}
