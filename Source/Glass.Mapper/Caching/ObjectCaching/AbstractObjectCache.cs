using System;
using System.Collections.Generic;
using System.Threading;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching.Exceptions;
using Glass.Mapper.Caching.Proxy;
using Glass.Mapper.Pipelines.ObjectConstruction;
using System.Linq;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class AbstractObjectCache<TIdType> : IAbstractObjectCache
    {
        private readonly CacheInformation<TIdType> _cacheInformation = new CacheInformation<TIdType>();
        public string BaseCacheKey { get; set; }
        
        
        public string DefaultBaseCacheKey
        {
            get { return "GlassObjectCahe"; }
        }

        protected AbstractCacheKeyResolver<TIdType> CacheKeyResolver;

        protected abstract object InternalGetObject(string objectKey);
        protected abstract bool InternalRemoveObject(string objectKey);
        protected abstract bool InternalContansObject(string objectKey);
        protected abstract void InternalAddObject(string objectKey, object objectForCaching);
        protected abstract bool InternalClearCache();

        protected AbstractObjectCache(string baseCacheKey)
        {
            BaseCacheKey = baseCacheKey;
        }

        protected AbstractObjectCache()
        {
            BaseCacheKey = DefaultBaseCacheKey;
        }

        protected AbstractObjectCache(Context glassContext)
            : this()
        {
            CacheKeyResolver = glassContext.DependencyResolver.Resolve<AbstractCacheKeyResolver<TIdType>>();
        }

        protected AbstractObjectCache(AbstractCacheKeyResolver<TIdType> cacheKeyResolver)
            : this()
        {
            CacheKeyResolver = cacheKeyResolver;
        }

        public object GetObject(ObjectConstructionArgs args)
        {
            return InternalGetObject(CacheKeyResolver.GetKey(args).ToString());
        }

        public bool ContansObject(ObjectConstructionArgs args)
        {
            return InternalContansObject(CacheKeyResolver.GetKey(args).ToString());
        }
            
        public void AddObject(ObjectConstructionArgs args)
        {
            InternalAddObject(CacheKeyResolver.GetKey(args).ToString(), args.Result);
        }

        public object GetObject(ICacheKey cacheKey)
        {
            return InternalGetObject(cacheKey.ToString());
        }

        public bool ContansObject(ICacheKey cacheKey)
        {
            return InternalContansObject(cacheKey.ToString());
        }

        public void AddObject(ICacheKey cacheKey, object objectForCaching)
        {
            InternalAddObject(cacheKey.ToString(), objectForCaching);
        }

        public bool ClearCache()
        {
            return InternalClearCache();
        }

        public bool ClearRelatedCache(string releatedKey)
        {
            CheckRelatedKey(releatedKey);

            if (!_cacheInformation.ContainsRelatedKey(releatedKey))
                return false;

            var returnBool = false;

            foreach (var key in (List<string>)_cacheInformation.GetObjectKeys(releatedKey))
            {
                returnBool = InternalRemoveObject(key);
            }


            return returnBool;
        }

        public void AddToRelatedCache(string objectKey, IEnumerable<string> releatedKeys, object objectForCaching)
        {
            CheckKey(objectKey);
            var releatedKeysList = releatedKeys as IList<string> ?? releatedKeys.ToList();

            CheckRelatedKey(releatedKeysList);
            CheckobjectForCaching(objectForCaching);

            _cacheInformation.AddObjectKey(releatedKeysList, objectKey);
            InternalAddObject(objectKey, objectForCaching);
        }

        public void AddToRelatedCache(string objectKey, string releatedKey, object objectForCaching)
        {
            CheckKey(objectKey);
            CheckRelatedKey(releatedKey);
            CheckobjectForCaching(objectForCaching);

            _cacheInformation.AddObjectKey(releatedKey, objectKey);
            InternalAddObject(objectKey, objectForCaching);
        }

        public object GetFromRelatedCache<T>(string objectKey)
        {
            return InternalGetObject(objectKey);
        }

        public bool RelatedCacheContansObject(string objectKey)
        {
            return InternalContansObject(objectKey);
        }

        public virtual IDictionary<string, List<string>> GetRelatedKeys()
        {
            var returnDictionary = new Dictionary<string, List<string>>();

            foreach (string item in _cacheInformation.GetRelatedKeys())
            {
                returnDictionary.Add(item, (List<string>) _cacheInformation.GetObjectKeys(item));
            }


            return returnDictionary;
        }

        #region Private Helpers

        private static void CheckKey(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new KeyNullOrEmptyObjectCacheException(String.Format("Key was {0}", key == null ? "NULL" : "Empty"));
            }
        }

        private static void CheckRelatedKey(string releatedKey)
        {
            if (String.IsNullOrEmpty(releatedKey))
            {
                throw new RelatedTemplateIdNullOrEmptyObjectCacheException(String.Format("releatedKey was {0}", releatedKey == null ? "NULL" : "Empty"));
            }
        }

        private static void CheckRelatedKey(IEnumerable<string> releatedKeys)
        {
            if (releatedKeys == null)
            {
                throw new RelatedTemplateIdNullOrEmptyObjectCacheException("releatedKeys list was NULL");
            }


            if (!releatedKeys.Any())
            {
                throw new RelatedTemplateIdNullOrEmptyObjectCacheException("releatedKeys list was empty");
            }
        }

        private static void CheckobjectForCaching(object objectForCaching)
        {
            if (objectForCaching == null)
            {
                throw new ObjectForCachingNullObjectCacheException("ObjectForCaching is null");
            }
        }
        #endregion


        public bool RemoveFromRelatedCache(string releatedKey)
        {
            CheckKey(releatedKey);
            _cacheInformation.RemoveObjectKey(releatedKey);

            return InternalRemoveObject(releatedKey);
        }


        public ICacheKey GetLatestCacheKey(object id)
        {
            return _cacheInformation.GetCacheKeys((TIdType)id).Peek();
        }
    }
}
