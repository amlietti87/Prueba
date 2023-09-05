using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TECSO.FWK.Caching.Configuration;

namespace TECSO.FWK.Caching
{
    /// <summary>
    /// Base class for cache managers.
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager
    {        
        protected readonly ICachingConfiguration Configuration;

        protected readonly ConcurrentDictionary<string, ICache> Caches;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iocManager"></param>
        /// <param name="configuration"></param>
        protected CacheManagerBase(ICachingConfiguration configuration)
        {            
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }
        
        public virtual ICache GetCache(string name)
        {            
            if (name == null)
            {
                throw new ArgumentNullException(name);
            }

            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);

                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }

                return cache;
            });
        }

        public virtual void Dispose()
        {
            DisposeCaches();
            Caches.Clear();
        }

        protected virtual void DisposeCaches()
        {
            foreach (var cache in Caches)
            {
                //IocManager.Release(cache.Value);
            }
        }

        /// <summary>
        /// Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}