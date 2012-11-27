using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;

namespace Glass.Mapper.Pipelines.ObjectCaching.Tasks
{
    public class ObjectCachingTask : IObjectCachingTask
    {
        public void Execute(ObjectCachingArgs args)
        {
            args.ObjectConstructionTask.Execute(args as ObjectConstructionArgs);
        }
    }
}
