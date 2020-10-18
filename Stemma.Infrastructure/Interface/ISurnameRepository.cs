using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface ISpouseRelationRepository
    {
        Task<PagedResult<SpouseRelation>> Filter(string searchText, int pageNo, int pageSize);
        Task<IEnumerable<SpouseRelation>> Get(long spouseRelationId);
        Task<long> Save(SpouseRelation spouseRelation, IDatabaseTransaction transaction);
        Task<bool> Delete(long spouseRelationId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
