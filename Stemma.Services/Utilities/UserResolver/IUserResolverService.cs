using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Services.DTOs.Response;

namespace Stemma.Services.Utilities.UserResolver
{
    public interface IUserResolverService
    {
        Task<ApplicationUser> GetCurrentUser();
        Task<UserDetails> GetUserDetails(string identityUserId);
    }
}
