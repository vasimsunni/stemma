using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ResponseDTOs;

namespace WebAPI.Helpers
{
    public class CustomAuthorizeFilter
    {
        public AuthorizationPolicy Policy { get; }

        public CustomAuthorizeFilter(AuthorizationPolicy policy)
        {
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Allow Anonymous skips all authorization
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
            {
                // Return custom 401 result
                ResponseDTO<string> responseDTO = new ResponseDTO<string>();
                responseDTO.StatusCode = 401;
                responseDTO.Message = "Unauthorised User.";


                context.Result = ResponseHelper<string>.GenerateResponse(responseDTO);
            }
            else if (authorizeResult.Forbidden)
            {
                // Return default 403 result
                ResponseDTO<string> responseDTO = new ResponseDTO<string>();
                responseDTO.StatusCode = 403;
                responseDTO.Message = "You are not authorized to perform this task.";

                context.Result = ResponseHelper<string>.GenerateResponse(responseDTO);
            }
        }

    }
}
