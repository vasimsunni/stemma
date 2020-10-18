using System.Collections.Generic;
using System.Threading.Tasks;
using Stemma.Core;

namespace Stemma.Infrastructure.Interface
{
    public interface IFileUploadRepository
    {
        Task<IEnumerable<FileUpload>> Get(long fileId);
        Task<IEnumerable<FileUpload>> GetByModule(long masterId, string module);
        long Save(FileUpload fileUpload,IDatabaseTransaction transaction);
        bool Delete(long fileId, IDatabaseTransaction transaction);
        bool DeleteByModule(long masterId, string module, IDatabaseTransaction transaction);
    }
}
