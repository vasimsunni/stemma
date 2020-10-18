using Microsoft.Extensions.DependencyInjection;
using Stemma.Infrastructure.Caching;
using Stemma.Services.Interfaces;
using Stemma.Services.Services;
using Stemma.Services.Utilities.UserResolver;

namespace Stemma.Services.Configurations.DataExtention
{
    public static class ServiceExtention
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICachingService, CachingService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserResolverService, UserResolverService>();
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            return services;
        }
    }
}
