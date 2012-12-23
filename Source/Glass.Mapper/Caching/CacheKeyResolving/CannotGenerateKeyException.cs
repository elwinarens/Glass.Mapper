using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
