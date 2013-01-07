using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching.Implementations;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class CacheTableFixture
    {
        private CacheTable<int> _cacheTable;
        private CacheKey<int> _cacheKey;

        [SetUp]
        public void SetUp()
        {
            var args = Substitute.For<ObjectConstructionArgs>();
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            _cacheKey = Substitute.For<CacheKey<int>>();
            cacheKeyResolver.GetKey(args).Returns(_cacheKey);
            _cacheTable = new CacheTable<int>(cacheKeyResolver);
            _cacheTable.ClearCache();
        }

        [Test]
        public void CanGetGetObjectFromCacheTable()
        {
            //Assign
            var stubClass = new StubClass();

            var args = Substitute.For<ObjectConstructionArgs>();
            args.Result.Returns(stubClass);
            

            //Act
            _cacheTable.AddObject(args);

            //Assert 

            Assert.AreEqual(args.Result, stubClass);
            Assert.AreEqual(_cacheTable.GetObject(args), stubClass);

        }

        [Test]
        public void CanGetContansObjectFromCacheTable()
        {
            //Assign
            var stubClass = new StubClass();

            var args = Substitute.For<ObjectConstructionArgs>();
            args.Result.Returns(stubClass);


            //Act
            _cacheTable.AddObject(args);

            //Assert 

            Assert.AreEqual(args.Result, stubClass);
            Assert.IsTrue(_cacheTable.ContansObject(args));

        }

        [Test]
        public void CantGetContansObjectFromCacheTable()
        {
            //Assign
            var stubClass = new StubClass();

            var args = Substitute.For<ObjectConstructionArgs>();
            args.Result.Returns(null);


            //Act
            Assert.IsFalse(_cacheTable.AddObject(args));

            //Assert 

            Assert.AreEqual(args.Result, null);
            Assert.IsFalse(_cacheTable.ContansObject(args));

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
