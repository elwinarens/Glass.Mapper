using System;

namespace Glass.Mapper.Caching.ObjectCaching.Exceptions
{
    public class KeyNullOrEmptyObjectCacheException : ObjectCacheException
    {
        public KeyNullOrEmptyObjectCacheException(string message)
            : base(message)
        {
        }
    }
}
