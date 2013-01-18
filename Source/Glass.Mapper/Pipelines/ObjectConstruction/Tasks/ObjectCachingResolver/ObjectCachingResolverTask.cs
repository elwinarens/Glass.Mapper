using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching;
using Glass.Mapper.Caching.Proxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver
{
    public class ObjectCachingResolverTask : ObjectConstructionTask
    {
        public override void Execute(ObjectConstructionArgs args)
        {
            if (CacheDisabler.CacheDisabled) return;

            if (args.Context.ObjectCacheConfiguration == null) return;

            if (!args.Context.ObjectCacheConfiguration.ObjectCache.ContansObject(args)) return;
            //args.Result = args.Context.ObjectCacheConfiguration.ObjectCache.GetObject(args);
            args.Result = CacheProxyGenerator.CreateProxy(args.Context.ObjectCacheConfiguration.ObjectCache.GetObject(args), args);


            args.ObjectOrigin = ObjectOrigin.ObjectCachingResolver;
            args.AbortPipeline();
        }
    }
}
