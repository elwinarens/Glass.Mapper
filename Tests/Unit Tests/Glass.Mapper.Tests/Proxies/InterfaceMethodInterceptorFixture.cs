using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Proxies;
using Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Proxies
{
    [TestFixture]
    public class InterfaceMethodInterceptorFixture
    {
        protected ProxyGenerator Generator;
        protected PersistentProxyBuilder Builder;
        protected ObjectConstructionArgs Args;
        protected AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>> Runner;
        
        [SetUp]
        public virtual void Init()
        {
            Builder = new PersistentProxyBuilder();
            Generator = new ProxyGenerator(Builder);

            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            Args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            Runner =
                Substitute.For<AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>>>();

            Args.Result = Substitute.For<StubClass>();
        }

        [Test]
        public void CanCreateInterfaceMethodInterceptor()
        {
            //Assign
            InterfaceMethodInterceptor interceptor = new InterfaceMethodInterceptor(Args, Runner);

            object proxy = Generator.CreateInterfaceProxyWithTarget(
                typeof(IStubInterface), new StubClass(), interceptor);

            IStubInterface instance = (IStubInterface)proxy;

            instance.TestCall("Test");

            Assert.IsNotNull(interceptor.Invocation);

            Assert.IsNotNull(interceptor.Invocation.Arguments);
            Assert.AreEqual(1, interceptor.Invocation.Arguments.Length);
            Assert.AreEqual("Test", interceptor.Invocation.Arguments[0]);
            Assert.AreEqual("Test", interceptor.Invocation.GetArgumentValue(0));
            //Assert.AreEqual("Test", interceptor.Invocation.ReturnValue);

            Assert.IsNotNull(interceptor.Invocation.Proxy);
            Assert.IsNotInstanceOf(typeof(StubClass), interceptor.Invocation.Proxy);

            Assert.IsNotNull(interceptor.Invocation.InvocationTarget);
            Assert.IsInstanceOf(typeof(IStubInterface), interceptor.Invocation.InvocationTarget);
            Assert.IsNotNull(interceptor.Invocation.TargetType);
            Assert.AreSame(typeof(StubClass), interceptor.Invocation.TargetType);

            Assert.IsNotNull(interceptor.Invocation.Method);
            Assert.IsNotNull(interceptor.Invocation.MethodInvocationTarget);
            Assert.AreNotSame(interceptor.Invocation.Method, interceptor.Invocation.MethodInvocationTarget);


            Assert.AreEqual("Test", ((IStubInterface)interceptor.Invocation.InvocationTarget).Test);

        }

        #region Stubs

        public class StubClass : IStubInterface
        {
            private string _test = "Test";

            public string Test
            {
                get { return _test; }
                set { _test = value; }
            }

            public string TestCall(string s)
            {
                return s;
            }
        }

        public interface IStubInterface
        {
            string Test { get; set; }
            string TestCall(string s);
        }

        #endregion
    }
}
