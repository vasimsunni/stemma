using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Stemma.Services.Utilities.UserResolver;
using Stemma.Services.Configurations.Application;

namespace Stemma.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAdministratorRepository administratorRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserResolverService userResolverService;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContext;
        private readonly IConfiguration configuration;

        public LoginService(IAdministratorRepository administratorRepository, IUserRepository userRepository,
            IUserResolverService userResolverService, IFileUploadRepository fileUploadRepository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContext, IConfiguration configuration)
        {
            this.administratorRepository = administratorRepository;
            this.userRepository = userRepository;
            this.userResolverService = userResolverService;
            this.fileUploadRepository = fileUploadRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContext = httpContext;
            this.configuration = configuration;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Login>> Login(DTOs.Request.Login model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Login> response = new DTOs.Response.ResponseDTO<DTOs.Response.Login>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Login Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var signInResult = await userRepository.Authenticate(model.Username, model.Password);

                if (signInResult.Succeeded)
                {
                    var applicationUser = await userRepository.GetByUsername(model.Username);

                    if (applicationUser != null)
                    {  
                        if (applicationUser.IsActive == false)
                        {
                            isSuccess = false;
                            StatusCode = 400;
                            Message = "Account not active.";
                        }
                        else if (applicationUser.IsDeleted)
                        {
                            isSuccess = false;
                            StatusCode = 400;
                            Message = "Account deleted.";
                        }
                        else
                        {
                            if (!applicationUser.EmailConfirmed || !applicationUser.PhoneNumberConfirmed)
                            {
                                isSuccess = false;
                                StatusCode = 400;
                                Message = "Account not verified yet.";
                            }
                            else
                            {
                                var roles = await userRepository.GetRole(applicationUser);
                                
                                if (roles.Any(r => r == configuration["SystemRole:AdminRole"].ToString() || r == configuration["SystemRole:SuperAdminRole"].ToString()))
                                {
                                    var tokenString = await GenerateJSONWebToken(applicationUser);

                                    var loginResponse = new DTOs.Response.Login
                                    {
                                        Token = tokenString,
                                        UserDetails = await userResolverService.GetUserDetails(applicationUser.Id),
                                    };

                                    Data = loginResponse;
                                    StatusCode = 200;
                                    isSuccess = true;
                                    Message = "SignIn successful.";
                                }
                                else
                                {
                                    StatusCode = 403;
                                    Message = "Unauthorized not found.";
                                }
                            }
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        StatusCode = 404;
                        Message = "User not found.";
                    }
                }
                else if (signInResult.IsLockedOut)
                {
                    StatusCode = 400;
                    Message = "SignIn disabled, please try again after 15 minutes.";
                }
                else
                {
                    var applicationUser = await userRepository.GetByUsername(model.Username);

                    if (applicationUser != null)
                    {
                        if (applicationUser.AccessFailedCount == 2)
                        {
                            isSuccess = false;
                            StatusCode = 400;
                            Message = "Last attempt to login, if failed, please try again after 15 minutes.";
                        }
                        else
                        {
                            isSuccess = false;
                            StatusCode = 400;
                            Message = "Invalid username or password.";
                        }
                    }
                    else
                    {
                        StatusCode = 404;
                        Message = "Invalid username.";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed.";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }

        public async Task<DTOs.Response.ResponseDTO<string>> Logout()
        {
            DTOs.Response.ResponseDTO<string> response = new DTOs.Response.ResponseDTO<string>();

            int StatusCode = 0;
            bool isSuccess = false;
            string Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null && !string.IsNullOrEmpty(currentUser.Id))
                {
                    await signInManager.SignOutAsync();
                    isSuccess = true;
                    Message = "User signed out successfully.";
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Failed.";
                    ExceptionMessage = "User not signed in, please sign in inorder to sign out.";
                }
            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }



        private async Task<string> GenerateJSONWebToken(ApplicationUser applicationUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRoles = await userRepository.GetRole(applicationUser);

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: configuration.AppSettings().JWTIssuer,
                audience: configuration.AppSettings().JWTAudience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(configuration.AppSettings().JWTExprationTimeOut),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
