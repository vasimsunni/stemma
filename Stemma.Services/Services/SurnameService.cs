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
    public class SurnameService : ISurnameService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly ISurnameRepository surnameRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public SurnameService(IUnitOfWork unitOfWork,
                       UserManager<ApplicationUser> userManager,
                       IUserRepository userRepository,
                       ISurnameRepository surnameRepository,
                       IPersonRepository personRepository,
                       IMapper mapper,
                       IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.surnameRepository = surnameRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await surnameRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.Surname>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    foreach (var surname in filteredResponse.Records)
                    {
                        surname.NoOfPeople = await personRepository.TotalBySurname(surname.Id);

                        if (!string.IsNullOrEmpty(surname.CreatedBy)) surname.CreatedBy = await userRepository.GetFullName(surname.CreatedBy);
                        if (!string.IsNullOrEmpty(surname.UpdatedBy)) surname.UpdatedBy = await userRepository.GetFullName(surname.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Surname>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Surname>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.Surname>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.Surname> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await surnameRepository.Get(id);

                var personResponse = mapper.Map<IEnumerable<DTOs.Response.Surname>>(filteredResult);

                if (personResponse.Any())
                {
                    foreach (var surname in personResponse)
                    {
                        surname.NoOfPeople = await personRepository.TotalBySurname(surname.Id);

                        if (!string.IsNullOrEmpty(surname.CreatedBy)) surname.CreatedBy = await userRepository.GetFullName(surname.CreatedBy);
                        if (!string.IsNullOrEmpty(surname.UpdatedBy)) surname.UpdatedBy = await userRepository.GetFullName(surname.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Create(DTOs.Request.Surname model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Surname> response = new DTOs.Response.ResponseDTO<DTOs.Response.Surname>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Surname Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var person = mapper.Map<Stemma.Core.Surname>(model);

                    person.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await surnameRepository.Save(person, transaction);

                    if (model.Id > 0)
                    {
                        transaction.Commit();

                        StatusCode = 200;
                        isSuccess = true;
                        Message = "Surname added successfully.";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Update(DTOs.Request.Surname model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Surname> response = new DTOs.Response.ResponseDTO<DTOs.Response.Surname>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Surname Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var surnames = await surnameRepository.Get(model.Id);

                        if (surnames.Any())
                        {
                            var surname = surnames.LastOrDefault();

                            surname = mapper.Map<Stemma.Core.Surname>(model);

                            surname.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await surnameRepository.Save(surname, transaction);

                            if (model.Id > 0)
                            {
                                transaction.Commit();

                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Surname updated successfully.";
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
                            Message = "Surname not found";
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.Surname>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.Surname> response = new DTOs.Response.ResponseDTO<DTOs.Response.Surname>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.Surname Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var people = await surnameRepository.Get(id);

                        if (people.Any())
                        {
                            var person = people.LastOrDefault();


                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await surnameRepository.Delete(person.SurnameId, currentUser.Id, transaction);

                            if (isDeleted)
                            {
                                StatusCode = 200;
                                isSuccess = true;
                                Message = "Surname deleted successfully.";
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
                            Message = "Surname not found";
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
