using TECSO.FWK.Caching.Configuration;


namespace TECSO.FWK.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICacheManager"/> to work with MemoryCache.
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MemoryCacheManager(ICachingConfiguration configuration)
            : base(configuration)
        {

        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new MemoryCache(name)
            {
               
            };
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
