using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stemma.Core;

namespace Stemma.Services.Interfaces
{
    public interface IUserService
    {
        Task<SignInResult> Authenticate(string username, string password);
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<ApplicationUser> Get(string userId);
        Task<ApplicationUser> GetByEmail(string email);
        Task<ApplicationUser> GetByUsername(string username);
        Task<IdentityResult> Add(ApplicationUser user, string password, string[] roles);
        Task<IdentityResult> Update(ApplicationUser applicationUser, string[] roles);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ResetPassword(ApplicationUser applicationUser, string code, string password);
        Task<bool> ChangePassword(string email, string oldPassword, string newPassword);
        Task<IList<string>> GetRole(ApplicationUser applicationUser);
        Task<bool> Delete(string userId, string deletedBy);
        Task<bool> IsEmailExists(string email);
        Task<string> GetFullName(string id);
        Task<ApplicationUser> Activate(string userId, bool active);
    }
}
