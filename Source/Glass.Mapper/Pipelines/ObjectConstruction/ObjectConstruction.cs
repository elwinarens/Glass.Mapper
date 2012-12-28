using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstruction : AbstractPipelineRunner<ObjectConstructionArgs, ObjectConstructionTask>
    {
        public ObjectConstruction(IEnumerable<ObjectConstructionTask> tasks ):base(tasks)
        {
        }



    }
}
