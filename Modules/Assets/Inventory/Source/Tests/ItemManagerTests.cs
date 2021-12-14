using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Modules.Inventory.Tests
{
    public class ItemManagerTests : TestsBase
    {
        [Test]
        public void GetItemById_ThrowsInventoryException()
        {
            // Act & Assert
            Assert.Throws<InventoryException>(() => ItemManager.GetById(1));
        }

        [Test]
        public void GetItemById_Success()
        {
            // Arrange
            uint itemId = 1;
            var item = TestItem.CreateInstance(itemId, "item");
            var itemsField = typeof(ItemManager).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            itemsField.SetValue(ItemManager.GetComponent<ItemManager>(), new List<ItemBase>() { item });

            // Act
            var result = ItemManager.GetById(itemId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(itemId, result.Id);
        }
    }
}