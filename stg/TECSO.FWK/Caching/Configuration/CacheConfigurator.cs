using System;

namespace TECSO.FWK.Caching.Configuration
{
    internal class CacheConfigurator : ICacheConfigurator
    {
        public string CacheName { get; private set; }

        public Action<ICache> InitAction { get; private set; }

        public CacheConfigurator(Action<ICache> initAction)
        {
            InitAction = initAction;
        }

        public CacheConfigurator(string cacheName, Action<ICache> initAction)
        {
            CacheName = cacheName;
            InitAction = initAction;
        }
    }
}