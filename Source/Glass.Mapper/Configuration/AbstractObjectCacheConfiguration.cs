using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.ObjectCaching;

namespace Glass.Mapper.Configuration
{
    public abstract class AbstractObjectCacheConfiguration
    {
        public AbstractObjectCache ObjectCache { get; set; }

        public AbstractObjectCacheConfiguration()
            : this(Context.Default)
        {

        }

        public AbstractObjectCacheConfiguration(string contextName)
            : this(Context.Contexts[contextName])
        {
        }

        public AbstractObjectCacheConfiguration(Context glassContext)
        {
            ObjectCache = glassContext.DependencyResolver.Resolve<AbstractObjectCache>(new Dictionary<string, object> { { "glassContext", glassContext } });
        }
    }
}
