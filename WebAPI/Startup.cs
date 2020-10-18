using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Stemma.Services.Configurations.DataExtention;
using Stemma.Services.Configurations.MIddlewares;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDBContext(Configuration);
            services.RegisterRepositories();
            services.RegisterServices();
            services.ConfigureEmailSender(Configuration);
            services.ConfigureJWT(Configuration);
            services.ConfigureCORS(Configuration);
            services.ConfigureSwagger();

            #region Mappings
            //Automapper
            //Registering and Initializing AutoMapper
            var mappingconfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Stemma.Services.Mappings.MappingProfile(Configuration));
                cfg.AddProfile(new WebAPI.Mappings.MappingProfile(Configuration));
            });
            var mapper = mappingconfig.CreateMapper();
            services.TryAddSingleton(mapper);
            #endregion

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc(options =>
            options.EnableEndpointRouting = false
            );

            services.GeneralConfigurations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<IPSafeListMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.EnableCORS();
            app.UseHttpsRedirection();

            app.EnableSwagger();

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
