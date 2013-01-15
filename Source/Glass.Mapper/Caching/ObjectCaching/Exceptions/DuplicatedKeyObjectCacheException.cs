using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Caching.ObjectCaching.Exceptions
{
    public class DuplicatedKeyObjectCacheException:ObjectCacheException
    {
        public DuplicatedKeyObjectCacheException(string message) : base(message)
        {
        }
    }
}
