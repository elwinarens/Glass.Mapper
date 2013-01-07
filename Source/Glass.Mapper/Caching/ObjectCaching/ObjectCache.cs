using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class AbstractObjectCache<TIdType> : IAbstractObjectCache
    {
        protected AbstractCacheKeyResolver<TIdType> CacheKeyResolver;

        public abstract object GetObject(ObjectConstructionArgs args);
        public abstract bool ContansObject(ObjectConstructionArgs args);
        public abstract bool AddObject(ObjectConstructionArgs args);
        public abstract bool ClearCache();

        protected AbstractObjectCache()
        {
        }

        protected AbstractObjectCache(Context glassContext)
        {
            CacheKeyResolver = glassContext.DependencyResolver.Resolve<AbstractCacheKeyResolver<TIdType>>();
        }



        protected AbstractObjectCache(AbstractCacheKeyResolver<TIdType> cacheKeyResolver)
        {
            CacheKeyResolver = cacheKeyResolver;
        }
    }
}
