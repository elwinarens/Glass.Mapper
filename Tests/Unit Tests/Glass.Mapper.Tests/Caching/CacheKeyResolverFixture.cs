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
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();

            //Assign
            Type type = typeof(StubClass);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);
            
            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<AbstractObjectCache<int>>();

            context.ObjectCacheConfiguration.Returns(cacheConfiguration);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            var args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);


            var key = Substitute.For<CacheKey<int>>();
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
