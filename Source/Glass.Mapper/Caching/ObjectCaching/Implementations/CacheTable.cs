using System;
using System.Collections;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Caching.ObjectCaching.Implementations
{
    public class CacheTable<TIdType> : AbstractObjectCache<TIdType>
    {
        private volatile Hashtable _table = new Hashtable();

        public CacheTable():base()
        {
        }

        public CacheTable(AbstractCacheKeyResolver<TIdType> cacheKeyResolver)
            : base(cacheKeyResolver)
        {
        }

        public CacheTable(string baseCacheKey)
            : base(baseCacheKey)
        {
        }

        protected override object InternalGetObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table[base.CacheKeyResolver.GetKey(args)];
        }

        protected override bool InternalContansObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table.ContainsKey(base.CacheKeyResolver.GetKey(args));
        }

        protected override void InternalAddObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            _table.Add(base.CacheKeyResolver.GetKey(args), args.Result);
        }


        protected override bool InternalClearCache()
        {
            try
            {
                _table.Clear();
            }
            catch
            {
                return false;
            }
            return true;

        }
    }
}
