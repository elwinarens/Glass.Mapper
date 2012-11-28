using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.ObjectCaching;

namespace Glass.Mapper.Configuration
{
    public abstract class AbstractObjectCacheConfiguration
    {
        public ObjectCache ObjectCache { get; set; }
    }
}
