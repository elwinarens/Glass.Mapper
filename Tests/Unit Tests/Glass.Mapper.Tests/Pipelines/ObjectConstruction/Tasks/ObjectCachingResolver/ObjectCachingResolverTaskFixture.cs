using System;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver
{
    [TestFixture]
    public class ObjectCachingResolverTaskFixture
    {
        private ObjectCachingResolverTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new ObjectCachingResolverTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_ObjectCachingResolver_ObjectReturned()
        {
            //Assign
            Type type = typeof(StubClass);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);

            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();

            var cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<AbstractObjectCache<int>>(new object[] { cacheKeyResolver });
            

            context.ObjectCacheConfiguration = cacheConfiguration;

            var abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = type;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            var args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
            cacheKeyResolver.GetKey(args).ReturnsForAnyArgs(Substitute.For<CacheKey<int>>());
            args.Result = new StubClass();

            cacheConfiguration.ObjectCache.GetObject(args).ReturnsForAnyArgs(new StubClass());
            cacheConfiguration.ObjectCache.ContansObject(args).ReturnsForAnyArgs(true);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.IsAborted);
            Assert.IsTrue(ObjectOrigin.ObjectCachingResolver == args.ObjectOrigin);
        }

        [Test]
        public void Execute_ObjectCachingResolver_NotObjectReturned()
        {
            //Assign
            Type type = typeof(StubClass);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);

            var abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
           
            var cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<AbstractObjectCache<int>>(new object[] { cacheKeyResolver });
            
            context.ObjectCacheConfiguration.Returns(cacheConfiguration);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            var args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
            cacheKeyResolver.GetKey(args).ReturnsForAnyArgs(Substitute.For<CacheKey<int>>());

            cacheConfiguration.ObjectCache.ContansObject(args).ReturnsForAnyArgs(false);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
            Assert.IsFalse(args.IsAborted);
        }

        #endregion

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
