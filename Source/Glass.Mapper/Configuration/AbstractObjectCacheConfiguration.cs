using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.ObjectCaching;

namespace Glass.Mapper.Configuration
{
    public abstract class AbstractObjectCacheConfiguration
    {
        public IAbstractObjectCache ObjectCache { get; set; }

        protected AbstractObjectCacheConfiguration()
            : this(Context.Default)
        {

        }

        protected AbstractObjectCacheConfiguration(string contextName)
            : this(Context.Contexts[contextName])
        {
        }

        protected AbstractObjectCacheConfiguration(Context glassContext)
        {
            ObjectCache = glassContext.DependencyResolver.Resolve<IAbstractObjectCache>(new Dictionary<string, object> { { "glassContext", glassContext } });
        }
    }
}
