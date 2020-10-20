using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stemma.Services.Services
{
    public class GalleryPersonService : IGalleryPersonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IGalleryPersonRepository galleryPersonRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;


        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.GalleryPerson> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryPersonRepository.Get(id);

                var galleryPersonResponse = mapper.Map<IEnumerable<DTOs.Response.GalleryPerson>>(filteredResult);

                if (galleryPersonResponse.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var galleryPerson in galleryPersonResponse)
                    {
                        var people = await personRepository.Get(galleryPerson.PersonIDF);

                        if (people.Any())
                        {
                            galleryPerson.person = mapper.Map<DTOs.Response.Person>(people.LastOrDefault());

                            PictureURL = string.Empty;

                            var personGallery = await galleryRepository.GetPersonProfilePicture(galleryPerson.person.Id);

                            if (personGallery != null)
                            {
                                var fileUploads = await fileUploadRepository.Get(personGallery.GalleryId);

                                if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                if (string.IsNullOrEmpty(PictureURL))
                                {
                                    string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                    PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                }
                                else PictureURL = BaseURL + PictureURL;
                            }

                            galleryPerson.person.ProfilePictureURL = PictureURL;
                        }

                        if (!string.IsNullOrEmpty(galleryPerson.CreatedBy)) galleryPerson.CreatedBy = await userRepository.GetFullName(galleryPerson.CreatedBy);
                        if (!string.IsNullOrEmpty(galleryPerson.UpdatedBy)) galleryPerson.UpdatedBy = await userRepository.GetFullName(galleryPerson.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = galleryPersonResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = galleryPersonResponse;
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>> GetByGallery(long galleryId)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.GalleryPerson>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.GalleryPerson> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await galleryPersonRepository.GetByGallery(galleryId);

                var galleryPersonResponse = mapper.Map<IEnumerable<DTOs.Response.GalleryPerson>>(filteredResult);

                if (galleryPersonResponse.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var galleryPerson in galleryPersonResponse)
                    {
                        var people = await personRepository.Get(galleryPerson.PersonIDF);

                        if (people.Any())
                        {
                            galleryPerson.person = mapper.Map<DTOs.Response.Person>(people.LastOrDefault());

                            PictureURL = string.Empty;

                            var personGallery = await galleryRepository.GetPersonProfilePicture(galleryPerson.person.Id);

                            if (personGallery != null)
                            {
                                var fileUploads = await fileUploadRepository.Get(personGallery.GalleryId);

                                if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                if (string.IsNullOrEmpty(PictureURL))
                                {
                                    string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                    PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                }
                                else PictureURL = BaseURL + PictureURL;
                            }

                            galleryPerson.person.ProfilePictureURL = PictureURL;
                        }

                        if (!string.IsNullOrEmpty(galleryPerson.CreatedBy)) galleryPerson.CreatedBy = await userRepository.GetFullName(galleryPerson.CreatedBy);
                        if (!string.IsNullOrEmpty(galleryPerson.UpdatedBy)) galleryPerson.UpdatedBy = await userRepository.GetFullName(galleryPerson.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = galleryPersonResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = galleryPersonResponse;
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Create(DTOs.Request.GalleryPerson model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryPerson Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var person = mapper.Map<Stemma.Core.GalleryPerson>(model);

                    person.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await galleryPersonRepository.Save(person, transaction);

                    if (model.Id > 0)
                    {
                        transaction.Commit();

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Gallery Person added successfully.";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Update(DTOs.Request.GalleryPerson model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryPerson Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var galleryPersons = await galleryPersonRepository.Get(model.Id);

                        if (galleryPersons.Any())
                        {
                            var galleryPerson = galleryPersons.LastOrDefault();

                            galleryPerson = mapper.Map<Stemma.Core.GalleryPerson>(model);

                            galleryPerson.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await galleryPersonRepository.Save(galleryPerson, transaction);

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
                            Message = "GalleryPerson not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson> response = new DTOs.Response.ResponseDTO<DTOs.Response.GalleryPerson>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.GalleryPerson Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var galleryPeople = await galleryPersonRepository.Get(id);

                        if (galleryPeople.Any())
                        {
                            var galleryPerson = galleryPeople.LastOrDefault();

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await galleryPersonRepository.Delete(galleryPerson.GalleryPersonId, currentUser.Id, transaction);

                            if (isDeleted)
                            {
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Gallery Person deleted successfully.";
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
                            Message = "Gallery Person not found";
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
