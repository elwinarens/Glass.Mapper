using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.Proxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver
{
    public class ObjectCachingResolverTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.DisableCache) return;

            if (args.ObjectCacheConfiguration == null) return;

            if (!args.ObjectCacheConfiguration.ObjectCache.ContansObject(args)) return;

            args.Result =
                args.Result =
                CacheProxyGenerator.CreateProxy(
                    args.ObjectCacheConfiguration.ObjectCache.GetObject(args));
            args.ObjectOrigin = ObjectOrigin.ObjectCachingResolver;
            args.AbortPipeline();
        }
    }
}
