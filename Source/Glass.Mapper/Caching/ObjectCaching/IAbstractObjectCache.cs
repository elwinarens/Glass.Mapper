using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public interface IAbstractObjectCache
    {
        object GetObject(ObjectConstructionArgs args);
        bool ContansObject(ObjectConstructionArgs args);
        bool AddObject(ObjectConstructionArgs args);
        bool ClearCache();
    }
}