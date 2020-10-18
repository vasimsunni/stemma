using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Stemma.Core;
using Stemma.Infrastructure;
using Stemma.Services.Configurations.Application;

namespace Stemma.Services.Configurations.DataExtention
{
    public static class DBContextConfiguration
    {
        public static IServiceCollection ConfigureDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUserTwoFactorTokenProvider<ApplicationUser>, DataProtectorTokenProvider<ApplicationUser>>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                //Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configuration.AppSettings().AccountLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = configuration.AppSettings().AccountLockoutMaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = false;
                //User Settings
                options.User.RequireUniqueEmail = false;
                //Default Signin Settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<ApplicationUser>)));
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<DataProtectionTokenProviderOptions>(o =>
            {
                o.Name = "Default";
                o.TokenLifespan = TimeSpan.FromHours(1);
            });
            services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(configuration.AppSettings().ConnectionString));

            return services;
        }
    }
}
