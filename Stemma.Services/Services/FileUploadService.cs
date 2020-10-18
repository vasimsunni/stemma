using System.Collections.Generic;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Interface;
using Stemma.Services.Interfaces;

namespace Stemma.Services.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileUploadRepository fileUploadRepository;

        public FileUploadService(IUnitOfWork unitOfWork, IFileUploadRepository fileUploadRepository)
        {
            this.unitOfWork = unitOfWork;
            this.fileUploadRepository = fileUploadRepository;
        }

        public async Task<IEnumerable<FileUpload>> Get(long fileId) => await fileUploadRepository.Get(fileId);
        public async Task<IEnumerable<FileUpload>> GetByModule(long masterId, string module) => await fileUploadRepository.GetByModule(masterId, module);
        public long Save(FileUpload fileUpload)
        {
            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();
            var savedId = fileUploadRepository.Save(fileUpload, transaction);
            transaction.Commit();

            return savedId;
        }
        public bool Delete(long fileId)
        {
            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();
            var deleted = fileUploadRepository.Delete(fileId, transaction);
            transaction.Commit();

            return deleted;
        }
        public bool DeleteByModule(long masterId, string module)
        {
            IDatabaseTransaction transaction = unitOfWork.BeginTransaction();
            var deleted = fileUploadRepository.DeleteByModule(masterId, module, transaction);
            transaction.Commit();
            return deleted;
        }
    }
}
