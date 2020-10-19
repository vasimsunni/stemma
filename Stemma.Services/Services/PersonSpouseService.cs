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
    public class PersonSpouseService : IPersonSpouseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IPersonSpouseRepository personSpouseRepository;
        private readonly ISpouseRelationRepository spouseRelationRepository;
        private readonly IPersonRepository personRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public PersonSpouseService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
                                   IUserRepository userRepository, IPersonSpouseRepository personSpouseRepository,
                                   ISpouseRelationRepository spouseRelationRepository,
                                   IPersonRepository personRepository, IGalleryRepository galleryRepository,
                                   IFileUploadRepository fileUploadRepository, IMapper mapper,
                                   IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.personSpouseRepository = personSpouseRepository;
            this.spouseRelationRepository = spouseRelationRepository;
            this.personRepository = personRepository;
            this.galleryRepository = galleryRepository;
            this.fileUploadRepository = fileUploadRepository;
            this.mapper = mapper;
            this.configuration = configuration;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.PersonSpouse> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await personSpouseRepository.Get(id);

                var personSpouseResponse = mapper.Map<IEnumerable<DTOs.Response.PersonSpouse>>(filteredResult);

                if (personSpouseResponse.Any())
                {
                    foreach (var personSpouse in personSpouseResponse)
                    {
                        if (!string.IsNullOrEmpty(personSpouse.CreatedBy)) personSpouse.CreatedBy = await userRepository.GetFullName(personSpouse.CreatedBy);
                        if (!string.IsNullOrEmpty(personSpouse.UpdatedBy)) personSpouse.UpdatedBy = await userRepository.GetFullName(personSpouse.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = personSpouseResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = personSpouseResponse;
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>> GetByPerson(long personId)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.PersonSpouse>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.PersonSpouse> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await personSpouseRepository.GetByPerson(personId);

                var personSpouseResponse = mapper.Map<IEnumerable<DTOs.Response.PersonSpouse>>(filteredResult);

                if (personSpouseResponse.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var personSpouse in personSpouseResponse)
                    {
                        if (personSpouse.SpouseRelationIDF > 0)
                        {
                            var spouseRelations = await spouseRelationRepository.Get(personSpouse.SpouseRelationIDF);

                            if (spouseRelations.Any())
                            {
                                personSpouse.SpouseRelation = spouseRelations.LastOrDefault().Relation;
                            }
                        }

                        if (personSpouse.PersonIdF == personId)
                        {
                            var spouseResult = await personRepository.Get(personSpouse.SpousePersonIDF);

                            if (spouseResult.Any())
                            {
                                personSpouse.Spouse = mapper.Map<DTOs.Response.Person>(spouseResult.LastOrDefault());
                            }
                        }
                        else if (personSpouse.SpousePersonIDF == personId)
                        {
                            var spouseResult = await personRepository.Get(personSpouse.SpousePersonIDF);

                            if (spouseResult.Any())
                            {
                                personSpouse.Spouse = mapper.Map<DTOs.Response.Person>(spouseResult.LastOrDefault());
                            }
                        }

                        if (personSpouse.Spouse != null)
                        {

                            PictureURL = string.Empty;

                            var spouseGallery = await galleryRepository.GetPersonProfilePicture(personSpouse.Spouse.Id);

                            if (spouseGallery != null)
                            {
                                var fileUploads = await fileUploadRepository.Get(spouseGallery.GalleryId);

                                if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                if (string.IsNullOrEmpty(PictureURL))
                                {
                                    string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                    PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                }
                                else PictureURL = BaseURL + PictureURL;
                            }

                            personSpouse.Spouse.ProfilePictureURL = PictureURL;
                        }

                        if (!string.IsNullOrEmpty(personSpouse.CreatedBy)) personSpouse.CreatedBy = await userRepository.GetFullName(personSpouse.CreatedBy);
                        if (!string.IsNullOrEmpty(personSpouse.UpdatedBy)) personSpouse.UpdatedBy = await userRepository.GetFullName(personSpouse.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = personSpouseResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = personSpouseResponse;
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Create(DTOs.Request.PersonSpouse model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse> response = new DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PersonSpouse Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var personSpouse = mapper.Map<Stemma.Core.PersonSpouse>(model);

                    personSpouse.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await personSpouseRepository.Save(personSpouse, transaction);

                    if (model.Id > 0)
                    {
                        transaction.Commit();

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Spouse Relation added successfully.";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Update(DTOs.Request.PersonSpouse model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse> response = new DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PersonSpouse Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var personSpouses = await personSpouseRepository.Get(model.Id);

                        if (personSpouses.Any())
                        {
                            var PersonSpouse = personSpouses.LastOrDefault();

                            PersonSpouse = mapper.Map<Stemma.Core.PersonSpouse>(model);

                            PersonSpouse.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await personSpouseRepository.Save(PersonSpouse, transaction);

                            if (model.Id > 0)
                            {
                                transaction.Commit();

                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Spouse Relation updated successfully.";
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
                            Message = "Spouse Relation not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse> response = new DTOs.Response.ResponseDTO<DTOs.Response.PersonSpouse>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PersonSpouse Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var personSpouses = await personSpouseRepository.Get(id);

                        if (personSpouses.Any())
                        {
                            var personSpouse = personSpouses.LastOrDefault();


                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await personSpouseRepository.Delete(personSpouse.PersonSpouseId, currentUser.Id, transaction);

                            if (isDeleted)
                            {
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Spouse Relation deleted successfully.";
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
                            Message = "Spouse Relation not found";
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
