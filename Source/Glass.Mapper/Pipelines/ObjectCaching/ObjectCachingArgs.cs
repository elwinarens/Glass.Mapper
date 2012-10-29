using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Pipelines.ObjectCaching
{
    public class ObjectCachingArgs : ObjectConstructionArgs
    {

        public ObjectCachingArgs(Context context, IDataContext dataContext, AbstractTypeConfiguration configuration)
            : base(context, dataContext, configuration)
        {
        }
    }
}
