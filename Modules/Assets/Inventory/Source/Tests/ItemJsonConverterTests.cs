using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Modules.Inventory.Tests
{
    public class ItemJsonConverterTests : TestsBase
    {
        [Test]
        public void Write_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");

            // Act
            var json = JsonConvert.SerializeObject(item, new ItemConverter());

            // Assert
            Assert.NotNull(json);

            try
            {
                var value = JsonConvert.DeserializeObject<int>(json);
                Assert.AreEqual(item.Id, value);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [Test]
        public void Read_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            var json = JsonConvert.SerializeObject(item, new ItemConverter());
            var itemsField = typeof(ItemManager).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            itemsField.SetValue(ItemManager.GetComponent<ItemManager>(), new List<ItemBase>() { item });

            // Act
            var result = JsonConvert.DeserializeObject<ItemBase>(json, new ItemConverter());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, item);
        }
    }
}
