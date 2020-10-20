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



            //CreateMap<FileUpload, FileUploadRequestDTO>()
            //   .ForMember(dest => dest.Id, source => source.MapFrom(src => src.FileId))
            //   .ReverseMap();

            CreateMap<Stemma.Core.Administrator, DTOs.Response.Administrator>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.AdminId))
                .ReverseMap();

            CreateMap<Stemma.Core.Administrator, DTOs.Request.Administrator>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.AdminId))
               .ReverseMap();

            CreateMap<Stemma.Core.Person, DTOs.Response.Person>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.PersonId))
               .ReverseMap();

            CreateMap<Stemma.Core.Person, DTOs.Request.Person>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.PersonId))
               .ReverseMap();

            CreateMap<Stemma.Core.Surname, DTOs.Response.Surname>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.SurnameId))
               .ReverseMap();

            CreateMap<Stemma.Core.Surname, DTOs.Request.Surname>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.SurnameId))
               .ReverseMap();

            CreateMap<Stemma.Core.SpouseRelation, DTOs.Response.SpouseRelation>()
              .ForMember(dest => dest.Id, source => source.MapFrom(src => src.SpouseRelationId))
              .ReverseMap();

            CreateMap<Stemma.Core.SpouseRelation, DTOs.Request.SpouseRelation>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.SpouseRelationId))
               .ReverseMap();

            CreateMap<Stemma.Core.GalleryType, DTOs.Response.GalleryType>()
              .ForMember(dest => dest.Id, source => source.MapFrom(src => src.GalleryTypeId))
              .ReverseMap();

            CreateMap<Stemma.Core.GalleryType, DTOs.Request.GalleryType>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.GalleryTypeId))
               .ReverseMap();

            CreateMap<Stemma.Core.Gallery, DTOs.Response.Gallery>()
             .ForMember(dest => dest.Id, source => source.MapFrom(src => src.GalleryId))
             .ReverseMap();

            CreateMap<Stemma.Core.Gallery, DTOs.Request.Gallery>()
               .ForMember(dest => dest.Id, source => source.MapFrom(src => src.GalleryId))
               .ReverseMap();
        }
    }
}
