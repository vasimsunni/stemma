using Stemma.Core;
using Stemma.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Interface
{
    public interface IPersonRepository
    {
        Task<PagedResult<Person>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<Person>> Get(long personId);
        Task<Person> GetByIdentityId(string identityId);
        Task<long> Save(Person person, IDatabaseTransaction transaction);
        Task<bool> Activate(long PersonId, bool isActive, IDatabaseTransaction transaction);
        Task<bool> Delete(long personId, string deletedByIdentityId, IDatabaseTransaction transaction);

        Task<long> TotalBySurname(long surnameId);
    }
}
