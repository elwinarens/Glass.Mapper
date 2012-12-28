using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public abstract class AbstractCacheKeyResolver
    {
        public abstract CacheKey GetKey(ObjectConstructionArgs args);

        protected AbstractCacheKeyResolver()
        {
        }
    }
}
