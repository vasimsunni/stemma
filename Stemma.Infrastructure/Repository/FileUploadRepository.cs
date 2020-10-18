using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.Caching;
using Stemma.Infrastructure.Interface;

namespace Stemma.Infrastructure.Repository
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachingService cachingService;

        public FileUploadRepository(IUnitOfWork unitOfWork, ICachingService cachingService)
        {
            this.unitOfWork = unitOfWork;
            this.cachingService = cachingService;
        }

        public async Task<IEnumerable<FileUpload>> Get(long fileId)
        {
            var result = await GetCachedList();
            return result.Where(x => x.FileId == (fileId > 0 ? fileId : x.FileId)).ToList();
        }
        public async Task<IEnumerable<FileUpload>> GetByModule(long masterId, string module)
        {
            return await unitOfWork.Context.FileUploads.Where(x => x.MasterIdf == masterId && x.Module == module).ToListAsync();
        }
        public long Save(FileUpload fileUpload, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    if (fileUpload.FileId > 0)
                    {
                        fileUpload.CreatedOn = DateTime.Now;
                        unitOfWork.Context.Set<FileUpload>().Add(fileUpload);
                    }
                    else
                    {
                        fileUpload.UpdatedOn = DateTime.Now;
                        unitOfWork.Context.Entry(fileUpload).State = EntityState.Modified;
                        unitOfWork.Context.FileUploads.Update(fileUpload);
                    }

                    if (fileUpload.FileId > 0) RemoveCache();

                    unitOfWork.Commit();
                    return fileUpload.FileId;
                }
                catch (Exception Ex)
                {
                    transaction.Rollback();
                }
            }

            return 0;
        }
        public bool Delete(long fileId, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    FileUpload fileUpload = unitOfWork.Context.FileUploads.Where(x => x.FileId == fileId).LastOrDefault();

                    if (fileUpload != null)
                    {
                        unitOfWork.Context.FileUploads.Remove(fileUpload);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return false;
        }
        public bool DeleteByModule(long masterId, string module, IDatabaseTransaction transaction)
        {
            using (transaction)
            {
                try
                {
                    var fileUploads = unitOfWork.Context.FileUploads.Where(x => x.MasterIdf == masterId && x.Module == module).ToList();

                    if (fileUploads.Any())
                    {
                        unitOfWork.Context.FileUploads.RemoveRange(fileUploads);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return false;
        }

        private async Task<IEnumerable<FileUpload>> GetCachedList()
        {
            return await cachingService.GetOrCreateAsync(
                "FileUploads",
                () => unitOfWork.Context.FileUploads.ToListAsync(),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
                );
        }
        private bool RemoveCache()
        {
            return cachingService.Remove("FileUploads");
        }
    }
}
