using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Modules.Inventory.Tests
{
    public class InventoryTests : TestsBase
    {
        [Test]
        public void Construct_Success()
        {
            // Arrange
            var capacity = 5;

            // Act
            var inventory = new Inventory(capacity);

            // Assert
            Assert.IsNotNull(inventory);
            Assert.AreEqual(capacity, inventory.Capacity);

            for (int i = 0; i < capacity; i++)
                Assert.DoesNotThrow(() => { var slot = inventory[i]; });
        }

        [Test]
        public void Construct_WithZeroCapacity_ThrowsArgumentOutOfRange()
        {
            // Arrange, Act & Assert
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new Inventory(0));
        }

        [Test]
        public void Add_WithNewItem_Success()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;

            // Act & Assert
            Assert.DoesNotThrow(() => inventory.Add(item, quantity));
            Assert.AreEqual(item.Id, inventory[0].Item.Id);
            Assert.AreEqual(quantity, inventory[0].Quantity);
        }

        [Test]
        public void Add_WithExistingItem_Success()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;
            inventory.Add(item, quantity);
            uint newQuantity = 4;

            // Act
            inventory.Add(item, newQuantity);

            // Assert
            Assert.AreEqual(item.Id, inventory[0].Item.Id);
            Assert.AreEqual(quantity + newQuantity, inventory[0].Quantity);
        }

        [Test]
        public void Add_WithNoItem_ThrowsArgumentNull()
        {
            // Arrange
            var inventory = new Inventory(1);

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() => inventory.Add(null, 0));
        }

        [Test]
        public void Add_WithZeroQuantity_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");

            // Act & Assert
            Assert.Throws<System.ArgumentOutOfRangeException>(() => inventory.Add(item, 0));
        }

        [Test]
        public void GetSlotOrNextEmpty_ForItem_Success()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");
            inventory.Add(item, 2);

            // Act
            var slot = inventory.GetSlotOrNextEmpty(item);

            // Assert
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.IsEmpty);
            Assert.AreEqual(slot.Item.Id, item.Id);
        }

        [Test]
        public void GetSlotOrNextEmpty_ForEmpty_Success()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");

            // Act
            var slot = inventory.GetSlotOrNextEmpty(item);

            // Assert
            Assert.IsNotNull(slot);
            Assert.IsTrue(slot.IsEmpty);
        }

        [Test]
        public void GetSlotOrNextEmpty_ThrowsArgumentNull()
        {
            // Arrange
            var inventory = new Inventory(1);

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() => inventory.GetSlotOrNextEmpty(null));
        }

        [Test]
        public void Serialize_Success()
        {
            // Arrange
            var inventory = new Inventory(1);
            var item = TestItem.CreateInstance(1, "test");
            var itemsField = typeof(ItemManager).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            itemsField.SetValue(ItemManager.GetComponent<ItemManager>(), new List<ItemBase>() { item });
            uint count = 2;
            inventory.Add(item, count);

            // Act
            var json = JsonConvert.SerializeObject(inventory, new ItemConverter());
            inventory = JsonConvert.DeserializeObject<Inventory>(json, new ItemConverter());

            // Assert
            Assert.AreEqual(1, inventory.Capacity);
            Assert.AreEqual(item, inventory[0].Item);
            Assert.AreEqual(count, inventory[0].Quantity);
        }
    }
}
