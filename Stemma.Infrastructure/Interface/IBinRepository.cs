using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface IBinRepository
    {
        Task<PagedResult<Bin>> Filter(string searchText, int pageNo, int pageSize);
        Task<long> Add(long entityId, string entity, string title, string deletedByIdentity);
    }
}
