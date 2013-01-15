using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Caching.ObjectCaching;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class CacheInformationFixture
    {
        [Test]
        public void Can_Create_CreateCacheInformation()
        {
            var ci = new CacheInformation();

            Assert.IsNotNull(ci);
        }

        [Test]
        public void Can_Add_Related_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Related_Key";

            ci.AddRelatedKey(releatedKey);

            Assert.Contains(releatedKey, ci.GetRelatedKeys());
        }

        [Test]
        public void Can_Add_Related_Key_Thread_Safe()
        {
            var ci = new CacheInformation();

            var releatedKey = "Can_Add_Related_Key";

            Parallel.For(0, 1000, i =>
            {
                ci.AddRelatedKey(releatedKey + i);
                Assert.Contains(releatedKey + i, ci.GetRelatedKeys());
            });

            for (int i = 0; i < 1000; i++)
            {
                Assert.Contains(releatedKey + i, ci.GetRelatedKeys());
            }
        }

        [Test]
        public void Can_Add_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);
        }

        [Test]
        public void Can_Add_Key_Thread_Safe()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";

            Parallel.For(0, 1000, i =>
            {
                ci.AddObjectKey(releatedKey + i, key + i);

                Assert.Contains(key + i, ci.Keys);
            });

            for (int i = 0; i < 1000; i++)
            {
                Assert.Contains(key + i, ci.Keys);
            }
        }

        [Test]
        public void Does_CacheInformation_Contain_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);

            Assert.IsTrue(ci.ContainsObjectKey(key));
        }

        [Test]
        public void Does_CacheInformation_Contain_Related_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(releatedKey, ci.GetRelatedKeys());

            Assert.IsTrue(ci.ContainsRelatedKey(releatedKey));
        }


        [Test]
        public void Does_CacheInformation_Contain_Key_Thread_Safe()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";
            ci.AddObjectKey(releatedKey, key);


            Parallel.For(0, 1000, i =>
            {
                ci.AddObjectKey(releatedKey + i, key + i);

                Assert.Contains(key + i, ci.Keys);
                Assert.IsTrue(ci.ContainsObjectKey(key + i));
            });

            for (int i = 0; i < 1000; i++)
            {
                Assert.Contains(key + i, ci.Keys);
                Assert.IsTrue(ci.ContainsObjectKey(key + i));
            }
        }

        [Test]
        public void Test_LookUp_Time_For_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";
            var key = "Can_Add_Key";

            Parallel.For(0, 100000, i =>
            {
                ci.AddObjectKey(releatedKey + i, key + i);
            });

            Stopwatch st = new Stopwatch();
            st.Start();

            ci.ContainsObjectKey(key + "1002");

            st.Stop();
            Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());
        }

        [Test]
        public void Can_Get_Add_List_Of_Related_Keys_With_Key()
        {
            var ci = new CacheInformation();
            var relatedKey1 = "Related_Key1";
            var relatedKey2 = "Related_Key2";

            var releatedKeys = new List<string>() { relatedKey1, relatedKey2 };
            var key = "Can_Get_Add_List_Of_Related_Keys_With_Key";

            ci.AddObjectKey(releatedKeys, key);

            Assert.Contains(relatedKey1, ci.GetRelatedKeys());
            Assert.Contains(relatedKey2, ci.GetRelatedKeys());
        }

        [Test]
        public void Can_Get_List_Of_Related_Keys_From_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Add_Key_Related_Key";

            var key1 = "key1";
            var key2 = "key2";

            ci.AddObjectKey(releatedKey, key1);

            ci.AddObjectKey(releatedKey, key2);

            Assert.Contains(key1, ci.GetObjectKeys(releatedKey));
            Assert.Contains(key2, ci.GetObjectKeys(releatedKey));
        }


        [Test]
        public void Can_Remove_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Remove_Key_Related_Key";
            var key = "Can_Remove_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);

            ci.RemoveObjectKey(key);

            Assert.IsFalse(((List<string>)ci.Keys).Any(x => x == key));
        }


        [Test]
        public void Can_Remove_Key_By_Related_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Remove_Key_Related_Key";
            var key = "Can_Remove_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);

            ci.RemoveRelatedKey(releatedKey);

            Assert.IsFalse(((List<string>)ci.Keys).Any(x => x == key));
        }

        [Test]
        public void Related_Key_Is_Removed_When_Remove_Key_By_Related_Key()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Remove_Key_Related_Key";
            var key = "Can_Remove_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);

            ci.RemoveRelatedKey(releatedKey);

            Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == releatedKey));
        }

        [Test]
        public void Related_Key_Is_Removed_When_No_Keys_Are_Left()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Remove_Key_Related_Key";
            var key = "Can_Remove_Key";
            ci.AddObjectKey(releatedKey, key);

            Assert.Contains(key, ci.Keys);

            ci.RemoveObjectKey(key);

            Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == releatedKey));
        }


        [Test]
        public void Related_Key_Is_Removed_When_No_Keys_Are_Left_Thread_Safe()
        {
            var ci = new CacheInformation();
            var releatedKey = "Can_Remove_Key_Related_Key";
            var key = "Can_Remove_Key";


            Parallel.For(0, 1000, i =>
            {
                ci.AddObjectKey(releatedKey + i, key + i);

                Assert.Contains(key + i, ci.Keys);

                ci.RemoveObjectKey(key + i);

                Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == releatedKey + i));
            });


        }


        [Test]
        public void List_Related_Key_Is_Removed_When_No_Keys_Are_Left()
        {
            var ci = new CacheInformation();
            var relatedKey1 = "Related_Key1";
            var relatedKey2 = "Related_Key2";

            var releatedKeys = new List<string>() { relatedKey1, relatedKey2 };
            var key = "Can_Get_Add_List_Of_Related_Keys_With_Key";

            ci.AddObjectKey(releatedKeys, key);

            Assert.Contains(relatedKey1, ci.GetRelatedKeys());
            Assert.Contains(relatedKey2, ci.GetRelatedKeys());

            ci.RemoveObjectKey(key);

            Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == relatedKey1));
            Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == relatedKey2));
        }


        [Test]
        public void List_Related_Key_Is_Removed_When_No_Keys_Are_Left_Thread_Safe()
        {
            var ci = new CacheInformation();
            var relatedKey1 = "Related_Key1";
            var relatedKey2 = "Related_Key2";

            var key = "Can_Get_Add_List_Of_Related_Keys_With_Key";

            Parallel.For(0, 1000, i =>
            {
                var releatedKeys = new List<string>() { relatedKey1 + i, relatedKey2 + i };

                ci.AddObjectKey(releatedKeys, key + i);

                Assert.Contains(relatedKey1 + i, ci.GetRelatedKeys());
                Assert.Contains(relatedKey2 + i, ci.GetRelatedKeys());

                ci.RemoveObjectKey(key + i);

                Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == relatedKey1 + i));
                Assert.IsFalse(((List<string>)ci.GetRelatedKeys()).Any(x => x == relatedKey2 + i));
            });


        }


    }
}
