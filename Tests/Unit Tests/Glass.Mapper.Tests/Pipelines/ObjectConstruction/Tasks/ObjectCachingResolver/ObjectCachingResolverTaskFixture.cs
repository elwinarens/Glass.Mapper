using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(type);

            AbstractObjectCacheConfiguration cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<ObjectCache>();

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration, cacheConfiguration);


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

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(type);

            AbstractObjectCacheConfiguration cacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            cacheConfiguration.ObjectCache = Substitute.For<ObjectCache>();

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration, cacheConfiguration);

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
