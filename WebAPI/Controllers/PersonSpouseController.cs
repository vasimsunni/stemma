using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stemma.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.ResponseDTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Super Admin, Admin")]
    public class PersonSpouseController : ControllerBase
    {
        private readonly IPersonSpouseService personSpouseService;
        private readonly IMapper mapper;

        public PersonSpouseController(IPersonSpouseService personSpouseService, IMapper mapper)
        {
            this.personSpouseService = personSpouseService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(0, long.MaxValue)] long id)
        {
            var serviceResponse = await personSpouseService.Get(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("GetByPerson/{personId}")]
        public async Task<IActionResult> GetByPerson([Range(0, long.MaxValue)] long personId)
        {
            var serviceResponse = await personSpouseService.Get(personId);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Stemma.Services.DTOs.Request.PersonSpouse model)
        {
            var serviceResponse = await personSpouseService.Create(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Stemma.Services.DTOs.Request.PersonSpouse model)
        {
            var serviceResponse = await personSpouseService.Update(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([Range(1, long.MaxValue)] long id)
        {
            var serviceResponse = await personSpouseService.Delete(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}
