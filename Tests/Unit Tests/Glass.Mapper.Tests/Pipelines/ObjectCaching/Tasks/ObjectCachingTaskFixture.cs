using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectCaching;
using Glass.Mapper.Pipelines.ObjectCaching.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectCaching.Tasks
{
    [TestFixture]

    public class ObjectCachingTaskFixture
    {
        private ObjectCachingTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new ObjectCachingTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_ObjectCaching_ConcreteType_TypeCreated()
        {
            //Assign
            Type type = typeof(StubClass);

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof(StubClass));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);

            ObjectCachingArgs args = new ObjectCachingArgs(context, dataContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsTrue(args.Result.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ObjectCaching_LazyType_LazyTypeCreated()
        {
            //Assign
            Type type = typeof(StubClass);

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof(StubClass));
            dataContext.IsLazy = true;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);

            ObjectCachingArgs args = new ObjectCachingArgs(context, dataContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsFalse(args.Result.GetType() == typeof(StubClass));
        }


        [Test]
        public void Execute_ObjectCaching_ProxyInterface_ProxyGetsCreated()
        {
            //Assign
            Type type = typeof(IStubInterface);

            Context context = Context.Create();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof(IStubInterface));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectCachingArgs args = new ObjectCachingArgs(context, dataContext, configuration, new CreateInterfaceTask());

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.IsAborted);
            Assert.IsTrue(args.Result is IStubInterface);
            Assert.IsFalse(args.Result.GetType() == typeof(IStubInterface));
        }

        #endregion


        public interface IStubInterface
        {

        }

        public class StubClass
        {

        }
    }
}
