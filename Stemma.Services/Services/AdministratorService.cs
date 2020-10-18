using AutoMapper;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.ComponentModel.DataAnnotations;

namespace Stemma.Services.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAdministratorRepository administratorRepository;
        private readonly IUserRepository userRepository;
        private readonly IFileUploadRepository fileUploadRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdministratorService(IUnitOfWork unitOfWork, IAdministratorRepository administratorRepository,
            IUserRepository userRepository, IFileUploadRepository fileUploadRepository,
            UserManager<ApplicationUser> userManager, IMapper mapper, IHttpContextAccessor httpContext,
            IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.administratorRepository = administratorRepository;
            this.userRepository = userRepository;
            this.fileUploadRepository = fileUploadRepository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.httpContext = httpContext;
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await administratorRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Administrator>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var admin in filteredResponse.Records)
                    {
                        PictureURL = string.Empty;

                        var applicationUser = await userRepository.GetByEmail(admin.Email);

                        if (applicationUser != null)
                        {
                            var fileUploads = await fileUploadRepository.GetByModule(admin.Id, Helpers.FileUploadEnum.AdminProfilePicture.ToString());

                            if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                            if (string.IsNullOrEmpty(PictureURL))
                            {
                                string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                            }
                            else PictureURL = BaseURL + PictureURL;

                            admin.Username = applicationUser.UserName;
                            admin.Roles = (await userRepository.GetRole(applicationUser)).ToArray();
                            admin.ProfilePictureURL = PictureURL;

                            if (!string.IsNullOrEmpty(admin.CreatedBy)) admin.CreatedBy = await userRepository.GetFullName(admin.CreatedBy);
                            if (!string.IsNullOrEmpty(admin.UpdatedBy)) admin.UpdatedBy = await userRepository.GetFullName(admin.UpdatedBy);
                        }
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
            catch(Exception ex)
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Administrator>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Administrator>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Administrator>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Administrator> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await administratorRepository.Get(id);

                var filteredResponse = mapper.Map<IEnumerable<DTOs.Response.Administrator>>(result);

                if (filteredResponse.Any())
                {
                    string PictureURL = string.Empty;
                    string BaseURL = configuration["Utility:APIBaseURL"].ToString();
                    string uploadFolderName = configuration["UploadFolders:UploadFolder"];

                    foreach (var admin in filteredResponse)
                    {
                        PictureURL = string.Empty;

                        var applicationUser = await userRepository.GetByEmail(admin.Email);

                        if (applicationUser != null)
                        {
                            var fileUploads = await fileUploadRepository.GetByModule(admin.Id, Helpers.FileUploadEnum.AdminProfilePicture.ToString());

                            if (fileUploads.Any()) PictureURL = uploadFolderName + "/" + fileUploads.LastOrDefault().Name;

                            if (string.IsNullOrEmpty(PictureURL))
                            {
                                string defaultProfilePicture = configuration["UploadFolders:DefaultProfilePicture"];
                                PictureURL = BaseURL + uploadFolderName + "/" + defaultProfilePicture;
                            }
                            else PictureURL = BaseURL + PictureURL;

                            admin.Username = applicationUser.UserName;
                            admin.Roles = (await userRepository.GetRole(applicationUser)).ToArray();
                            admin.ProfilePictureURL = PictureURL;

                            if (!string.IsNullOrEmpty(admin.CreatedBy)) admin.CreatedBy = await userRepository.GetFullName(admin.CreatedBy);
                            if (!string.IsNullOrEmpty(admin.UpdatedBy)) admin.UpdatedBy = await userRepository.GetFullName(admin.UpdatedBy);
                        }
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Create(DTOs.Request.Administrator model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Administrator> response = new DTOs.Response.ResponseDTO<DTOs.Response.Administrator>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Administrator Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    NormalizedUserName = model.Username,
                    NormalizedEmail = model.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = model.ContactNumbers,
                    TwoFactorEnabled = true,
                    LockoutEnd = new DateTime(2099, 12, 31),
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = currentUser.Id
                };

                var result = await userRepository.Add(applicationUser, model.Password, model.Roles);

                if (result.Succeeded)
                {
                    var administrator = new Administrator()
                    {
                        IdentityUserIdf = applicationUser.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        ContactNumbers = model.ContactNumbers,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = currentUser.Id
                    };

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await administratorRepository.Save(administrator, transaction);

                    if (model.Id > 0)
                    {
                        if (model.UploadedFileId > 0)
                        {
                            var uploadedFile = (await fileUploadRepository.Get(model.UploadedFileId)).LastOrDefault();

                            if (uploadedFile != null)
                            {
                                var uploadedFileRequestDTO = new FileUpload
                                {
                                    Module = Helpers.FileUploadEnum.AdminProfilePicture.ToString(),
                                    MasterIdf = model.Id,
                                    Name = uploadedFile.Name,
                                    OriginalName = uploadedFile.OriginalName,
                                    Size = uploadedFile.Size,
                                    Type = uploadedFile.Type,
                                    OtherDetails = uploadedFile.OtherDetails
                                };

                                fileUploadRepository.Save(uploadedFile,transaction);

                                transaction.Commit();
                            }
                        }

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Admin added successfully.";
                    }
                    else
                    {
                        StatusCode = 500;
                        Message = "Failed while saving.";
                    }
                }
                else
                {
                    StatusCode = 400;
                    Message = string.Join(" ,", result.Errors.Select(x => x.Description).ToList());
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Update(DTOs.Request.Administrator model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Administrator> response = new DTOs.Response.ResponseDTO<DTOs.Response.Administrator>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Administrator Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (model.Id > 0)
                {
                    var admins = await administratorRepository.Get(model.Id);

                    if (admins.Any())
                    {
                        var administrator = admins.LastOrDefault();
                        var applicationUser = await userRepository.GetByEmail(administrator.Email);

                        if (applicationUser != null)
                        {
                            applicationUser.Email = model.Email;
                            applicationUser.FirstName = model.FirstName;
                            applicationUser.LastName = model.LastName;
                            applicationUser.UserName = model.Username;
                            applicationUser.IsActive = true;
                            applicationUser.IsDeleted = false;
                            applicationUser.LockoutEnd = new DateTime(2099, 12, 31);
                            applicationUser.UpdatedBy = currentUser.Id;
                            if (!string.IsNullOrEmpty(model.Password))
                            {
                                applicationUser.PasswordHash = userManager.PasswordHasher.HashPassword(applicationUser, model.Password);
                            }

                            var result = await userRepository.Update(applicationUser, model.Roles);

                            if (result.Succeeded)
                            {
                                administrator.FirstName = model.FirstName;
                                administrator.LastName = model.LastName;
                                administrator.Email = model.Email;
                                administrator.ContactNumbers = model.ContactNumbers;
                                administrator.UpdatedBy = currentUser.Id;

                                IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                                model.Id = await administratorRepository.Save(administrator,transaction);

                                if (model.UploadedFileId > 0)
                                {
                                    var uploadedFile = (await fileUploadRepository.Get(model.UploadedFileId)).LastOrDefault();

                                    if (uploadedFile != null)
                                    {
                                        var uploadedFileRequestDTO = new FileUpload
                                        {
                                            Module = Helpers.FileUploadEnum.AdminProfilePicture.ToString(),
                                            MasterIdf = model.Id,
                                            Name = uploadedFile.Name,
                                            OriginalName = uploadedFile.OriginalName,
                                            Size = uploadedFile.Size,
                                            Type = uploadedFile.Type,
                                            OtherDetails = uploadedFile.OtherDetails
                                        };

                                        long savedFile = fileUploadRepository.Save(uploadedFile,transaction);
                                        if (savedFile > 0)
                                        {
                                            string uploadFolderName = configuration["UploadFolders:UploadFolder"];
                                            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
                                            {
                                                hostingEnvironment.WebRootPath = Path.Combine(string.Empty, "wwwroot");
                                            }

                                            //find the previous uploaded profile picture
                                            var recentUploads = await fileUploadRepository.GetByModule(model.Id, Helpers.FileUploadEnum.AdminProfilePicture.ToString());

                                            if (recentUploads.Any())
                                            {
                                                //delete previously uploaded profile picture
                                                foreach (var recentUpload in recentUploads)
                                                {
                                                    string physicalStoredFile = Path.Combine(hostingEnvironment.WebRootPath + "\\" + uploadFolderName + "\\", recentUpload.Name);
                                                    if (System.IO.File.Exists(physicalStoredFile))
                                                    {
                                                        System.IO.File.Delete(physicalStoredFile);
                                                    }
                                                }

                                                fileUploadRepository.DeleteByModule(model.Id, Helpers.FileUploadEnum.AdminProfilePicture.ToString(), transaction);
                                            }
                                        }
                                    }

                                    transaction.Commit();
                                }

                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Admin updated successfully.";

                            }
                            else
                            {
                                StatusCode = 400;
                                Message = string.Join(" ,", result.Errors.Select(x => x.Description).ToList());
                            }
                        }
                        else
                        {
                            StatusCode = 404;
                            Message = "Admin not found";
                        }
                    }
                    else
                    {
                        StatusCode = 404;
                        Message = "Admin not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Activate(long id, bool activate)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Administrator> response = new DTOs.Response.ResponseDTO<DTOs.Response.Administrator>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Administrator Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (id > 0)
                {
                    var admins = await administratorRepository.Get(id);

                    if (admins.Any())
                    {
                        var administrator = admins.LastOrDefault();
                        var applicationUser = await userRepository.GetByEmail(administrator.Email);

                        if (applicationUser != null)
                        {
                            applicationUser.IsActive = activate;
                            applicationUser.UpdatedBy = currentUser.Id;

                            var oldRoles = await userManager.GetRolesAsync(applicationUser);

                            var result = await userRepository.Update(applicationUser, oldRoles.ToArray());

                            if (result.Succeeded)
                            {
                                IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                                bool updated = await administratorRepository.Activate(id, activate,transaction);
                                
                                if (updated)
                                {
                                    transaction.Commit();

                                    StatusCode = 200;
                                    isSuccess = true;
                                    Message = "Admin updated successfully.";
                                }
                                else
                                {
                                    StatusCode = 500;
                                    Message = "Failed";
                                }
                            }
                            else
                            {
                                StatusCode = 400;
                                Message = string.Join(" ,", result.Errors.Select(x => x.Description).ToList());
                            }
                        }
                        else
                        {
                            StatusCode = 404;
                            Message = "Admin not found";
                        }
                    }
                    else
                    {
                        StatusCode = 404;
                        Message = "Admin not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Administrator>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Administrator> response = new DTOs.Response.ResponseDTO<DTOs.Response.Administrator>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Administrator Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (id > 0)
                {
                    var admins = await administratorRepository.Get(id);

                    if (admins.Any())
                    {
                        var administrator = admins.LastOrDefault();
                        var applicationUser = await userRepository.GetByEmail(administrator.Email);

                        if (applicationUser != null)
                        {  
                            var oldRoles = await userManager.GetRolesAsync(applicationUser);

                            applicationUser.IsDeleted = true;
                            applicationUser.UpdatedBy = currentUser.Id;


                            var result = await userRepository.Update(applicationUser, oldRoles.ToArray());

                            if (result.Succeeded)
                            {
                                IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                                bool updated = await administratorRepository.Delete(id, currentUser.Id, transaction);

                                if (updated)
                                {
                                    transaction.Commit();

                                    StatusCode = 200;
                                    isSuccess = true;
                                    Message = "Admin delete successfully.";
                                }
                                else
                                {
                                    StatusCode = 500;
                                    Message = "Failed";
                                }
                            }
                            else
                            {
                                StatusCode = 400;
                                Message = string.Join(" ,", result.Errors.Select(x => x.Description).ToList());
                            }
                        }
                        else
                        {
                            StatusCode = 404;
                            Message = "Admin not found";
                        }
                    }
                    else
                    {
                        StatusCode = 404;
                        Message = "Admin not found";
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
