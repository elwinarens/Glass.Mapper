using System;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.ObjectCachingResolver;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Pipelines.ObjectConstruction;

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

            AbstractObjectCacheConfiguration cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<ObjectCache>();

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service, cacheConfiguration);


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

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            AbstractObjectCacheConfiguration cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<ObjectCache>();

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service, cacheConfiguration);

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
