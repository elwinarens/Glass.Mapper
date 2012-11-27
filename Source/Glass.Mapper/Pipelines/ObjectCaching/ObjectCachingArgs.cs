using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;

namespace Glass.Mapper.Pipelines.ObjectCaching
{
    public class ObjectCachingArgs : ObjectConstructionArgs
    {
        private readonly IObjectConstructionTask _objectConstructionTask = null;

        public IObjectConstructionTask ObjectConstructionTask {
            get { return _objectConstructionTask; }
        }

        public ObjectCachingArgs(Context context, IDataContext dataContext, AbstractTypeConfiguration configuration, IObjectConstructionTask objectConstructionTask)
            : base(context, dataContext, configuration)
        {
            _objectConstructionTask = objectConstructionTask;
        }

        public ObjectCachingArgs(Context context, IDataContext dataContext, AbstractTypeConfiguration configuration)
            : base(context, dataContext, configuration)
        {
            _objectConstructionTask = new CreateConcreteTask();
        }
    }
}
