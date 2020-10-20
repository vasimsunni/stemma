using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface IPersonSpouseService
    {
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>> Get(long id);
        Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>> GetByPerson(long personId);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Create(DTOs.Request.PersonSpouse model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Update(DTOs.Request.PersonSpouse model);
        Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Delete(long id);
    }
}
