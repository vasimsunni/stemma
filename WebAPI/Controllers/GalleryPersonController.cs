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
    public class GalleryPersonController : ControllerBase
    {
        private readonly IGalleryPersonService galleryPersonService;
        private readonly IMapper mapper;

        public GalleryPersonController(IGalleryPersonService galleryPersonService, IMapper mapper)
        {
            this.galleryPersonService = galleryPersonService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(0, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryPersonService.Get(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("GetByGallery/{galleryPersonId}")]
        public async Task<IActionResult> GetByGalleryPersonType([Range(0, long.MaxValue)] long galleryPersonId)
        {
            var serviceResponse = await galleryPersonService.GetByGallery(galleryPersonId);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Stemma.Services.DTOs.Request.GalleryPerson model)
        {
            var serviceResponse = await galleryPersonService.Create(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Stemma.Services.DTOs.Request.GalleryPerson model)
        {
            var serviceResponse = await galleryPersonService.Update(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([Range(1, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryPersonService.Delete(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}
