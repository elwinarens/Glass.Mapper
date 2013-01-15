using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Caching.ObjectCaching.Exceptions;
using Glass.Mapper.Caching.ObjectCaching.Implementations;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class AbstractObjectCacheFixture
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
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Default_Constructor_Sets_Default_BaseCacheKey(Type type)
        {
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type);

            Assert.AreEqual(objectCache.DefaultBaseCacheKey, objectCache.BaseCacheKey);
        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Set_BaseCacheKey_With_Constructor(Type type)
        {
            var baseKey = "Can_Set_BaseCacheKey_With_Constructor";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new object[] { baseKey });

            Assert.AreEqual(baseKey, objectCache.BaseCacheKey);
        }

       
        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_Object(Type type)
        {
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new object[] { _cacheKeyResolver });

            var test = new StubClass();
            test.MyProperty = "Can_Add_Object";
            var key = "Can_Add_ObjectKey";
            
            _args.Result.Returns(test);

            _cacheKey.ToString().Returns(key);

            objectCache.AddObject(_args);
            Assert.IsTrue(objectCache.ContansObject(_args));
            Assert.AreSame(objectCache.GetObject(_args), test);
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_Thread_Safe(Type type)
        {
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });
            var key = "Can_Add_Thread_Safe";

            Parallel.For(0, 10, i =>
            {

                for (var j = 0; j < 100; j++)
                {
                    var localKey = key + "_" + j + "_" + i;
                    var test = new StubClass { MyProperty = localKey };

                    var cacheKey = Substitute.For<CacheKey<int>>();

                    var args = Substitute.For<ObjectConstructionArgs>();
                    cacheKey.ToString().Returns(x => localKey);
                    cacheKeyResolver.GetKey(args).Returns(x => cacheKey);
                    cacheKeyResolver.GetKey(args).ToString().Returns(x => localKey);

                    args.Result = test;
                    
                    
                    objectCache.AddObject(args);

                    Assert.IsTrue(objectCache.ContansObject(args));
                }
            });
        }



        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_To_Related_Cache(Type type)
        {
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });

            var test = new List<StubClass>
                {
                    new StubClass(),
                    new StubClass()
                };

            objectCache.AddToRelatedCache("Can_Add_To_Related_Cache", "releatedKey", test);
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_To_Related_CacheThread_Safe(Type type)
        {
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });


            Parallel.For(0, 10, i =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        var test = new List<StubClass>
                            {
                                new StubClass(),
                                new StubClass()
                            };

                        objectCache.AddToRelatedCache("Can_Add_To_Related_Cache" + i + j, "releatedKey", test);
                    }
                });


        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_To_Multiple_Related_Cache(Type type)
        {
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });

            var test = new List<StubClass>
                {
                    new StubClass(),
                    new StubClass()
                };

            objectCache.AddToRelatedCache("Can_Add_To_Related_Cache", new[] { "releatedKey1", "releatedKey2" }, test);
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_To_Multiple_Related_CacheThread_Safe(Type type)
        {
            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });


            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var test = new List<StubClass>
                            {
                                new StubClass(),
                                new StubClass()
                            };

                    objectCache.AddToRelatedCache("Can_Add_To_Related_Cache" + i + j, "releatedKey", test);
                }
            });
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Clear_Object_In_Multiple_Related_Cache(Type type)
        {
            var baseKey = "Can_Clear_Object_In_Multiple_Related_Cache";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new object[] { baseKey });

            var test = new StubClass {MyProperty = "Can_Add_Object"};
            var templateStrings = new List<string>() { "templateString1", "templateString2" };

            var key = "Can_Clear_Object_In_Multiple_Related_Cache";

            objectCache.AddToRelatedCache(key, templateStrings, test);

            Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0]));
            Assert.IsTrue(objectCache.GetFromRelatedCache<StubClass>(key) == null);

            objectCache.AddToRelatedCache(key, templateStrings, test);

            Assert.AreEqual(objectCache.GetFromRelatedCache<StubClass>(key), test);
            Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[1]));
            Assert.IsTrue(objectCache.GetFromRelatedCache<StubClass>(key) == null);
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Clear_Object_In_Multiple_Related_Cache_Thread_Safe(Type type)
        {
            var baseKey = "Can_Add_Object_To_Multiple_Related_Cache_Thread_Safe";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var test = new StubClass { MyProperty = "Can_Add_Object" };

            var key = "Can_Add_Object_To_Multiple_Related_Cache_Thread_Safe";

            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var localKey = key + i + j;
                    var templateStrings = new List<string>() { "templateString1" + i, "templateString2" + i };

                    objectCache.AddToRelatedCache(localKey, templateStrings, test);

                    Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(localKey));
                    Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0]));
                    Assert.IsTrue(objectCache.GetFromRelatedCache<StubClass>(localKey ) == null);

                    objectCache.AddToRelatedCache(localKey, templateStrings, test);

                    Assert.AreEqual(objectCache.GetFromRelatedCache<StubClass>(localKey), test);
                    Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[1]));
                    Assert.IsTrue(objectCache.GetFromRelatedCache<StubClass>(localKey) == null);
                }
            });
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Clear_Related_Cache_With_Multiple_Object_In_Multiple_Related_Cache(Type type)
        {
            var baseKey = "Can_Clear_Related_Cache_With_Multiple_Object_In_Multiple_Related_Cache";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var test = new StubClass { MyProperty = "Can_Add_Object1" };

            var test2 = new StubClass { MyProperty = "Can_Add_Object2" };

            var templateStrings = new List<string>() { "templateString1", "templateString2" };

            var key = "Can_Clear_Object_In_Multiple_Related_Cache";
            var key2 = "Can_Clear_Object_In_Multiple_Related_Cache2";

            objectCache.AddToRelatedCache(key, templateStrings, test);
            objectCache.AddToRelatedCache(key2, templateStrings, test2);

            Assert.AreEqual(objectCache.GetFromRelatedCache<StubClass>(key), test);
            Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0]));
            Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(key2));
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Does_Clearing_One_Related_Cache_Clear_Another(Type type)
        {
            var baseKey = "Does_Clearing_One_Related_Cache_Clear_Another";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });


            var test = new StubClass { MyProperty = "Can_Add_Object1" };

            var test2 = new StubClass { MyProperty = "Can_Add_Object2" };

            var templateStrings = new List<string>() { "templateString1", "templateString2" };

            var key = "Can_Clear_Object_In_Multiple_Related_Cache";
            var key2 = "Can_Clear_Object_In_Multiple_Related_Cache2";

            objectCache.AddToRelatedCache(key, templateStrings, test);
            objectCache.AddToRelatedCache(key2, templateStrings, test2);

            Assert.IsNotNull(objectCache.GetRelatedKeys());
            Assert.AreEqual(templateStrings[0], objectCache.GetRelatedKeys().First().Key);

            Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0]));

            Assert.IsNotNull(objectCache.GetRelatedKeys());

            Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(key2));

        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Clear_Related_Cache(Type type)
        {
            var baseKey = "Can_Clear_Related_Cache";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var test = new StubClass { MyProperty = "Can_Add_Object1" };
            var templateString = "templateString";
            var key = "Can_Clear_Related_CacheKey";

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreEqual(objectCache.GetFromRelatedCache<StubClass>(key), test);
            Assert.IsTrue(objectCache.ClearRelatedCache(templateString));
            Assert.IsTrue(objectCache.GetFromRelatedCache<StubClass>(key) == null);
        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Clear_Related_Thread_Safe(Type type)
        {
            var baseKey = "Can_Clear_Related_Thread_Safe";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "Can_Clear_Related_Thread_SafetemplateString";
            var key = "Can_Clear_Related_Thread_Safe";

            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var localKey = key + i + j;
                    var localtemplateString = templateString + i;
                    var test = new StubClass { MyProperty = "Can_Clear_Related_Thread_Safe" };

                    objectCache.AddToRelatedCache(localKey, localtemplateString, test);

                    Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(localKey));
                    Assert.IsTrue(objectCache.ClearRelatedCache(localtemplateString));
                    Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(localKey));
                }
            });
        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Get_Object_By_Key(Type type)
        {
            var baseKey = "Can_Get_Object_By_Key";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Cant_Add_NullKey";

            var test = new StubClass { MyProperty = "Can_Get_Object_By_Key" };

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(key));
        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Get_Object_By_Key_Thread_Safe(Type type)
        {
            var baseKey = "Can_Get_Object_By_Key_Thread_Safe";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "CreateKey";

            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var localKey = key + i + j;
                    var test = new StubClass { MyProperty = "Can_Get_Object_By_Key_Thread_Safe" };

                    objectCache.AddToRelatedCache(localKey, templateString, test);

                    Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(localKey));
                }
            });
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Remove_Object_By_Key(Type type)
        {
            var baseKey = "Can_Remove_Object_By_Key";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Cant_Add_NullKey";

            var test = new StubClass { MyProperty = "Can_Remove_Object_By_Key" };

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(key));

            objectCache.RemoveFromRelatedCache(key);

            Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsNotNull(objectCache.GetRelatedKeys());
            Assert.IsFalse(objectCache.GetRelatedKeys().ContainsKey(templateString));
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Remove_Object_By_Key_Thread_Safe(Type type)
        {
            var baseKey = "Can_Remove_Object_By_Key_Thread_Safe";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Can_Remove_Object_By_Key_Thread_SafeKey";

            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var localKey = key + i + j;
                    var test = new StubClass { MyProperty = "Can_Get_Object_By_Key_Thread_Safe" };

                    objectCache.AddToRelatedCache(localKey, templateString, test);

                    Assert.AreEqual(test, objectCache.GetFromRelatedCache<StubClass>(localKey));

                    objectCache.RemoveFromRelatedCache(localKey);

                    Assert.IsNull(objectCache.GetFromRelatedCache<StubClass>(localKey));
                }
            });
        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Get_ObjectCachePriority_RelatedKeys(Type type)
        {
            var baseKey = "Get_ObjectCachePriority_RelatedKeys";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Get_ObjectCachePriority_RelatedKeys";

            var test = new StubClass { MyProperty = "Get_ObjectCachePriority_RelatedKeys" };

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.IsNotNull(objectCache.GetRelatedKeys());
            Assert.AreEqual(templateString, objectCache.GetRelatedKeys().First().Key);
        }

        [Test]
        [ExpectedException(typeof(DuplicatedKeyObjectCacheException))]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Cant_Add_Two_Objects_With_Same_Key(Type type)
        {
            var baseKey = "Cant_Add_Two_Objects_With_Same_Key";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Cant_Add_Two_Objects_With_Same_Key";

            var test = new StubClass { MyProperty = "Cant_Add_Two_Objects_With_Same_Key" };
            var test1 = new StubClass { MyProperty = "Cant_Add_Two_Objects_With_Same_Key" };


            objectCache.AddToRelatedCache(key, templateString, test);
            objectCache.AddToRelatedCache(key, templateString, test1);
        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Clearing_Differnt_Related_Cache_Wont_Remove_Object(Type type)
        {
            var baseKey = "Clearing_Differnt_Related_Cache_Wont_Remove_Object";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Clearing_Differnt_Related_Cache_Wont_Remove_Object";

            var test = new StubClass { MyProperty = "Clearing_Differnt_Related_Cache_Wont_Remove_Object" };

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreSame(test, objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));

            Assert.IsTrue(objectCache.ClearRelatedCache(templateString));

            Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));

        }

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Add_To_Cache_Clear_Related_And_Readd(Type type)
        {
            var baseKey = "Add_To_Cache_Clear_Related_And_Readd";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Cant_Add_NullKey";


            var test = new StubClass { MyProperty = "Add_To_Cache_Clear_Related_And_Readd" };

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreSame(test, objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));

            Assert.IsTrue(objectCache.ClearRelatedCache(templateString));

            objectCache.AddToRelatedCache(key, templateString, test);

            Assert.AreSame(test, objectCache.GetFromRelatedCache<StubClass>(key));
            Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));

        }


        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Add_To_Cache_Clear_Related_And_Readd_Thread_Safe(Type type)
        {
            var baseKey = "Add_To_Cache_Clear_Related_And_Readd_Thread_Safe";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "Add_To_Cache_Clear_Related_And_Readd_Thread_Safe";

            var test = new StubClass { MyProperty = "Add_To_Cache_Clear_Related_And_Readd" };


            Parallel.For(0, 100, i =>
                {
                    var localKey = key + i;

                    objectCache.AddToRelatedCache(localKey, templateString, test);

                    Assert.AreSame(test, objectCache.GetFromRelatedCache<StubClass>(localKey));
                    Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));

                    Assert.IsTrue(objectCache.ClearRelatedCache(templateString));

                    objectCache.AddToRelatedCache(localKey, templateString, test);

                    Assert.AreSame(test, objectCache.GetFromRelatedCache<StubClass>(localKey));
                    Assert.IsTrue(objectCache.GetRelatedKeys().Any(x => x.Key == templateString));
                });

        }

        [Test]
        [ExpectedException(typeof(KeyNullOrEmptyObjectCacheException))]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Does_Empty_Key_Fail(Type type)
        {
            var baseKey = "Does_Empty_Key_Fail";

            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

            var templateString = "templateString";
            var key = "";

            var test = new StubClass { MyProperty = "Add_To_Cache_Clear_Related_And_Readd" };


            objectCache.AddToRelatedCache(key, templateString, test);
        }


        [Test]
        [TestCase(typeof (CacheTable<int>))]
        [TestCase(typeof (MemoryCache<int>))]
        public void Can_Add_Colection(Type type)
        {
            var baseKey = "Can_Add_Colection";

            var objectCache = (IAbstractObjectCache) System.Activator.CreateInstance(type, new[] {baseKey});

            var templateString = "templateString";
            var key = "Can_Add_Colection";

            var list = new List<string>() {"1", "2", "3", "4", "5"};

            objectCache.AddToRelatedCache(key, templateString, list);
            Assert.AreEqual(list, objectCache.GetFromRelatedCache<StubClass>(key));
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
