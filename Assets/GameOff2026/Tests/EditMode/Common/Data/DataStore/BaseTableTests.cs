using System.Collections.Generic;
using System.Reflection;
using GameOff2026.Common.Data.DataStore;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Common.Data.DataStore
{
    [TestFixture]
    public sealed class BaseTableTests
    {
        [Test]
        public void All_WhenListIsSet_ReturnsSerializedItems()
        {
            var table = ScriptableObject.CreateInstance<TestTable>();
            var items = new List<TestTableItem>
            {
                ScriptableObject.CreateInstance<TestTableItem>(),
                ScriptableObject.CreateInstance<TestTableItem>(),
            };

            try
            {
                var field = typeof(BaseTable<TestTableItem>).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(field);
                field.SetValue(table, items);

                CollectionAssert.AreEqual(items, table.all);
            }
            finally
            {
                Object.DestroyImmediate(table);
                foreach (var item in items)
                {
                    Object.DestroyImmediate(item);
                }
            }
        }

        private sealed class TestTable : BaseTable<TestTableItem>
        {
        }

        private sealed class TestTableItem : ScriptableObject
        {
        }
    }
}

