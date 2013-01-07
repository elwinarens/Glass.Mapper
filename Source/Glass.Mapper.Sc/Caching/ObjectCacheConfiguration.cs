using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Caching
{
    public class ObjectCacheConfiguration:AbstractObjectCacheConfiguration
    {
         protected ObjectCacheConfiguration()
            : base(Context.Default)
        {

        }

         protected ObjectCacheConfiguration(string contextName)
            : base(Context.Contexts[contextName])
        {
        }

        public ObjectCacheConfiguration(Context glassContext)
            : base(glassContext)
        {
        }
    }
}
