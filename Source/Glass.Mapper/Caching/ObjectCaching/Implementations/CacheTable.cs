using System;
using System.Collections;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Caching.ObjectCaching.Implementations
{
    public class CacheTable : AbstractObjectCache
    {
        private static volatile Hashtable _table = new Hashtable();

        public CacheTable(AbstractCacheKeyResolver cacheKeyResolver)
            : base(cacheKeyResolver)
        {
        }

        public override object GetObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table[base.CacheKeyResolver.GetKey(args)];
        }

        public override bool ContansObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table.ContainsKey(base.CacheKeyResolver.GetKey(args));
        }

        public override bool AddObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            if (args.Result == null)
                return false;

            _table.Add(base.CacheKeyResolver.GetKey(args), args.Result);
            return true;
        }


        public override bool ClearCache()
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
