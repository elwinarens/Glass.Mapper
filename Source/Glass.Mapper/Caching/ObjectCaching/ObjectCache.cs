using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class AbstractObjectCache
    {
        protected AbstractCacheKeyResolver CacheKeyResolver;

        public abstract object GetObject(ObjectConstructionArgs args);
        public abstract bool ContansObject(ObjectConstructionArgs args);
        public abstract bool AddObject(ObjectConstructionArgs args);
        public abstract bool ClearCache();

        protected AbstractObjectCache()
        {
        }

        protected AbstractObjectCache(Context glassContext)
        {
            CacheKeyResolver = glassContext.DependencyResolver.Resolve<AbstractCacheKeyResolver>();
        }

       

        protected AbstractObjectCache(AbstractCacheKeyResolver cacheKeyResolver)
        {
            CacheKeyResolver = cacheKeyResolver;
        }
    }
}
