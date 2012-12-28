using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class ObjectCachingFixture
    {
        protected Database Database { get; set; }

        [SetUp]
        public void SetUp()
        {
            Database = Sitecore.Configuration.Factory.GetDatabase("master");
        }
    }
}
