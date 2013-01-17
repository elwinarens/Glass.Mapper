namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public interface ICacheKey
    {
        bool Equals(object other);
        object GetId();
    }
}