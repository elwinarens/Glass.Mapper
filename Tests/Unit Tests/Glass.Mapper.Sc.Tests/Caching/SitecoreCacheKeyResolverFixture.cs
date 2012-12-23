using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using NSubstitute.Core;
using NUnit.Framework;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Glass.Mapper.Sc.Caching.CacheKeyResolving.Implementations;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Sc.Tests.Caching
{
    [TestFixture]
    public class SitecoreCacheKeyResolverFixture
    {
        [Test]
        public void CanGenerateCacheKey()
        {
            //Assign
            var fieldList = new FieldList();
            var revistionId = new Guid();
            fieldList.Add(Sitecore.FieldIDs.Revision, revistionId.ToString());
            var sitecoreItem = new MockItem(fieldList, "CanGenerateCacheKey", "master");

            var type = typeof (StubClass);
            var glassConfig = Substitute.For<GlassCastleConfigBase>();
            var service = Substitute.For<IAbstractService>();

            var context = Context.Create(glassConfig);

            var sitecoreTypeCreationContext = Substitute.For<SitecoreTypeCreationContext>();
            sitecoreTypeCreationContext.RequestedType = typeof (StubClass);
            sitecoreTypeCreationContext.Item = sitecoreItem;

            var configuration = Substitute.For<AbstractTypeConfiguration>();

            var args = new ObjectConstructionArgs(context, sitecoreTypeCreationContext, configuration, service);

            //Act
            var cacheKey = new SitecoreCacheKeyResolver().GetKey(args);

            //Assert

            Assert.AreEqual(new CacheKey(revistionId, "master", type), cacheKey);
        }

        #region Stubs

        public class StubClass
        {

        }

        #endregion
    }
}
