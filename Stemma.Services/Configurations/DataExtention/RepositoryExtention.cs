using Microsoft.Extensions.DependencyInjection;
using Stemma.Infrastructure.Interface;
using Stemma.Infrastructure.Repository;

namespace Stemma.Services.Configurations.DataExtention
{
    public static class RepositoryExtention
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IFileUploadRepository, FileUploadRepository>();
            services.AddScoped<IGalleryPersonRepository, GalleryPersonRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IGalleryTypeRepository, GalleryTypeRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonSpouseRepository, PersonSpouseRepository>();
            services.AddScoped<ISpouseRelationRepository, SpouseRelationRepository>();
            services.AddScoped<ISurnameRepository, SurnameRepository>();

            return services;
        }
    }
}
