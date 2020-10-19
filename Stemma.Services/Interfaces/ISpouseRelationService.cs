using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface ISpouseRelationService
    {


        Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>>>> Filter(string searchText, int pageNo, int pageSize);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.SpouseRelation>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Create(DTOs.Request.SpouseRelation model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Update(DTOs.Request.SpouseRelation model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Delete(long id);
    }
}
