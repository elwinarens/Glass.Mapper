using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public abstract class ObjectCache
    {
        public abstract object GetObject(ObjectConstructionArgs args);
        public abstract bool ContansObject(ObjectConstructionArgs args);
    }
}
