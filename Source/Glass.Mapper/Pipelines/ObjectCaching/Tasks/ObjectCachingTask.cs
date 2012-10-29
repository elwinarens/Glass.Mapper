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
        private IObjectConstructionTask _objectConstructionTask;

        public ObjectCachingTask()
        {
            _objectConstructionTask = new CreateConcreteTask();
        }

        public void Execute(ObjectCachingArgs args)
        {
            _objectConstructionTask.Execute(args as ObjectConstructionArgs);
        }
    }
}
