using AutoMapper;
using Stemma.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.ResponseDTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IMapper mapper;

        public LoginController(ILoginService loginService, IMapper mapper)
        {
            this.loginService = loginService;
            this.mapper = mapper;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(Stemma.Services.DTOs.Request.Login model)
        {
            var serviceResponse = await loginService.Login(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            var serviceResponse = await loginService.Logout();
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}