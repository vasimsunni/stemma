using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Configurations.MIddlewares
{
    public class IPSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IPSafeListMiddleware> _logger;

        public IPSafeListMiddleware(RequestDelegate next, ILogger<IPSafeListMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                //Define the whitelisted IP here. Get from DB or configuration.
                string[] saferIPList=new string[0];

                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);


                if (saferIPList.Count() > 0)
                {
                    var bytes = remoteIp.GetAddressBytes();
                    var badIp = true;
                    foreach (var address in saferIPList)
                    {
                        var testIp = IPAddress.Parse(address);
                        if (testIp.GetAddressBytes().SequenceEqual(bytes))
                        {
                            badIp = false;
                            break;
                        }
                    }

                    if (badIp)
                    {
                        _logger.LogWarning(
                            "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}
