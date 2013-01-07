using System;

namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public class CannotGenerateKeyException: Exception
    {
        public CannotGenerateKeyException(string message)
            : base(message)
        {
        }
    }
}
