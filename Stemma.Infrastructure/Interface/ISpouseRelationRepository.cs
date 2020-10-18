using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface ISurnameRepository
    {
        Task<PagedResult<Surname>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<Surname>> Get(long surnameId);
        Task<long> Save(Surname surname, IDatabaseTransaction transaction);
        Task<bool> Delete(long surnameId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
