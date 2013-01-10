using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class AbstractObjectCache<TIdType> : IAbstractObjectCache
    {
        public string BaseCacheKey { get; set; }

        public string DefaultBaseCacheKey
        {
            get { return "GlassObjectCahe"; }
        }

        protected AbstractCacheKeyResolver<TIdType> CacheKeyResolver;

        protected abstract object InternalGetObject(ObjectConstructionArgs args);
        protected abstract bool InternalContansObject(ObjectConstructionArgs args);
        protected abstract void InternalAddObject(ObjectConstructionArgs args);
        protected abstract bool InternalClearCache();

        public object GetObject(ObjectConstructionArgs args)
        {
            return InternalGetObject(args);
        }

        public bool ContansObject(ObjectConstructionArgs args)
        {
            return InternalContansObject(args);
        }

        public void AddObject(ObjectConstructionArgs args)
        {
            InternalAddObject(args);
        }

        public bool ClearCache()
        {
            return InternalClearCache();
        }

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
    }
}
