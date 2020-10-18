using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.DTOs.Response;

namespace Stemma.Services.Utilities.UserResolver
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IAdministratorRepository administratorRepository;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly IConfiguration configuration;

        public UserResolverService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager,
            IUserRepository userRepository, IAdministratorRepository administratorRepository,
            IFileUploadRepository fileUploadRepository, IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.administratorRepository = administratorRepository;
            this.fileUploadRepository = fileUploadRepository;
            this.configuration = configuration;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            return await userManager.GetUserAsync(context.HttpContext.User);
        }

        public async Task<UserDetails> GetUserDetails(string identityUserId)
        {
            var UploadFolderURL = configuration["Utility:APIBaseURL"] + "/" + configuration["UploadFolders:UploadFolder"] + "/";
            var DefaultPictureURL = UploadFolderURL + configuration["UploadFolders:DefaultProfilePicture"];

            var user = await userRepository.Get(identityUserId);

            if (user != null)
            {
                var userRoles = await userRepository.GetRole(user);

                if (userRoles.Any())
                {
                    long UserId = 0;
                    string FullName = "", PictureName = "", PictureURL = "", Email = "", MobileNumber = "";
                    string[] Roles;


                    var userPrimaryRole = userRoles.FirstOrDefault();

                    Roles = userRoles.ToArray();

                    if (userPrimaryRole == "Super Admin" || userPrimaryRole == "Admin")
                    {
                        var administrator = await administratorRepository.GetByIdentityId(user.Id);

                        if (administrator != null)
                        {
                            UserId = administrator.AdminId;
                            FullName = administrator.FirstName + " " + administrator.LastName;
                            Email = administrator.Email;
                            MobileNumber = administrator.ContactNumbers;
                        }

                        var fileUploads = await fileUploadRepository.GetByModule(administrator.AdminId, "AdminProfilePicture");

                        if (fileUploads.Any())
                        {
                            PictureName = fileUploads.LastOrDefault().Name;
                        }
                    }
                    else if (userPrimaryRole == "Client")
                    {

                    }

                    PictureURL = !string.IsNullOrEmpty(PictureName) ? UploadFolderURL + PictureName : DefaultPictureURL;

                    return new UserDetails
                    {
                        FullName = FullName,
                        Email = Email,
                        ContactNumbers = MobileNumber,
                        IdentityUserId = identityUserId,
                        UserId = UserId,
                        ProfilePictureURL = PictureURL,
                        Roles = Roles
                    };
                }
            }

            return null;
        }
    }
}
