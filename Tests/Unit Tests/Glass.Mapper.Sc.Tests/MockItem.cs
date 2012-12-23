using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Tests
{
    public class MockItem : Item, IMockItem
    {
        public MockItem(FieldList fieldList, string itemName, string database)
            : base(new ID(new Guid()), new ItemData(new ItemDefinition(new ID(new Guid()), itemName, new ID(new Guid()), new ID(new Guid())), Language.Invariant, new Sitecore.Data.Version(1), fieldList), new Database(database))
            {

            }
    }
}
