using System;
using System.Collections;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Caching.ObjectCaching.Implementations
{
    public class CacheTable : ObjectCache
    {
        
        private static volatile Hashtable _table = new Hashtable();


        public CacheTable(CacheKeyResolver cacheKeyResolver) : base(cacheKeyResolver)
        {
        }

        public override object GetObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table[base._cacheKeyResolver.GetKey(args)];
        }

        public override bool ContansObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            return _table.ContainsKey(base._cacheKeyResolver.GetKey(args));
        }

        public override bool AddObject(Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            if (args.Result == null)
                return false;

            _table.Add(base._cacheKeyResolver.GetKey(args), args.Result);
            return true;
        }
    }
}
