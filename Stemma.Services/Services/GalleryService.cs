using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Services.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IGalleryTypeRepository galleryTypeRepository;
        private readonly IGalleryPersonRepository galleryPersonRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public GalleryService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
                              IUserRepository userRepository, IGalleryTypeRepository galleryTypeRepository,
                              IGalleryPersonRepository galleryPersonRepository, IGalleryRepository galleryRepository,
                              IMapper mapper, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.galleryTypeRepository = galleryTypeRepository;
            this.galleryPersonRepository = galleryPersonRepository;
            this.galleryRepository = galleryRepository;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Gallery>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    foreach (var gallery in filteredResponse.Records)
                    {
                        gallery.NoOfPeople = await galleryPersonRepository.TotalPerson(gallery.Id);

                        if (!string.IsNullOrEmpty(gallery.CreatedBy)) gallery.CreatedBy = await userRepository.GetFullName(gallery.CreatedBy);
                        if (!string.IsNullOrEmpty(gallery.UpdatedBy)) gallery.UpdatedBy = await userRepository.GetFullName(gallery.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Gallery> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryRepository.Get(id);

                var galleryResponse = mapper.Map<IEnumerable<DTOs.Response.Gallery>>(filteredResult);

                if (galleryResponse.Any())
                {
                    foreach (var gallery in galleryResponse)
                    {
                        gallery.NoOfPeople = await galleryPersonRepository.TotalPerson(gallery.Id);

                        if (!string.IsNullOrEmpty(gallery.CreatedBy)) gallery.CreatedBy = await userRepository.GetFullName(gallery.CreatedBy);
                        if (!string.IsNullOrEmpty(gallery.UpdatedBy)) gallery.UpdatedBy = await userRepository.GetFullName(gallery.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = galleryResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = galleryResponse;
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> GetByGalleryType(long galleryTypeId)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Gallery> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryRepository.GetByGalleryType(galleryTypeId);

                var galleryResponse = mapper.Map<IEnumerable<DTOs.Response.Gallery>>(filteredResult);

                if (galleryResponse.Any())
                {
                    foreach (var gallery in galleryResponse)
                    {
                        gallery.NoOfPeople = await galleryPersonRepository.TotalPerson(gallery.Id);

                        if (!string.IsNullOrEmpty(gallery.CreatedBy)) gallery.CreatedBy = await userRepository.GetFullName(gallery.CreatedBy);
                        if (!string.IsNullOrEmpty(gallery.UpdatedBy)) gallery.UpdatedBy = await userRepository.GetFullName(gallery.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = galleryResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = galleryResponse;
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>> GetByPerson(long personId)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Gallery>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Gallery> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryRepository.GetByPerson(personId);

                var galleryResponse = mapper.Map<IEnumerable<DTOs.Response.Gallery>>(filteredResult);

                if (galleryResponse.Any())
                {
                    foreach (var gallery in galleryResponse)
                    {
                        gallery.NoOfPeople = await galleryPersonRepository.TotalPerson(gallery.Id);

                        if (!string.IsNullOrEmpty(gallery.CreatedBy)) gallery.CreatedBy = await userRepository.GetFullName(gallery.CreatedBy);
                        if (!string.IsNullOrEmpty(gallery.UpdatedBy)) gallery.UpdatedBy = await userRepository.GetFullName(gallery.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = galleryResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = galleryResponse;
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Create(DTOs.Request.Gallery model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Gallery> response = new DTOs.Response.ResponseDTO<DTOs.Response.Gallery>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Gallery Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var gallery = mapper.Map<Stemma.Core.Gallery>(model);

                    gallery.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await galleryRepository.Save(gallery, transaction);

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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Update(DTOs.Request.Gallery model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Gallery> response = new DTOs.Response.ResponseDTO<DTOs.Response.Gallery>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Gallery Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var galleries = await galleryRepository.Get(model.Id);

                        if (galleries.Any())
                        {
                            var gallery = galleries.LastOrDefault();

                            gallery = mapper.Map<Stemma.Core.Gallery>(model);

                            gallery.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await galleryRepository.Save(gallery, transaction);

                            if (model.Id > 0)
                            {
                                transaction.Commit();

                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Gallery updated successfully.";
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
                            Message = "Gallery not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Gallery>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Gallery> response = new DTOs.Response.ResponseDTO<DTOs.Response.Gallery>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Gallery Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var galleries = await galleryRepository.Get(id);

                        if (galleries.Any())
                        {
                            var gallery = galleries.LastOrDefault();

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await galleryRepository.Delete(gallery.GalleryId, currentUser.Id, transaction);

                            if (isDeleted)
                            {
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Gallery deleted successfully.";
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
                            Message = "Gallery not found";
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
