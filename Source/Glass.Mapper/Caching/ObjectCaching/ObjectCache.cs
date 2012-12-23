using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class ObjectCache
    {
        protected CacheKeyResolver _cacheKeyResolver;

        public abstract object GetObject(ObjectConstructionArgs args);
        public abstract bool ContansObject(ObjectConstructionArgs args);
        public abstract bool AddObject(ObjectConstructionArgs args);

        protected ObjectCache(CacheKeyResolver cacheKeyResolver)
        {
            _cacheKeyResolver = cacheKeyResolver;
        }
    }
}
