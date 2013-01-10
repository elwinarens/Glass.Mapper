using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Glass.Mapper.Caching.CacheKeyResolving;
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

        protected override object InternalGetObject(ObjectConstructionArgs args)
        {
            throw new NotImplementedException();
        }

        protected override bool InternalContansObject(ObjectConstructionArgs args)
        {
            return _memoryCache.Contains(CacheKeyResolver.GetKey(args).ToString());
        }

        protected override void InternalAddObject(ObjectConstructionArgs args)
        {
            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = new TimeSpan(0, 2, 0,0);

            _memoryCache.Set(CacheKeyResolver.GetKey(args).ToString(), args.Result, policy);

        }

        protected override bool InternalClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
