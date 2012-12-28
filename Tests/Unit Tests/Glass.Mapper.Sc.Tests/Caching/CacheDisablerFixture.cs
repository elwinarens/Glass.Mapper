using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Tests.Caching
{
    [TestFixture]
    public class CacheDisablerFixture
    {
        [Test]
        public void CanDisableCache()
        {
            var args = Substitute.For<ObjectConstructionArgs>();

            using (new CacheDisabler(args))
            {
                Assert.IsTrue(args.DisableCache);
            } 
        }
    }
}
