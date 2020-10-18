using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Stemma.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IConfiguration configuration)
        {
            //Default keys
            var UploadFolderURL = configuration["Utility:APIBaseURL"] + "/" + configuration["UploadFolders:UploadFolder"] + "/";
            var DefaultPictureURL = UploadFolderURL + configuration["UploadFolders:DefaultProfilePicture"];

           // Generic type mapping(for paging)
                CreateMap(typeof(Infrastructure.DTOs.PagedResult<>), typeof(DTOs.Response.PaginatedResponse<>)).ReverseMap();

            //User Details
            //CreateMap<UserDetailsDTO, UserDetailsResponseDTO>()
            //    .ForMember(dest => dest.ProfilePictureURL, source => source.MapFrom(src => src.ProfilePictureName.Trim() == "" ? DefaultPictureURL : UploadFolderURL + src.ProfilePictureName));

            CreateMap<Stemma.Core.Administrator, DTOs.Response.Administrator>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.AdminId))
                .ReverseMap();

            CreateMap<Stemma.Core.Administrator, DTOs.Request.Administrator>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.AdminId))
               .ReverseMap();

            //CreateMap<FileUpload, FileUploadRequestDTO>()
            //   .ForMember(dest => dest.Id, source => source.MapFrom(src => src.FileId))
            //   .ReverseMap();
        }
    }
}
