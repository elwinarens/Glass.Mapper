using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public interface ICacheKeyResolver<T>
    {
        CacheKey<Guid> GetKey(ObjectConstructionArgs args);
    }
}
