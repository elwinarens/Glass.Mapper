using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.Proxy;
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

        [SetUp]
        public void SetUp()
        {
            _cacheKey = Substitute.For<CacheKey<int>>();
            _args = Substitute.For<ObjectConstructionArgs>();
            _cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            _cacheKeyResolver.GetKey(_args).Returns(_cacheKey);
        }

        [Test]
        public void Can_Create_Proxy_For_Object()
        {
            var test = new StubClass();
            CacheProxyGenerator.CreateProxy(test, _args);
        }

        #region Stubs

        public class StubClass
        {

            public string MyProperty { get; set; }
        }

        public interface IStubInterface
        {

        }




        #endregion
    }
}
