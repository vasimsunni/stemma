using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stemma.Services.Helpers;

namespace Stemma.Services.Configurations.DataExtention
{
    public static class GeneralConfiguration
    {
        public static IServiceCollection GeneralConfigurations(this IServiceCollection services)
        {
            //Extend Model Validation with Response Helper

            services.AddMvc(options =>
            {
                //Custom Authorization Failure Response

                //options.Filters.Add(new CustomAuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));

            }).ConfigureApiBehaviorOptions(options =>
            {

                //InvalidModelStateResponseFactory is a Func delegate and used to customize the error response.    
                //It is exposed as property of ApiBehaviorOptions class that is used to configure api behaviour.    
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    List<string> Errors = actionContext.ModelState.Where(modelError => modelError.Value.Errors.Count > 0).Select(modelError => modelError.Value.Errors.FirstOrDefault().ErrorMessage).ToList();

                    ObjectResult objectResult = new ObjectResult(new ModelStateErrorHelper(Errors).ErrorResponse);
                    objectResult.StatusCode = StatusCodes.Status400BadRequest;

                    return objectResult;
                };
            });


            return services;
        }
    }
}
