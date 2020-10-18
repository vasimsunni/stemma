using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Stemma.Services.Interfaces;
using WebAPI.Helpers;
using WebAPI.RequestDTOs;
using WebAPI.ResponseDTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Super Admin")]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService administratorService;
        private readonly IMapper mapper;

        public AdministratorController(IAdministratorService administratorService, IMapper mapper)
        {
            this.administratorService = administratorService;
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> Filter([FromQuery] PagingRequestDTO model)
        {
            var serviceResponse = await administratorService.Filter(model.SearchText, model.PageNo, model.PageSize);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(0, long.MaxValue)] long id)
        {
            var serviceResponse = await administratorService.Get(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Stemma.Services.DTOs.Request.Administrator model)
        {
            var serviceResponse = await administratorService.Create(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Stemma.Services.DTOs.Request.Administrator model)
        {
            var serviceResponse = await administratorService.Update(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Activate")]
        public async Task<IActionResult> Activate([Range(1, long.MaxValue)] long id, bool activate)
        {
            var serviceResponse = await administratorService.Activate(id, activate);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([Range(1, long.MaxValue)] long id)
        {
            var serviceResponse = await administratorService.Delete(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}
