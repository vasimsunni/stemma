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
    public class SpouseRelationService : ISpouseRelationService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly ISpouseRelationRepository spouseRelationRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public SpouseRelationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
                                     IUserRepository userRepository, ISpouseRelationRepository spouseRelationRepository,
                                     IMapper mapper, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.spouseRelationRepository = spouseRelationRepository;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>>>> Filter(string searchText, int pageNo, int pageSize)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>>> response = new DTOs.Response.ResponseDTO<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>>>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await spouseRelationRepository.Filter(searchText, pageNo, pageSize);

                var filteredResponse = mapper.Map<DTOs.Response.PaginatedResponse<IEnumerable<DTOs.Response.SpouseRelation>>>(filteredResult);

                if (filteredResponse.Records.Any())
                {
                    foreach (var spouseRelation in filteredResponse.Records)
                    {
                        if (!string.IsNullOrEmpty(spouseRelation.CreatedBy)) spouseRelation.CreatedBy = await userRepository.GetFullName(spouseRelation.CreatedBy);
                        if (!string.IsNullOrEmpty(spouseRelation.UpdatedBy)) spouseRelation.UpdatedBy = await userRepository.GetFullName(spouseRelation.UpdatedBy);
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

        public async Task<DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.SpouseRelation>>> Get(long id)
        {
            DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.SpouseRelation>> response = new DTOs.Response.ResponseDTO<IEnumerable<DTOs.Response.SpouseRelation>>();

            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<DTOs.Response.SpouseRelation> Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var filteredResult = await spouseRelationRepository.Get(id);

                var spouseRelationResponse = mapper.Map<IEnumerable<DTOs.Response.SpouseRelation>>(filteredResult);

                if (spouseRelationResponse.Any())
                {
                    foreach (var spouseRelation in spouseRelationResponse)
                    {
                        if (!string.IsNullOrEmpty(spouseRelation.CreatedBy)) spouseRelation.CreatedBy = await userRepository.GetFullName(spouseRelation.CreatedBy);
                        if (!string.IsNullOrEmpty(spouseRelation.UpdatedBy)) spouseRelation.UpdatedBy = await userRepository.GetFullName(spouseRelation.UpdatedBy);
                    }

                    isSuccess = true;
                    StatusCode = 200;
                    Data = spouseRelationResponse;
                    Message = "Success";
                }
                else
                {
                    StatusCode = 404;
                    Data = spouseRelationResponse;
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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Create(DTOs.Request.SpouseRelation model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation> response = new DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.SpouseRelation Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    var spouseRelation = mapper.Map<Stemma.Core.SpouseRelation>(model);

                    spouseRelation.CreatedBy = currentUser.Id;

                    IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                    model.Id = await spouseRelationRepository.Save(spouseRelation, transaction);

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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Update(DTOs.Request.SpouseRelation model)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation> response = new DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.SpouseRelation Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (model.Id > 0)
                    {
                        var spouseRelations = await spouseRelationRepository.Get(model.Id);

                        if (spouseRelations.Any())
                        {
                            var SpouseRelation = spouseRelations.LastOrDefault();

                            SpouseRelation = mapper.Map<Stemma.Core.SpouseRelation>(model);

                            SpouseRelation.CreatedBy = currentUser.Id;

                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            model.Id = await spouseRelationRepository.Save(SpouseRelation, transaction);

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

        public async Task<DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>> Delete(long id)
        {
            DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation> response = new DTOs.Response.ResponseDTO<DTOs.Response.SpouseRelation>();

            int StatusCode = 0;
            bool isSuccess = false;
            DTOs.Response.SpouseRelation Data = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                ApplicationUser currentUser = await userManager.GetUserAsync(httpContext.HttpContext.User);

                if (currentUser != null)
                {
                    if (id > 0)
                    {
                        var spouseRelations = await spouseRelationRepository.Get(id);

                        if (spouseRelations.Any())
                        {
                            var spouseRelation = spouseRelations.LastOrDefault();


                            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();

                            bool isDeleted = await spouseRelationRepository.Delete(spouseRelation.SpouseRelationId, currentUser.Id, transaction);

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
