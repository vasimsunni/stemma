using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Interface;

namespace Stemma.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserRepository(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SignInResult> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return SignInResult.Failed;

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return SignInResult.Failed;

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: true);
            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            return result;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await unitOfWork.Context.Users.ToListAsync();
        }
        public async Task<ApplicationUser> Get(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetByUsername(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> Add(ApplicationUser applicationUser, string password, string[] roles)
        {
            try
            {
                applicationUser.LockoutEnabled = false;
                applicationUser.IsDeleted = false;
                applicationUser.CreatedOn = DateTime.Now;

                var result = await userManager.CreateAsync(applicationUser, password);
                if (result.Succeeded)
                {
                    foreach (var role in roles)
                    {
                        result = await userManager.AddToRoleAsync(applicationUser, role);
                        if (!result.Succeeded) break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> Update(ApplicationUser applicationUser, string[] roles)
        {
            applicationUser.UpdatedOn = DateTime.Now;
            var result = await userManager.UpdateAsync(applicationUser);

            if (result.Succeeded)
            {
                var oldRoles = await userManager.GetRolesAsync(applicationUser);

                if (oldRoles.Any())
                {
                    result = await userManager.RemoveFromRolesAsync(applicationUser, oldRoles);                    
                }

                if (result.Succeeded)
                {
                    foreach (var role in roles)
                    {
                        result = await userManager.AddToRoleAsync(applicationUser, role);
                        if (!result.Succeeded) break;
                    }
                }
            }

            return result;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return false;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? result.Succeeded : false;
        }

        public async Task<bool> ResetPassword(ApplicationUser applicationUser, string code, string password)
        {
            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null) return false;

            var result = await userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded) return result.Succeeded;

            return false;
        }
        public async Task<bool> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<IList<string>> GetRole(ApplicationUser applicationUser)
        {
            var role = await userManager.GetRolesAsync(applicationUser);
            return role;
        }
        public async Task<bool> Delete(string userId, string deletedBy)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IsDeleted = true;
            user.UpdatedBy = deletedBy;
            user.UpdatedOn = DateTime.Now;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded ? result.Succeeded : false;
        }
        public async Task<bool> IsEmailExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) return true;

            return false;
        }
        public async Task<string> GetFullName(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null) return user.FirstName + " " + user.LastName;

            return string.Empty;
        }

        public async Task<ApplicationUser> Activate(string userId, bool active)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = active;
                user.UpdatedOn = DateTime.Now;
            }
            return user;
        }
    }
}
