using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public abstract class ObjectConstructionTask : IPipelineTask<ObjectConstructionArgs>
    {
        public abstract void Execute(ObjectConstructionArgs args);


        public ObjectConstructionTask(int order)
        {
            Order = order;
        }

        public ObjectConstructionTask()
        {
            Order = 0;
        }

        public int Order { get; set; }
    }
}
