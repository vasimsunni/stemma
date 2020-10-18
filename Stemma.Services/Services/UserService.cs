using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;

namespace Stemma.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<SignInResult> Authenticate(string username, string password) { return await userRepository.Authenticate(username, password); }
        public async Task<IEnumerable<ApplicationUser>> GetAll() => await userRepository.GetAll();
        public async Task<ApplicationUser> Get(string userId) => await userRepository.Get(userId);
        public async Task<ApplicationUser> GetByEmail(string email) => await userRepository.GetByEmail(email);
        public async Task<ApplicationUser> GetByUsername(string username) => await userRepository.GetByUsername(username);
        public async Task<IdentityResult> Add(ApplicationUser user, string password, string[] roles) => await userRepository.Add(user, password, roles);
        public async Task<IdentityResult> Update(ApplicationUser applicationUser, string[] roles) => await userRepository.Update(applicationUser, roles);
        public async Task<bool> ConfirmEmail(string userId, string code) => await userRepository.ConfirmEmail(userId, code);
        public async Task<bool> ResetPassword(ApplicationUser applicationUser, string code, string password) => await userRepository.ResetPassword(applicationUser, code, password);
        public async Task<bool> ChangePassword(string email, string oldPassword, string newPassword) => await userRepository.ChangePassword(email, oldPassword, newPassword);
        public async Task<IList<string>> GetRole(ApplicationUser applicationUser) => await userRepository.GetRole(applicationUser);
        public async Task<bool> Delete(string userId, string deletedBy) => await userRepository.Delete(userId, deletedBy);
        public async Task<bool> IsEmailExists(string email) => await userRepository.IsEmailExists(email);
        public async Task<string> GetFullName(string id) => await userRepository.GetFullName(id);
        public async Task<ApplicationUser> Activate(string userId, bool active) => await userRepository.Activate(userId, active);
    }
}
