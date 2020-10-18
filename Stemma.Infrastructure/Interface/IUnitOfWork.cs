using System;

namespace Stemma.Infrastructure.Interface
{
    public interface IUnitOfWork:IDisposable
    {
        IDatabaseTransaction BeginTransaction();
        ApplicationDbContext Context { get;}
        void Commit();
    }
}
