using System.Runtime.Caching;
using NLog;
using System;

namespace bussedly.Models
{
    public class LocalCache
    {
        private ObjectCache cache;
        private Logger logger;

        public LocalCache()
        {
            this.logger = LogManager.GetCurrentClassLogger();
            this.cache = MemoryCache.Default;
        }

        public object Get(string key)
        {
            var cacheData = this.cache.Get(key);
            if (cacheData != null)
            {
                this.logger.Debug("cache hit for key {0}", key);
                return cacheData;
            }
            this.logger.Debug("cache miss for key {0}", key);
            return null;
        }

        public void Set(string key, object data, TimeSpan expiry)
        {
            this.cache.Set(key, data, new DateTimeOffset(DateTime.UtcNow + expiry));
        }
    }
}