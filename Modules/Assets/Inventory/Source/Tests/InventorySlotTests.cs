using NUnit.Framework;

namespace Modules.Inventory.Tests
{
    public class InventorySlotTests
    {
        [Test]
        public void Create_Success()
        {
            // Arrange & Act
            var slot = new Inventory.InventorySlot();

            // Assert
            Assert.IsNotNull(slot);
            Assert.IsTrue(slot.IsEmpty);
        }

        [Test]
        public void Create_WithItem_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;

            // Act
            var slot = new Inventory.InventorySlot(item, quantity);

            // Assert
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.IsEmpty);
            Assert.AreEqual(item, slot.Item);
            Assert.AreEqual(quantity, slot.Quantity);
        }

        [Test]
        public void Create_WithItem_ThrowsArgumentNull()
        {
            // Arrange
            TestDelegate testDelegate = () => new Inventory.InventorySlot(null, 1);

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Create_WithItem_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            TestDelegate testDelegate = () => new Inventory.InventorySlot(item, 0);

            // Act & Assert
            Assert.Throws<System.ArgumentOutOfRangeException>(testDelegate);
        }

        [Test]
        public void Set_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;
            var slot = new Inventory.InventorySlot();

            // Act
            slot.Set(item, quantity);

            // Assert
            Assert.IsFalse(slot.IsEmpty);
            Assert.AreEqual(item, slot.Item);
            Assert.AreEqual(quantity, slot.Quantity);
        }

        [Test]
        public void Set_ThrowsArgumentNull()
        {
            // Arrange
            TestDelegate testDelegate = () => new Inventory.InventorySlot(null, 1);

            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Set_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            TestDelegate testDelegate = () => new Inventory.InventorySlot(item, 0);

            // Act & Assert
            Assert.Throws<System.ArgumentOutOfRangeException>(testDelegate);
        }

        [Test]
        public void Empty_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;
            var slot = new Inventory.InventorySlot(item, quantity);

            // Act
            slot.Empty();

            // Assert
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.Item);
            Assert.Zero(slot.Quantity);
        }

        [Test]
        public void SetQuantity_Success()
        {
            // Arrange
            var item = TestItem.CreateInstance(1, "test");
            uint quantity = 2;
            var slot = new Inventory.InventorySlot(item, 1);

            // Act
            slot.SetQuantity(quantity);

            // Assert
            Assert.AreEqual(quantity, slot.Quantity);
        }

        [Test]
        public void SetQuantity_ThrowsInvalidOperation()
        {
            // Arrange
            var slot = new Inventory.InventorySlot();

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() => slot.SetQuantity(2));
        }
    }
}
