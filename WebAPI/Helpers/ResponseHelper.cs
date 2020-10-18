using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ResponseDTOs;

namespace WebAPI.Helpers
{
    public static class ResponseHelper<T>
    {
        public static ObjectResult GenerateResponse(ResponseDTO<T> response)
        {
            ObjectResult objectResult = new ObjectResult(response);

            if (response.StatusCode == 100)
                objectResult.StatusCode = StatusCodes.Status100Continue;
            else if (response.StatusCode == 101)
                objectResult.StatusCode = StatusCodes.Status101SwitchingProtocols;
            else if (response.StatusCode == 102)
                objectResult.StatusCode = StatusCodes.Status102Processing;
            else if (response.StatusCode == 200)
                objectResult.StatusCode = StatusCodes.Status200OK;
            else if (response.StatusCode == 201)
                objectResult.StatusCode = StatusCodes.Status201Created;
            else if (response.StatusCode == 202)
                objectResult.StatusCode = StatusCodes.Status202Accepted;
            else if (response.StatusCode == 203)
                objectResult.StatusCode = StatusCodes.Status203NonAuthoritative;
            else if (response.StatusCode == 204)
                objectResult.StatusCode = StatusCodes.Status204NoContent;
            else if (response.StatusCode == 205)
                objectResult.StatusCode = StatusCodes.Status205ResetContent;
            else if (response.StatusCode == 206)
                objectResult.StatusCode = StatusCodes.Status206PartialContent;
            else if (response.StatusCode == 207)
                objectResult.StatusCode = StatusCodes.Status207MultiStatus;
            else if (response.StatusCode == 208)
                objectResult.StatusCode = StatusCodes.Status208AlreadyReported;
            else if (response.StatusCode == 226)
                objectResult.StatusCode = StatusCodes.Status226IMUsed;
            else if (response.StatusCode == 300)
                objectResult.StatusCode = StatusCodes.Status300MultipleChoices;
            else if (response.StatusCode == 301)
                objectResult.StatusCode = StatusCodes.Status301MovedPermanently;
            else if (response.StatusCode == 302)
                objectResult.StatusCode = StatusCodes.Status302Found;
            else if (response.StatusCode == 303)
                objectResult.StatusCode = StatusCodes.Status303SeeOther;
            else if (response.StatusCode == 304)
                objectResult.StatusCode = StatusCodes.Status304NotModified;
            else if (response.StatusCode == 305)
                objectResult.StatusCode = StatusCodes.Status305UseProxy;
            else if (response.StatusCode == 306)
                objectResult.StatusCode = StatusCodes.Status306SwitchProxy;
            else if (response.StatusCode == 307)
                objectResult.StatusCode = StatusCodes.Status307TemporaryRedirect;
            else if (response.StatusCode == 308)
                objectResult.StatusCode = StatusCodes.Status308PermanentRedirect;
            else if (response.StatusCode == 400)
                objectResult.StatusCode = StatusCodes.Status400BadRequest;
            else if (response.StatusCode == 401)
                objectResult.StatusCode = StatusCodes.Status401Unauthorized;
            else if (response.StatusCode == 402)
                objectResult.StatusCode = StatusCodes.Status402PaymentRequired;
            else if (response.StatusCode == 403)
                objectResult.StatusCode = StatusCodes.Status403Forbidden;
            else if (response.StatusCode == 404)
                objectResult.StatusCode = StatusCodes.Status404NotFound;
            else if (response.StatusCode == 405)
                objectResult.StatusCode = StatusCodes.Status405MethodNotAllowed;
            else if (response.StatusCode == 406)
                objectResult.StatusCode = StatusCodes.Status406NotAcceptable;
            else if (response.StatusCode == 407)
                objectResult.StatusCode = StatusCodes.Status407ProxyAuthenticationRequired;
            else if (response.StatusCode == 408)
                objectResult.StatusCode = StatusCodes.Status408RequestTimeout;
            else if (response.StatusCode == 409)
                objectResult.StatusCode = StatusCodes.Status409Conflict;
            else if (response.StatusCode == 410)
                objectResult.StatusCode = StatusCodes.Status410Gone;
            else if (response.StatusCode == 411)
                objectResult.StatusCode = StatusCodes.Status411LengthRequired;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status412PreconditionFailed;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413RequestEntityTooLarge;
            else if (response.StatusCode == 414)
                objectResult.StatusCode = StatusCodes.Status414RequestUriTooLong;
            else if (response.StatusCode == 415)
                objectResult.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RangeNoStemmatisfiable;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RequestedRangeNoStemmatisfiable;
            else if (response.StatusCode == 417)
                objectResult.StatusCode = StatusCodes.Status417ExpectationFailed;
            else if (response.StatusCode == 418)
                objectResult.StatusCode = StatusCodes.Status418ImATeapot;
            else if (response.StatusCode == 419)
                objectResult.StatusCode = StatusCodes.Status419AuthenticationTimeout;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status421MisdirectedRequest;
            else if (response.StatusCode == 422)
                objectResult.StatusCode = StatusCodes.Status422UnprocessableEntity;
            else if (response.StatusCode == 423)
                objectResult.StatusCode = StatusCodes.Status423Locked;
            else if (response.StatusCode == 424)
                objectResult.StatusCode = StatusCodes.Status424FailedDependency;
            else if (response.StatusCode == 426)
                objectResult.StatusCode = StatusCodes.Status426UpgradeRequired;
            else if (response.StatusCode == 428)
                objectResult.StatusCode = StatusCodes.Status428PreconditionRequired;
            else if (response.StatusCode == 429)
                objectResult.StatusCode = StatusCodes.Status429TooManyRequests;
            else if (response.StatusCode == 431)
                objectResult.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
            else if (response.StatusCode == 451)
                objectResult.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;
            else if (response.StatusCode == 500)
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
            else if (response.StatusCode == 501)
                objectResult.StatusCode = StatusCodes.Status501NotImplemented;
            else if (response.StatusCode == 502)
                objectResult.StatusCode = StatusCodes.Status502BadGateway;
            else if (response.StatusCode == 503)
                objectResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
            else if (response.StatusCode == 504)
                objectResult.StatusCode = StatusCodes.Status504GatewayTimeout;
            else if (response.StatusCode == 505)
                objectResult.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            else if (response.StatusCode == 506)
                objectResult.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
            else if (response.StatusCode == 507)
                objectResult.StatusCode = StatusCodes.Status507InsufficientStorage;
            else if (response.StatusCode == 508)
                objectResult.StatusCode = StatusCodes.Status508LoopDetected;
            else if (response.StatusCode == 510)
                objectResult.StatusCode = StatusCodes.Status510NotExtended;
            else if (response.StatusCode == 511)
                objectResult.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
            else
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return objectResult;
        }
    }
}
