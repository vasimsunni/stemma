using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Caching
{
    public interface ICachingService
    {

        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null) where T : class;
        Task<string> GetOrCreateStringAsync(string key, Func<Task<string>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null);
        Task<T> GetOrCreateAsync<T>(IConverter<T> converter, string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration);
        bool Remove(string key);
    }
}
