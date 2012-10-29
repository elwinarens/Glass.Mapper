using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectCaching
{
    public class ObjectCachingRunner : AbstractPipelineRunner<ObjectCachingArgs, IObjectCachingTask>
    {
        public ObjectCachingRunner(IList<IObjectCachingTask> tasks)
            : base(tasks)
        {
        }
    }
}
