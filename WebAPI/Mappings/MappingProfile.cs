using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IConfiguration configuration)
        {
            //Default keys
            var UploadFolderURL = configuration["Utility:APIBaseURL"] + "/" + configuration["UploadFolders:UploadFolder"] + "/";
            var DefaultPictureURL = UploadFolderURL + configuration["UploadFolders:DefaultProfilePicture"];

            //Generic type mapping (for response)
            CreateMap(typeof(Stemma.Services.DTOs.Response.ResponseDTO<>), typeof(WebAPI.ResponseDTOs.ResponseDTO<>));

        }
    }
}
