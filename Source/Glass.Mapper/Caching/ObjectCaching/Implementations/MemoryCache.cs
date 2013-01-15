using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching.Exceptions;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching.Implementations
{
    public class MemoryCache<TIdType> : AbstractObjectCache<TIdType>
    {
        private volatile MemoryCache _memoryCache;

        public MemoryCache():base()
        {
            _memoryCache = new MemoryCache(DefaultBaseCacheKey);
        }

        public MemoryCache(AbstractCacheKeyResolver<TIdType> cacheKeyResolver)
            : base(cacheKeyResolver)
        {
            _memoryCache = new MemoryCache(DefaultBaseCacheKey);
        }

        public MemoryCache(string baseCacheKey)
            : base(baseCacheKey)
        {
            _memoryCache = new MemoryCache(baseCacheKey);
        }

        protected override object InternalGetObject(string objectKey)
        {
            return _memoryCache.Get(objectKey);
        }

        protected override bool InternalContansObject(string objectKey)
        {
            return _memoryCache.Contains(objectKey);
        }

        protected override void InternalAddObject(string objectKey, object objectForCaching)
        {
            if (_memoryCache.Contains(objectKey))
            {
                throw new DuplicatedKeyObjectCacheException("Key exists in testCache");
            }

            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = new TimeSpan(0, 2, 0,0);

            _memoryCache.Set(objectKey, objectForCaching, policy);

        }

        protected override bool InternalClearCache()
        {
            throw new NotImplementedException();
        }


        protected override bool InternalRemoveObject(string objectKey)
        {
            _memoryCache.Remove(objectKey);
            return !_memoryCache.Contains(objectKey);
        }
    }
}
