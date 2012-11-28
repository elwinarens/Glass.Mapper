using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver
{
    public class ObjectCachingResolverTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.ObjectCacheConfiguration.ObjectCache.ContansObject(args))
            {
                args.Result = args.ObjectCacheConfiguration.ObjectCache.GetObject(args);
                args.ObjectOrigin = ObjectOrigin.ObjectCachingResolver;
                args.AbortPipeline();
            }
        }
    }
}
