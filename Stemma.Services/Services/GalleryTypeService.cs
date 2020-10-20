using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stemma.Services.Services
{
    public class GalleryTypeService : IGalleryTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IGalleryTypeRepository galleryTypeRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public GalleryTypeService(IUnitOfWork unitOfWork,
                       UserManager<ApplicationUser> userManager,
                       IUserRepository userRepository,
                       IGalleryTypeRepository galleryTypeRepository,
                       IGalleryRepository galleryRepository,
                       IMapper mapper,
                       IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.galleryTypeRepository = galleryTypeRepository;
            this.galleryRepository = galleryRepository;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryTypeRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.GalleryType>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    foreach (var galleryType in filteredResponse.Records)
                    {
                        galleryType.NoOfItem = await galleryRepository.TotalByGalleryType(galleryType.Id);

                        if (!string.IsNullOrEmpty(galleryType.CreatedBy)) galleryType.CreatedBy = await userRepository.GetFullName(galleryType.CreatedBy);
                        if (!string.IsNullOrEmpty(galleryType.UpdatedBy)) galleryType.UpdatedBy = await userRepository.GetFullName(galleryType.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = filteredResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = filteredResponse;
                    Message = "No Data found.";
                }
            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryType>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryType>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryType>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.GalleryType> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryTypeRepository.Get(id);

                var personResponse = mapper.Map<IEnumerable<DTOs.Response.GalleryType>>(filteredResult);

                if (personResponse.Any())
                {
                    foreach (var galleryType in personResponse)
                    {
                        galleryType.NoOfItem = await galleryRepository.TotalByGalleryType(galleryType.Id);

                        if (!string.IsNullOrEmpty(galleryType.CreatedBy)) galleryType.CreatedBy = await userRepository.GetFullName(galleryType.CreatedBy);
                        if (!string.IsNullOrEmpty(galleryType.UpdatedBy)) galleryType.UpdatedBy = await userRepository.GetFullName(galleryType.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = personResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = personResponse;
                    Message = "No Data found.";
                }
            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Create(DTOs.Request.GalleryType model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryType> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryType Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var person = mapper.Map<Stemma.Core.GalleryType>(model);

                    person.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await galleryTypeRepository.Save(person, transaction);

                    if (model.Id > 0)
                    {
                        transaction.Commit();

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Gallery Type added successfully.";
                    }
                    else
                    {
                        StatusCode = 500;
                        Message = "Failed while saving.";
                    }
                }
                else
                {
                    StatusCode = 403;
                    Message = "Unauthorise Acccess.";
                }
            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Update(DTOs.Request.GalleryType model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryType> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryType Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var galleryTypes = await galleryTypeRepository.Get(model.Id);

                        if (galleryTypes.Any())
                        {
                            var galleryType = galleryTypes.LastOrDefault();

                            galleryType = mapper.Map<Stemma.Core.GalleryType>(model);

                            galleryType.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await galleryTypeRepository.Save(galleryType, transaction);

                            if (model.Id > 0)
                            {
                                transaction.Commit();

                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Gallery Type updated successfully.";
                            }
                            else
                            {
                                StatusCode = 500;
                                Message = "Failed while saving.";
                            }

                        }
                        else
                        {
                            StatusCode = 404;
                            Message = "GalleryType not found";
                        }
                    }
                    else
                    {
                        StatusCode = 406;
                        Message = "Not Acceptable.";
                    }
                }
                else
                {
                    StatusCode = 405;
                    Message = "Unauthorized access.";
                }

            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryType> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryType>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryType Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var people = await galleryTypeRepository.Get(id);

                        if (people.Any())
                        {
                            var person = people.LastOrDefault();


                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await galleryTypeRepository.Delete(person.GalleryTypeId, currentUser.Id, transaction);

                            if (isDeleted)
                            {
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Gallery Type deleted successfully.";
                            }
                            else
                            {
                                StatusCode = 500;
                                Message = "Failed while deleting.";
                            }

                        }
                        else
                        {
                            StatusCode = 404;
                            Message = "GalleryType not found";
                        }
                    }
                    else
                    {
                        StatusCode = 406;
                        Message = "Not Acceptable.";
                    }
                }
                else
                {
                    StatusCode = 405;
                    Message = "Unathorized access";
                }


            }
            catch (Exception ex)
            {
                StatusCode = 500;
                Message = "Failed";
                ExceptionMessage = ex.ToString();
            }

            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Data = Data;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;

            return response;
        }
    }
}
