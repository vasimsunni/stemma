using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface IAdministratorRepository
    {
        Task<PagedResult<Administrator>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<Administrator>> Get(long administratorId);
        Task<Administrator> GetByIdentityId(string identityId);
        Task<long> Save(Administrator administrator, IDatabaseTransaction transaction);
        Task<bool> Activate(long administratorId, bool isActive, IDatabaseTransaction transaction);
        Task<bool> Delete(long administratorId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
