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
    public class GalleryTypeController : ControllerBase
    {
        private readonly IGalleryTypeService galleryTypeService;
        private readonly IMapper mapper;

        public GalleryTypeController(IGalleryTypeService galleryTypeService, IMapper mapper)
        {
            this.galleryTypeService = galleryTypeService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(0, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryTypeService.Get(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Stemma.Services.DTOs.Request.GalleryType model)
        {
            var serviceResponse = await galleryTypeService.Create(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Stemma.Services.DTOs.Request.GalleryType model)
        {
            var serviceResponse = await galleryTypeService.Update(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([Range(1, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryTypeService.Delete(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}
