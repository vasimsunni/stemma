using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stemma.Core;
using Stemma.Infrastructure.DTOs;

namespace Stemma.Infrastructure.Interface
{
    public interface IPersonSpouseRepository
    {
        Task<IEnumerable<PersonSpouse>> Get(long personSpouseId);
        Task<PersonSpouse> GetByPerson(long personId);
        Task<long> Save(PersonSpouse personSpouse, IDatabaseTransaction transaction);
        Task<bool> Delete(long personSpouseId, string deletedByIdentityId, IDatabaseTransaction transaction);
    }
}
