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
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IPersonRepository personRepository;
        private readonly ISurnameRepository surnameRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly IGalleryPersonRepository galleryPersonRepository;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public PersonService(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IUserRepository userRepository,
                             IPersonRepository personRepository,
                             ISurnameRepository surnameRepository,
                             IGalleryRepository galleryRepository,
                             IGalleryPersonRepository galleryPersonRepository,
                             IFileUploadRepository fileUploadRepository,
                             IMapper mapper,
                             IConfiguration configuration,
                             IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.personRepository = personRepository;
            this.surnameRepository = surnameRepository;
            this.galleryRepository = galleryRepository;
            this.galleryPersonRepository = galleryPersonRepository;
            this.fileUploadRepository = fileUploadRepository;
            this.mapper = mapper;
            this.configuration = configuration;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await personRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Person>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var person in filteredResponse.Records)
                    {
                        PictureURL = string.Empty;

                        var gallery = await galleryRepository.GetPersonProfilePicture(person.Id);

                        if (gallery != null)
                        {
                            var fileUploads = await fileUploadRepository.Get(gallery.GalleryId);

                            if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                            if (string.IsNullOrEmpty(PictureURL))
                            {
                                string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                            }
                            else PictureURL = BaseURL + PictureURL;
                        }

                        if (person.FatherPersonIDF > 0)
                        {
                            var fatherResult = await personRepository.Get(person.FatherPersonIDF);

                            if (fatherResult.Any())
                            {
                                person.Father = mapper.Map<DTOs.Response.Person>(fatherResult.LastOrDefault());

                                PictureURL = string.Empty;

                                var fatherGallery = await galleryRepository.GetPersonProfilePicture(person.Id);

                                if (fatherGallery != null)
                                {
                                    var fileUploads = await fileUploadRepository.Get(fatherGallery.GalleryId);

                                    if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                    if (string.IsNullOrEmpty(PictureURL))
                                    {
                                        string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                        PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                    }
                                    else PictureURL = BaseURL + PictureURL;
                                }

                                person.Father.ProfilePictureURL = PictureURL;
                            }
                        }

                        if (person.MotherPersonIDF > 0)
                        {
                            var motherResult = await personRepository.Get(person.MotherPersonIDF);

                            if (motherResult.Any())
                            {
                                person.Mother = mapper.Map<DTOs.Response.Person>(motherResult.LastOrDefault());

                                PictureURL = string.Empty;

                                var motherGallery = await galleryRepository.GetPersonProfilePicture(person.Id);

                                if (motherGallery != null)
                                {
                                    var fileUploads = await fileUploadRepository.Get(motherGallery.GalleryId);

                                    if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                    if (string.IsNullOrEmpty(PictureURL))
                                    {
                                        string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                        PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                    }
                                    else PictureURL = BaseURL + PictureURL;
                                }

                                person.Mother.ProfilePictureURL = PictureURL;
                            }
                        }

                        if (person.SurnameIDF > 0)
                        {
                            var surname = await surnameRepository.Get(person.SurnameIDF);

                            if (surname != null)
                            {
                                person.Surname = surname.LastOrDefault().Title;
                            }
                        }

                        if (person.DOD != null && person.DOD.Value > person.DOB.Value)
                        {
                            person.Age = person.DOD.Value.Year - person.DOB.Value.Year;
                        }
                        else
                        {
                            person.Age = DateTime.Today.Year - person.DOB.Value.Year;
                        }

                        if (!string.IsNullOrEmpty(person.CreatedBy)) person.CreatedBy = await userRepository.GetFullName(person.CreatedBy);
                        if (!string.IsNullOrEmpty(person.UpdatedBy)) person.UpdatedBy = await userRepository.GetFullName(person.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Person>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Person>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Person>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Person> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await personRepository.Get(id);

                var personResponse = mapper.Map<IEnumerable<DTOs.Response.Person>>(filteredResult);

                if (personResponse.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var person in personResponse)
                    {
                        PictureURL = string.Empty;

                        var gallery = await galleryRepository.GetPersonProfilePicture(person.Id);

                        if (gallery != null)
                        {
                            var fileUploads = await fileUploadRepository.Get(gallery.GalleryId);

                            if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                            if (string.IsNullOrEmpty(PictureURL))
                            {
                                string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                            }
                            else PictureURL = BaseURL + PictureURL;
                        }

                        if (person.FatherPersonIDF > 0)
                        {
                            var fatherResult = await personRepository.Get(person.FatherPersonIDF);

                            if (fatherResult.Any())
                            {
                                person.Father = mapper.Map<DTOs.Response.Person>(fatherResult.LastOrDefault());

                                PictureURL = string.Empty;

                                var fatherGallery = await galleryRepository.GetPersonProfilePicture(person.Father.Id);

                                if (fatherGallery != null)
                                {
                                    var fileUploads = await fileUploadRepository.Get(fatherGallery.GalleryId);

                                    if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                    if (string.IsNullOrEmpty(PictureURL))
                                    {
                                        string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                        PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                    }
                                    else PictureURL = BaseURL + PictureURL;
                                }

                                person.Father.ProfilePictureURL = PictureURL;
                            }
                        }

                        if (person.MotherPersonIDF > 0)
                        {
                            var motherResult = await personRepository.Get(person.MotherPersonIDF);

                            if (motherResult.Any())
                            {
                                person.Mother = mapper.Map<DTOs.Response.Person>(motherResult.LastOrDefault());

                                PictureURL = string.Empty;

                                var motherGallery = await galleryRepository.GetPersonProfilePicture(person.Mother.Id);

                                if (motherGallery != null)
                                {
                                    var fileUploads = await fileUploadRepository.Get(motherGallery.GalleryId);

                                    if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                                    if (string.IsNullOrEmpty(PictureURL))
                                    {
                                        string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                        PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                                    }
                                    else PictureURL = BaseURL + PictureURL;
                                }

                                person.Mother.ProfilePictureURL = PictureURL;
                            }
                        }


                        if (person.SurnameIDF > 0)
                        {
                            var surname = await surnameRepository.Get(person.SurnameIDF);

                            if (surname != null)
                            {
                                person.Surname = surname.LastOrDefault().Title;
                            }
                        }

                        if (person.DOD != null && person.DOD.Value > person.DOB.Value)
                        {
                            person.Age = person.DOD.Value.Year - person.DOB.Value.Year;
                        }
                        else
                        {
                            person.Age = DateTime.Today.Year - person.DOB.Value.Year;
                        }

                        if (!string.IsNullOrEmpty(person.CreatedBy)) person.CreatedBy = await userRepository.GetFullName(person.CreatedBy);
                        if (!string.IsNullOrEmpty(person.UpdatedBy)) person.UpdatedBy = await userRepository.GetFullName(person.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Create(DTOs.Request.Person model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Person> response = new DTOs.Response.ResponseDTO<DTOs.Response.Person>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Person Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var person = mapper.Map<Person>(model);

                    person.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await personRepository.Save(person, transaction);

                    if (model.Id > 0)
                    {
                        if (model.PictureUploadId > 0)
                        {
                            var uploadedFile = (await fileUploadRepository.Get(model.PictureUploadId)).LastOrDefault();

                            if (uploadedFile != null)
                            {
                                var uploadedFileRequestDTO = new FileUpload
                                {
                                    Module = Helpers.FileUploadEnum.PersonProfilePicture.ToString(),
                                    MasterIdf = model.Id,
                                    Name = uploadedFile.Name,
                                    OriginalName = uploadedFile.OriginalName,
                                    Size = uploadedFile.Size,
                                    Type = uploadedFile.Type,
                                    OtherDetails = uploadedFile.OtherDetails
                                };

                                var savedFileId = fileUploadRepository.Save(uploadedFile, transaction);

                                if (savedFileId > 0)
                                {
                                    var gallery = new Gallery()
                                    {
                                        FileUploadId = savedFileId,
                                        ItemName = uploadedFileRequestDTO.OriginalName,
                                        ItemType = uploadedFileRequestDTO.Type,
                                        ItemDate = DateTime.Now,
                                        CreatedBy = currentUser.Id,
                                        CreatedOn = DateTime.Now
                                    };

                                    var savedGalleryId = await galleryRepository.Save(gallery, transaction);

                                    if (savedGalleryId > 0)
                                    {
                                        var galleryPerson = new GalleryPerson()
                                        {
                                            GalleryIDF = savedGalleryId,
                                            PersonIDF = model.Id,
                                            IsProfilePicture = true,
                                            CreatedBy = gallery.CreatedBy,
                                            CreatedOn = gallery.CreatedOn
                                        };

                                        var savedGalleryPersonId = await galleryPersonRepository.Save(galleryPerson, transaction);
                                    }
                                }

                                transaction.Commit();
                            }
                        }

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Person added successfully.";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Update(DTOs.Request.Person model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Person> response = new DTOs.Response.ResponseDTO<DTOs.Response.Person>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Person Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (model.Id > 0)
                {
                    var people = await personRepository.Get(model.Id);

                    if (people.Any())
                    {
                        var person = people.LastOrDefault();

                        person = mapper.Map<Person>(model);

                        person.CreatedBy = currentUser.Id;

                        IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                        model.Id = await personRepository.Save(person, transaction);

                        if (model.Id > 0)
                        {
                            if (model.PictureUploadId > 0)
                            {
                                var uploadedFile = (await fileUploadRepository.Get(model.PictureUploadId)).LastOrDefault();

                                if (uploadedFile != null)
                                {
                                    var uploadedFileRequestDTO = new FileUpload
                                    {
                                        Module = Helpers.FileUploadEnum.PersonProfilePicture.ToString(),
                                        MasterIdf = model.Id,
                                        Name = uploadedFile.Name,
                                        OriginalName = uploadedFile.OriginalName,
                                        Size = uploadedFile.Size,
                                        Type = uploadedFile.Type,
                                        OtherDetails = uploadedFile.OtherDetails
                                    };

                                    var savedFileId = fileUploadRepository.Save(uploadedFile, transaction);

                                    if (savedFileId > 0)
                                    {
                                        var gallery = new Gallery()
                                        {
                                            FileUploadId = savedFileId,
                                            ItemName = uploadedFileRequestDTO.OriginalName,
                                            ItemType = uploadedFileRequestDTO.Type,
                                            ItemDate = DateTime.Now,
                                            CreatedBy = currentUser.Id,
                                            CreatedOn = DateTime.Now
                                        };

                                        var savedGalleryId = await galleryRepository.Save(gallery, transaction);

                                        if (savedGalleryId > 0)
                                        {
                                            var galleryPerson = new GalleryPerson()
                                            {
                                                GalleryIDF = savedGalleryId,
                                                PersonIDF = model.Id,
                                                IsProfilePicture = true,
                                                CreatedBy = gallery.CreatedBy,
                                                CreatedOn = gallery.CreatedOn
                                            };

                                            var savedGalleryPersonId = await galleryPersonRepository.Save(galleryPerson, transaction);
                                        }
                                    }

                                    transaction.Commit();
                                }
                            }

                            StatusCode = 200;
                            isSuccess = true;
                            Message = "Person updated successfully.";
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
                        Message = "Person not found";
                    }
                }
                else
                {
                    StatusCode = 406;
                    Message = "Not Acceptable.";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Person>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Person> response = new DTOs.Response.ResponseDTO<DTOs.Response.Person>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Person Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (id > 0)
                {
                    var people = await personRepository.Get(id);

                    if (people.Any())
                    {
                        var person = people.LastOrDefault();


                        IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                        bool isDeleted = await personRepository.Delete(person.PersonId, currentUser.Id, transaction);

                        if (isDeleted)
                        {
                            StatusCode = 200;
                            isSuccess = true;
                            Message = "Person deleted successfully.";
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
                        Message = "Person not found";
                    }
                }
                else
                {
                    StatusCode = 406;
                    Message = "Not Acceptable.";
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
