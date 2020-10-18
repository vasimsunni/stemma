using System.Collections.Generic;
using Stemma.Services.DTOs.Response;

namespace Stemma.Services.Helpers
{
    public class ModelStateErrorHelper
    {
        public ResponseDTO<string> ErrorResponse = new ResponseDTO<string>();
        public ModelStateErrorHelper(List<string> Errors)
        {
            ErrorResponse.StatusCode = 400;
            ErrorResponse.Message = string.Join(" ,", Errors);
        }
    }
}
