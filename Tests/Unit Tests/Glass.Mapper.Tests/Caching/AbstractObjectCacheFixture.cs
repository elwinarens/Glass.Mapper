using System;
using System.Threading.Tasks;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Caching.ObjectCaching.Implementations;
using Glass.Mapper.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class ObjectCachingFixture
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
            var templateString = "templateString";
            var key = "Can_Add_ObjectKey";
            
            _args.Result.Returns(test);

            _cacheKey.ToString().Returns(key);

            objectCache.AddObject(_args);
            Assert.IsTrue(objectCache.ContansObject(_args));
        }

        
        //[Test]
        //[TestCase(typeof(CacheTable<int>))]
        //[TestCase(typeof(MemoryCache<int>))]
        //public void Does_Remember_Template(Type type)
        //{
        //    var baseKey = "Does_Remember_Template";

        //    var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

        //    var test = new Test();
        //    test.MyProperty = "Can_Add_Object";
        //    var templateString = "templateString";
        //    var key = "CreateKey";

        //    objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

        //    Assert.IsTrue(objectCache.GetKeys(templateString, ObjectCachePriority.High).Any());
        //}

        [Test]
        [TestCase(typeof(CacheTable<int>))]
        [TestCase(typeof(MemoryCache<int>))]
        public void Can_Add_Thread_Safe(Type type)
        {
            var baseKey = "Can_Add_Thread_Safe";

            var cacheKeyResolver = Substitute.For<AbstractCacheKeyResolver<int>>();
            var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { cacheKeyResolver });
            var templateString = "templateString";
            var key = "Can_Add_Thread_Safe";

            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var test = new StubClass();
                    test.MyProperty = key + j + i;

                    var cacheKey = Substitute.For<CacheKey<int>>();

                    var args = Substitute.For<ObjectConstructionArgs>();
                    cacheKey.ToString().Returns(x => key + j + i);
                    cacheKeyResolver.GetKey(args).Returns(x => cacheKey);
                    cacheKeyResolver.GetKey(args).ToString().Returns(x => key + j + i);

                    
                    args.Result = test;
                    
                    
                    objectCache.AddObject(args);

                    Assert.IsTrue(objectCache.ContansObject(args));
                }
            });
        }


        //[Test]
        //[TestCase(typeof(CacheTable<int>))]
        //[TestCase(typeof(MemoryCache<int>))]
        //public void Can_Add_Object_To_Multiple_Releated_Cache(Type type)
        //{
        //    var baseKey = "Can_Add_Object_To_Multiple_Releated_Cache";

        //    var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

        //    var test = new Test();
        //    test.MyProperty = "Can_Add_Object";
        //    var templateStrings = new List<string>() {"templateString1", "templateString2"};

        //    var key = "Can_Add_Object_To_Multiple_Releated_Cache";

        //    objectCache.Add<Test>(key, test, templateStrings, ObjectCachePriority.High);

        //    Assert.AreEqual(test, objectCache.Get<Test>(key));
        //}

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Add_Object_To_Multiple_Releated_Cache_Thread_Safe(Type type)
       // {
       //     var baseKey = "Can_Add_Object_To_Multiple_Releated_Cache_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";
       //     var templateStrings = new List<string>() { "templateString1", "templateString2" };

       //     var key = "Can_Add_Object_To_Multiple_Releated_Cache_Thread_Safe";

       //     Parallel.For(0, 10, i =>
       //     {
       //         for (int j = 0; j < 100; j++)
       //         {
       //             objectCache.Add<Test>(key + i + j, test, templateStrings, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));
       //         }
       //     });
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Clear_Object_In_Multiple_Releated_Cache(Type type)
       // {
       //     var baseKey = "Can_Clear_Object_In_Multiple_Releated_Cache";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";
       //     var templateStrings = new List<string>() { "templateString1", "templateString2" };

       //     var key = "Can_Clear_Object_In_Multiple_Releated_Cache";

       //     objectCache.Add<Test>(key, test, templateStrings, ObjectCachePriority.High);

       //     Assert.AreEqual(test, objectCache.Get<Test>(key));
       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0], ObjectCachePriority.High));
       //     Assert.IsTrue(objectCache.Get<Test>(key) == null);

       //     objectCache.Add<Test>(key, test, templateStrings, ObjectCachePriority.High);

       //     Assert.AreEqual(objectCache.Get<Test>(key), test);
       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[1], ObjectCachePriority.High));
       //     Assert.IsTrue(objectCache.Get<Test>(key) == null);
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Clear_Object_In_Multiple_Releated_Cache_Thread_Safe(Type type)
       // {
       //     var baseKey = "Can_Add_Object_To_Multiple_Releated_Cache_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";
            
       //     var key = "Can_Add_Object_To_Multiple_Releated_Cache_Thread_Safe";

       //     Parallel.For(0, 10, i =>
       //     {
       //         for (int j = 0; j < 100; j++)
       //         {
       //             var templateStrings = new List<string>() { "templateString1" + i, "templateString2" + i };

       //             objectCache.Add<Test>(key + i + j, test, templateStrings, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));
       //             Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0], ObjectCachePriority.High));
       //             Assert.IsNull(objectCache.Get<Test>(key + i + j));

       //             objectCache.Add<Test>(key + i + j, test, templateStrings, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));
       //             Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[1], ObjectCachePriority.High));
       //             Assert.IsTrue(objectCache.Get<Test>(key + i + j) == null);
       //         }
       //     });
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Clear_Releated_Cache_With_Multiple_Object_In_Multiple_Releated_Cache(Type type)
       // {
       //     var baseKey = "Can_Clear_Releated_Cache_With_Multiple_Object_In_Multiple_Releated_Cache";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object1";

       //     var test2 = new Test();
       //     test.MyProperty = "Can_Add_Object2";

       //     var templateStrings = new List<string>() { "templateString1", "templateString2" };

       //     var key = "Can_Clear_Object_In_Multiple_Releated_Cache";
       //     var key2 = "Can_Clear_Object_In_Multiple_Releated_Cache2";

       //     objectCache.Add<Test>(key, test, templateStrings, ObjectCachePriority.High);
       //     objectCache.Add<Test>(key2, test2, templateStrings, ObjectCachePriority.High);

       //     Assert.AreEqual(objectCache.Get<Test>(key), test);
       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0], ObjectCachePriority.High));
       //     Assert.IsNull(objectCache.Get<Test>(key));
       //     Assert.IsNull(objectCache.Get<Test>(key2));
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Does_Clearing_One_Releated_Cache_Clear_Another(Type type)
       // {
       //     var baseKey = "Does_Clearing_One_Releated_Cache_Clear_Another";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object1";

       //     var test2 = new Test();
       //     test.MyProperty = "Can_Add_Object2";

       //     var templateStrings = new List<string>() { "templateString1", "templateString2" };

       //     var key = "Can_Clear_Object_In_Multiple_Releated_Cache";
       //     var key2 = "Can_Clear_Object_In_Multiple_Releated_Cache2";

       //     objectCache.Add<Test>(key, test, templateStrings, ObjectCachePriority.High);
       //     objectCache.Add<Test>(key2, test2, templateStrings, ObjectCachePriority.High);

       //     Assert.IsNotNull(objectCache.GetReleatedKeys(ObjectCachePriority.High));
       //     Assert.AreEqual(templateStrings[0], objectCache.GetReleatedKeys(ObjectCachePriority.High).First().Key);

       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateStrings[0], ObjectCachePriority.High));

       //     Assert.IsNotNull(objectCache.GetReleatedKeys(ObjectCachePriority.High));

       //     Assert.IsNull(objectCache.Get<Test>(key));
       //     Assert.IsNull(objectCache.Get<Test>(key2));

       //     //Assert.IsFalse(objectCache.GetReleatedKeys(ObjectCachePriority.High).Keys.con);

       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Clear_Releated_Cache(Type type)
       // {
       //     var baseKey = "Can_Clear_Releated_Cache";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";
       //     var templateString = "templateString";
       //     var key = "Can_Clear_Releated_CacheKey";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreEqual(objectCache.Get<Test>(key), test);
       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateString, ObjectCachePriority.High));
       //     Assert.IsTrue(objectCache.Get<Test>(key) == null);
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Clear_Releated_Thread_Safe(Type type)
       // {
       //     var baseKey = "Can_Clear_Releated_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "Can_Clear_Releated_Thread_SafetemplateString";
       //     var key = "Can_Clear_Releated_Thread_Safe";

       //     Parallel.For(0, 10, i =>
       //     {
       //         for (int j = 0; j < 100; j++)
       //         {
       //             var test = new Test();
       //             test.MyProperty = "Can_Clear_Releated_Thread_Safe";

       //             objectCache.Add<Test>(key + i + j, test, templateString + i, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));
       //             Assert.IsTrue(objectCache.ClearRelatedCache(templateString + i, ObjectCachePriority.High));
       //             Assert.IsNull(objectCache.Get<Test>(key + i + j));
       //         }
       //     });
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Get_Object_By_Key(Type type)
       // {
       //     var baseKey = "Can_Get_Object_By_Key";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Cant_Add_NullKey";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreEqual(test, objectCache.Get<Test>(key));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Get_Object_By_Key_Thread_Safe(Type type)
       // {
       //     var baseKey = "Can_Get_Object_By_Key_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "CreateKey";

       //     Parallel.For(0, 10, i =>
       //     {
       //         for (int j = 0; j < 100; j++)
       //         {
       //             var test = new Test();
       //             test.MyProperty = "Can_Add_Object";

       //             objectCache.Add<Test>(key + i + j, test, templateString, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));
       //         }
       //     });
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Remove_Object_By_Key(Type type)
       // {
       //     var baseKey = "Can_Remove_Object_By_Key";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Cant_Add_NullKey";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreEqual(objectCache.Get<Test>(key), test);

       //     objectCache.Remove(key);

       //     Assert.IsNull(objectCache.Get<Test>(key));
       //     Assert.IsNotNull(objectCache.GetReleatedKeys(ObjectCachePriority.High));
       //     Assert.IsFalse(objectCache.GetReleatedKeys(ObjectCachePriority.High).ContainsKey(templateString));
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Remove_Object_By_Key_Thread_Safe(Type type)
       // {
       //     var baseKey = "Can_Remove_Object_By_Key_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Can_Remove_Object_By_Key_Thread_SafeKey";

       //     Parallel.For(0, 10, i =>
       //     {
       //         for (int j = 0; j < 100; j++)
       //         {
       //             var test = new Test();
       //             test.MyProperty = "Can_Add_Object";

       //             objectCache.Add<Test>(key + i + j, test, templateString, ObjectCachePriority.High);

       //             Assert.AreEqual(test, objectCache.Get<Test>(key + i + j));

       //             //objectCache.Remove(key + i + j);

       //             //Assert.IsNull(objectCache.Get<Test>(key + i + j));
       //         }
       //     });
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Get_ObjectCachePriority_ReleatedKeys(Type type)
       // {
       //     var baseKey = "Get_ObjectCachePriority_ReleatedKeys";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Cant_Add_NullKey";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.IsNotNull(objectCache.GetReleatedKeys(ObjectCachePriority.High));
       //     Assert.AreEqual(templateString, objectCache.GetReleatedKeys(ObjectCachePriority.High).First().Key);
       // }

       // [Test]
       // [ExpectedException(typeof(DuplicatedKeyObjectCacheException))]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Cant_Add_Two_Objects_With_Same_Key(Type type)
       // {
       //     var baseKey = "Cant_Add_Two_Objects_With_Same_Key";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Cant_Add_NullKey";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     var test1 = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     Assert.IsTrue(objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High));
       //     Assert.IsFalse(objectCache.Add<Test>(key, test1, templateString, ObjectCachePriority.High));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Clearing_Differnt_Releated_Cache_Wont_Remove_Object(Type type)
       // {
       //     var baseKey = "Clearing_Differnt_Releated_Cache_Wont_Remove_Object";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Clearing_Differnt_Releated_Cache_Wont_Remove_Object";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreSame(test, objectCache.Get<Test>(key));
       //     Assert.IsTrue(objectCache.GetReleatedKeys(ObjectCachePriority.High).Any(x => x.Key == templateString));

       //     Assert.IsTrue(objectCache.ClearRelatedCache("Another_Clearing_Differnt_Releated_Cache_Wont_Remove_Object", ObjectCachePriority.High));

       //     Assert.IsTrue(objectCache.GetReleatedKeys(ObjectCachePriority.High).Any(x => x.Key == templateString));

       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Add_To_Cache_Clear_Releated_And_Readd(Type type)
       // {
       //     var baseKey = "Add_To_Cache_Clear_Releated_And_Readd";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Cant_Add_NullKey";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreSame(test, objectCache.Get<Test>(key));
       //     Assert.IsTrue(objectCache.GetReleatedKeys(ObjectCachePriority.High).Any(x => x.Key == templateString));

       //     Assert.IsTrue(objectCache.ClearRelatedCache(templateString, ObjectCachePriority.High));

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreSame(test, objectCache.Get<Test>(key));
       //     Assert.IsTrue(objectCache.GetReleatedKeys(ObjectCachePriority.High).Any(x => x.Key == templateString));

       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Add_To_Cache_Clear_Releated_And_Readd_Thread_Safe(Type type)
       // {
       //     var baseKey = "Add_To_Cache_Clear_Releated_And_Readd_Thread_Safe";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Add_To_Cache_Clear_Releated_And_Readd_Thread_Safe";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object";

       //     Parallel.For(0, 2, i =>
       //     {
       //         objectCache.Add<Test>(key + i, test, templateString + i, ObjectCachePriority.High);

       //         Assert.AreSame(test, objectCache.Get<Test>(key + i));
       //         Assert.IsTrue(objectCache.DoesReleatedTemplateIDExist(templateString + i, ObjectCachePriority.High));

       //         Assert.IsTrue(objectCache.ClearRelatedCache(templateString + i, ObjectCachePriority.High));

       //         objectCache.Add<Test>(key + i, test, templateString + i, ObjectCachePriority.High);

       //         Assert.AreSame(test, objectCache.Get<Test>(key + i));
       //         Assert.IsTrue(objectCache.DoesReleatedTemplateIDExist(templateString + i, ObjectCachePriority.High));
                
       //     });

       // }

       // [Test]
       // [ExpectedException(typeof(KeyNullOrEmptyObjectCacheException))]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Does_Empty_Key_Fail(Type type)
       // {
       //     var baseKey = "Does_Empty_Key_Fail";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "";

       //     var test = new Test();
       //     test.MyProperty = "Does_Empty_Key_Fail";

       //     Assert.IsFalse(objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High));
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Add_Colection(Type type)
       // {
       //     var baseKey = "Can_Add_Colection";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });
            
       //     var templateString = "templateString";
       //     var key = "Can_Add_Colection";

       //     var list = new List<string>() { "1", "2", "3", "4", "5" };

       //     Assert.IsTrue(objectCache.Add<List<string>>(key, list, templateString, ObjectCachePriority.High));
       //     Assert.AreEqual(list, objectCache.Get<List<string>>(key));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Rebuild_On_Remove(Type type)
       // {
       //     var baseKey = "Can_Add_Colection";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Can_Add_Colection";

       //     Func<List<string>> buildList = delegate()
       //     { 
       //         return new List<string>() { "1", "2", "3", "4", "5" }; 
       //     };

       //     var list = buildList.Invoke();

       //     Assert.IsTrue(objectCache.Add<List<string>>(key, list, templateString, ObjectCachePriority.High, buildList));
       //     Assert.AreEqual(list, objectCache.Get<List<string>>(key));

       //     objectCache.Remove(key);

       //     Assert.AreEqual(list, objectCache.Get<List<string>>(key));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Will_Rebuild_On_Remove(Type type)
       // {
       //     var baseKey = "Can_Add_Colection";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Can_Add_Colection";

       //     Func<List<string>> buildList = delegate()
       //     {
       //         return new List<string>() { "1", "2", "3", "4", "5" };
       //     };

       //     var list = buildList.Invoke();

       //     Assert.IsTrue(objectCache.Add<List<string>>(key, list, templateString, ObjectCachePriority.High, buildList));

       //     Assert.IsTrue(objectCache.WillRebuildOnRemove(key));

       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       //// [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Add_Object_With_Custom_Timeout(Type type)
       // {
       //     var baseKey = "Can_Add_Object_With_Custom_Timeout";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Can_Add_Object_With_Custom_Timeout";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object_With_Custom_Timeout";

       //     var ts = new TimeSpan(0, 0, 5);

       //     objectCache.Add<Test>(key, test, templateString, ts);

       //     Assert.AreEqual(test, objectCache.Get<Test>(key));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Add_Object_With_Custom_Timeout_Will_Expire(Type type)
       // {
       //     var baseKey = "Can_Add_Object_With_Custom_Timeout_Will_Expire";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Can_Add_Object_With_Custom_Timeout_Will_Expire";

       //     var test = new Test();
       //     test.MyProperty = "Can_Add_Object_With_Custom_Timeout_Will_Expire";

       //     var ts = new TimeSpan(0, 0, 1);

       //     objectCache.Add<Test>(key, test, templateString, ts);

       //     //sleep some so the expiry is after now
       //     Thread.Sleep(2000);

       //     Assert.IsNull(objectCache.Get<Test>(key));
       // }


       // [Test]
       // public void Can_Parse_String_To_ObjectCachePriority(Type type)
       // {
       //     Assert.AreEqual(ObjectCachePriority.High, ObjectCache.ParseObjectCachePriority("High"));
       //     Assert.AreEqual(ObjectCachePriority.Medium, ObjectCache.ParseObjectCachePriority("Medium"));
       //     Assert.AreEqual(ObjectCachePriority.Low, ObjectCache.ParseObjectCachePriority("Low"));
       //     Assert.AreEqual(ObjectCachePriority.Custom, ObjectCache.ParseObjectCachePriority("Custom"));
       // }

       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Turn_Cache_Off(Type type)
       // {
       //     var baseKey = "Can_Turn_Cache_Off";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new[] { baseKey });

       //     var templateString = "templateString";
       //     var key = "Type type";

       //     var test = new Test();
       //     test.MyProperty = "Can_Turn_Cache_Off";

       //     Assert.IsTrue(objectCache.UseCache);

       //     objectCache.TurnCacheOff();

       //     Assert.IsFalse(objectCache.UseCache);

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.IsNull(objectCache.Get<Test>(key));
       // }


       // [Test]
       // [TestCase(typeof(CacheTable<int>))]
       // [TestCase(typeof(MemoryCache<int>))]
       // public void Can_Turn_Cache_On(Type type)
       // {
       //     var baseKey = "Can_Turn_Cache_Off";

       //     var objectCache = (IAbstractObjectCache)System.Activator.CreateInstance(type, new object[] { baseKey});
       //     objectCache.TurnCacheOff();

       //     var templateString = "templateString";
       //     var key = "Type type";

       //     var test = new Test();
       //     test.MyProperty = "Can_Turn_Cache_Off";

       //     Assert.IsFalse(objectCache.UseCache);

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.IsNull(objectCache.Get<Test>(key));

       //     objectCache.TurnCacheOn();

       //     Assert.IsTrue(objectCache.UseCache);            

       //     objectCache.Add<Test>(key, test, templateString, ObjectCachePriority.High);

       //     Assert.AreEqual(test, objectCache.Get<Test>(key));
       // }


       // [Test]
       // public void ObjectCachePriority_Medium_Returned_When_Parse_Invalid_String_To_ObjectCachePriority(Type type)
       // {
       //     Assert.AreEqual(ObjectCachePriority.Medium, ObjectCache.ParseObjectCachePriority("Null_Returned_When_Parse_Invalid_String_To_ObjectCachePriority"));
       // }

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
