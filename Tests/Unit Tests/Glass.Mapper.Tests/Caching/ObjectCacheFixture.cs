using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.ObjectCaching;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class ObjectCacheFixture
    {
        #region Stubs

        public class TestObjectCache : ObjectCache
        {
            private volatile Hashtable testCache = new Hashtable();

            public override object GetObject(Mapper.Pipelines.ObjectConstruction.ObjectConstructionArgs args)
            {
                throw new NotImplementedException();
            }

            public override bool ContansObject(Mapper.Pipelines.ObjectConstruction.ObjectConstructionArgs args)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
