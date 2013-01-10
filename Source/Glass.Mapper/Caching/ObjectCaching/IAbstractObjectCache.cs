using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public interface IAbstractObjectCache
    {
        string BaseCacheKey { get; set; }
        string DefaultBaseCacheKey { get; }
        object GetObject(ObjectConstructionArgs args);
        bool ContansObject(ObjectConstructionArgs args);
        void AddObject(ObjectConstructionArgs args);
        bool ClearCache();
    }
}