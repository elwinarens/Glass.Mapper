using System;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class CacheKeyResolverFixture
    {
        [Test]
        public void ResolveKey_GetKey()
        {
            //Assign
            var cacheKeyResolver = Substitute.For<CacheKeyResolver>();

            

            Type type = typeof(StubClass);

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(type);

            AbstractObjectCacheConfiguration cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<ObjectCache>();

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration, cacheConfiguration);


            var key = new CacheKey(Guid.Empty, "database", type);
            cacheKeyResolver.GetKey(args).ReturnsForAnyArgs(key);

            //Act
            var newKey = cacheKeyResolver.GetKey(args);

            //Assert
            Assert.AreEqual(key, newKey);
            Assert.AreEqual(key.ToString(), newKey.ToString());

        }

        #region Stubs

        public class StubClass
        {

        }

        public interface IStubInterface
        {

        }




        #endregion
    }
}
