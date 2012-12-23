using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingSaver
{
    public class ObjectCachingSaverTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            //Save item to the cache
            args.ObjectCacheConfiguration.ObjectCache.AddObject(args);

            args.Result = Glass.Mapper.ObjectCaching.Proxy.CacheProxyGenerator.CreateProxy(args.Result);
        }
    }
}
