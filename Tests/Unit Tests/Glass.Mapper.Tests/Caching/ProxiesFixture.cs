using System;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Caching.Proxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class ProxiesFixture
    {
        private CacheKey<int> _cacheKey;
        private ObjectConstructionArgs _args;
        private AbstractCacheKeyResolver<int> _cacheKeyResolver;
        private AbstractObjectCache<int> _cache;
        [SetUp]
        public void SetUp()
        {
            Type type = typeof(StubClass);

            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);
            _cache = Substitute.For<AbstractObjectCache<int>>();
            var objectCacheConfiguration = Substitute.For<AbstractObjectCacheConfiguration>();
            objectCacheConfiguration.ObjectCache = _cache;

            context.ObjectCacheConfiguration = objectCacheConfiguration;
            var abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = type;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            _cacheKey = Substitute.For<CacheKey<int>>();
            _args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
            _cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            _cacheKeyResolver.GetKey(_args).Returns(_cacheKey);

            

        }

        [Test]
        public void Can_Create_Proxy_For_Object()
        {
            var test = new StubClass();
            CacheProxyGenerator.CreateProxy(test, _args);
        }


        [Test]
        public void Can_Update_Proxy_For_Object()
        {
            var test = new StubClass();
            test.MyProperty = "test";

            var cacheKey = Substitute.For<CacheKey<int>>(0, 0, "", typeof(StubClass));
            cacheKey.ToString().Returns("0");
            _args.Context.ObjectCacheConfiguration.ObjectCache.AddObject(cacheKey, test);
            _args.Context.ObjectCacheConfiguration.ObjectCache.GetLatestCacheKey(0).ReturnsForAnyArgs(cacheKey);
           

            _args.Context.ObjectCacheConfiguration.ObjectCache.GetObject(cacheKey).Returns(test);


            _args.CacheKey = cacheKey;

            
            var testProxy = (StubClass)CacheProxyGenerator.CreateProxy(test, _args);


            Assert.AreEqual(test.MyProperty, testProxy.MyProperty);

            var test1 = new StubClass();
            test1.MyProperty = "test1";

            var cacheKey1 = Substitute.For<CacheKey<int>>(0, 1, "", typeof(StubClass));
            cacheKey1.ToString().Returns("1");

            _args.Context.ObjectCacheConfiguration.ObjectCache.AddObject(cacheKey1, test1);

            _args.Context.ObjectCacheConfiguration.ObjectCache.GetObject(cacheKey1).Returns(test1);
            CacheMethodInterceptor.LastUpdated = DateTime.Now;

            Assert.AreEqual(test1.MyProperty, testProxy.MyProperty);
        }



        #region Stubs

        public class StubClass
        {

            public virtual string MyProperty { get; set; }
        }

        public interface IStubInterface
        {

        }




        #endregion
    }
}
