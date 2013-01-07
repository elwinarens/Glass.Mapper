using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public abstract class AbstractCacheKeyResolver<TIdType>
    {
        public abstract CacheKey<TIdType> GetKey(ObjectConstructionArgs args);
    }
}
