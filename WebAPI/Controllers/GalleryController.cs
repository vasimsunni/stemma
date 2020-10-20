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
using WebAPI.RequestDTOs;
using WebAPI.ResponseDTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Super Admin, Admin")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService galleryService;
        private readonly IMapper mapper;

        public GalleryController(IGalleryService galleryService, IMapper mapper)
        {
            this.galleryService = galleryService;
            this.mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Filter([FromQuery] PagingRequestDTO model)
        {
            var serviceResponse = await galleryService.Filter(model.SearchText, model.PageNo, model.PageSize);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(0, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryService.Get(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("GetByGalleryType/{galleryTypeId}")]
        public async Task<IActionResult> GetByGalleryType([Range(0, long.MaxValue)] long galleryTypeId)
        {
            var serviceResponse = await galleryService.GetByGalleryType(galleryTypeId);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpGet("GetByPerson/{personId}")]
        public async Task<IActionResult> GetByPerson([Range(0, long.MaxValue)] long personId)
        {
            var serviceResponse = await galleryService.GetByPerson(personId);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Stemma.Services.DTOs.Request.Gallery model)
        {
            var serviceResponse = await galleryService.Create(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Stemma.Services.DTOs.Request.Gallery model)
        {
            var serviceResponse = await galleryService.Update(model);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([Range(1, long.MaxValue)] long id)
        {
            var serviceResponse = await galleryService.Delete(id);
            var response = mapper.Map<ResponseDTO<object>>(serviceResponse);
            return ResponseHelper<object>.GenerateResponse(response);
        }
    }
}
