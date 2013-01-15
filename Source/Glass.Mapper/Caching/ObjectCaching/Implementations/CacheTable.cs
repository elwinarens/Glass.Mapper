using System;
using System.Collections;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching.Exceptions;

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

        protected override object InternalGetObject(string objectKey)
        {
            return _table[objectKey];
        }

        protected override bool InternalContansObject(string objectKey)
        {
            return _table.ContainsKey(objectKey);
        }

        protected override void InternalAddObject(string objectKey, object objectForCaching)
        {
            if (_table.ContainsKey(objectKey))
            {
                throw new DuplicatedKeyObjectCacheException("Key exists in testCache");
            }

            _table.Add(objectKey, objectForCaching);
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

        protected override bool InternalRemoveObject(string objectKey)
        {
            _table.Remove(objectKey);
            return !_table.ContainsKey(objectKey);
        }
    }
}
