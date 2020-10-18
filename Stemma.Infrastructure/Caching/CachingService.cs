using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.Caching
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CachingService> logger;

        public CachingService(IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<CachingService> logger)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration) where T : class
        {
            if (typeof(T) == typeof(string))
            {
                throw new NotSupportedException("This method does not support 'string' type. Please use GetOrCreateStringAsync method instead.");
            }
            return await GetOrCreateAsync(new JsonConverter<T>(), key, factory, memoryCacheExpiration, distributedCacheExpiration);
        }

        public async Task<string> GetOrCreateStringAsync(string key, Func<Task<string>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null)
        {
            return await GetOrCreateAsync(new StringConverter(), key, factory, memoryCacheExpiration, distributedCacheExpiration);
        }

        public async Task<T> GetOrCreateAsync<T>(IConverter<T> converter, string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration)
        {
            var local = await memoryCache.GetOrCreateAsync(key, entry =>
            {
                TimeSpan calculatedDistributedCacheExpiration = distributedCacheExpiration ?? memoryCacheExpiration;

                entry.AbsoluteExpiration = DateTime.UtcNow.Add(memoryCacheExpiration);
                return GetFromDistributedCache(converter, key, factory, calculatedDistributedCacheExpiration);
            });

            return local;
        }

        public bool Remove(string key)
        {
            if (RemoveFromDistributedCache(key))
            {
                var cachedItem = memoryCache.Get(key);
                if (cachedItem != null)
                {
                    memoryCache.Remove(key);
                }

                return true;

            }
            return false;
        }

        private async Task<T> GetFromDistributedCache<T>(IConverter<T> converter, string generatedKey, Func<Task<T>> factory, TimeSpan calculatedDistributedCacheExpiration)
        {
            logger.LogDebug("Getting cached value from Distributed cache for key {Key}", generatedKey);
            try
            {
                var cachedItem = await distributedCache.GetStringAsync(generatedKey);
                if (cachedItem != null)
                {
                    logger.LogDebug("Read cached value from Distributed cache for key {Key}", generatedKey);
                    var value = converter.Deserialize(cachedItem);
                    return value;
                }
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Exception getting cached item from Distributed cache.");
            }

            var item = await factory.Invoke();
            if (item != null)
            {
                try
                {
                    var cacheEntryOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = calculatedDistributedCacheExpiration };
                    var serializedValue = converter.Serialize(item);
                    await distributedCache.SetStringAsync(generatedKey, serializedValue, cacheEntryOptions, CancellationToken.None);
                    logger.LogDebug("Stored in Distributed cache for key {Key}", generatedKey);
                }
                catch (Exception e)
                {
                    logger.LogWarning(e, "Exception storing cached item in Distributed cache.");
                }
            }

            return item;
        }

        private bool RemoveFromDistributedCache(string generatedKey)
        {
            logger.LogDebug("Removing cached value from Distributed cache for key {Key}", generatedKey);
            try
            {
                var cachedItem = distributedCache.GetString(generatedKey);
                if (cachedItem != null)
                {
                    distributedCache.Remove(generatedKey);
                }

                return true;
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Exception removing cached item from Distributed cache.");
            }

            return false;
        }
    }
}
