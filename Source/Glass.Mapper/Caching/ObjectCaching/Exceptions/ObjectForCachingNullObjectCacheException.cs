using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Caching.ObjectCaching.Exceptions
{
    public class ObjectForCachingNullObjectCacheException:ObjectCacheException
    {
        public ObjectForCachingNullObjectCacheException(string message) : base(message)
        {
        }
    }
}
