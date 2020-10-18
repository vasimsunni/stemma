using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;

namespace Stemma.Services.Interfaces
{
   public interface IFileUploadService
    {
        Task<IEnumerable<FileUpload>> Get(long fileId);
        Task<IEnumerable<FileUpload>> GetByModule(long masterId, string module);
        long Save(FileUpload fileUpload);
        bool Delete(long fileId);
        bool DeleteByModule(long masterId, string module);
    }
}
