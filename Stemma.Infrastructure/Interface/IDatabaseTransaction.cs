using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Infrastructure.Interface
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
