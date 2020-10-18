using System.Threading.Tasks;

namespace Stemma.Services.Interfaces
{
    public interface ILoginService
    {
        Task<DTOs.Response.ResponseDTO<DTOs.Response.Login>> Login(DTOs.Request.Login model);
        Task<DTOs.Response.ResponseDTO<string>> Logout();
    }
}
