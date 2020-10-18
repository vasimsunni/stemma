
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stemma.Services.Configurations.Application;
using Stemma.Services.Services;

namespace Stemma.Services.Configurations.DataExtention
{
    public static class EmailSenderConfiguration
    {
        public static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>(i =>
                  new EmailSender(
                   configuration.AppSettings().EmailHost,
                   configuration.AppSettings().EmailPort,
                   configuration.AppSettings().EmailEnableSSL,
                   configuration.AppSettings().EmailUserName,
                   configuration.AppSettings().EmailPassword
                 )
            );

            return services;
        }
    }
}
