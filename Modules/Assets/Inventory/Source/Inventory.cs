using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Modules.Inventory
{
    [System.Serializable, JsonObject]
    public class Inventory : IEnumerable<Inventory.InventorySlot>
    {
        public InventorySlot this[int index] => _slots[index];
        public int Capacity => _slots.Count;

        [JsonProperty("slots")]
        private readonly List<InventorySlot> _slots;

        public Inventory(int capacity)
        {
            if (capacity < 1)
                throw new System.ArgumentOutOfRangeException(nameof(capacity), "Cannot construct inventory with specified size less than zero.");

            _slots = new List<InventorySlot>(capacity);
            for (int i = 0; i < capacity; i++)
                _slots.Add(new InventorySlot());
        }

        public void Add(ItemBase item, uint quantity)
        {
            if (item == null)
                throw new System.ArgumentNullException(nameof(item));

            if (quantity == 0)
                throw new System.ArgumentOutOfRangeException(nameof(quantity), $"Cannot add item {item.name} with quantity of {quantity}. Quantity must be greater than zero.");

            var slot = GetSlotOrEmpty(item);
            if (slot.IsEmpty)
                slot.Set(item, quantity);
            else
                slot.SetQuantity(slot.Quantity + quantity);
        }

        public void Remove(ItemBase item, uint quantity = 1)
        {
            if (quantity == 0)
                throw new System.ArgumentOutOfRangeException(nameof(quantity), $"Cannot remove item {item.name} with quantity of {quantity}. Quantity must be greater than zero.");

            var slot = GetSlotOrEmpty(item);
            if (slot.IsEmpty)
                throw new InventoryException($"Inventory does not contain the item {item.Name} so it cannot be removed");

            slot.SetQuantity(slot.Quantity - 1);
            if (slot.Quantity == 0)
                slot.Empty();
        }

        public InventorySlot GetSlotOrEmpty(ItemBase item)
        {
            if (item == null)
                throw new System.ArgumentNullException(nameof(item));

            var emptySlotIndex = -1;

            for (int i = 0; i < Capacity; i++)
            {
                var slot = _slots[i];
                if (slot.IsEmpty)
                {
                    if (emptySlotIndex < 0)
                        emptySlotIndex = i;
                }
                else if (slot.Item.Id == item.Id)
                    return slot;
            }

            return _slots[emptySlotIndex];
        }

        public IEnumerator<InventorySlot> GetEnumerator() =>
            _slots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public class InventorySlot
        {
            public ItemBase Item => _item;
            public uint Quantity => _quantity;
            public bool IsEmpty => _item == null;

            [JsonProperty("item")]
            private ItemBase _item;
            [JsonProperty("quantity")]
            private uint _quantity;

            internal InventorySlot() { }

            public InventorySlot(ItemBase item, uint quantity)
            {
                Set(item, quantity);
            }

            internal void Set(ItemBase item, uint quantity = 1)
            {
                if (item == null)
                    throw new System.ArgumentNullException(nameof(item));

                if (quantity < 1)
                    throw new System.ArgumentOutOfRangeException(nameof(quantity), "Cannot populate inventory slot with a quantity less than 1");

                _item = item;
                _quantity = quantity;
            }

            internal void Empty()
            {
                _item = null;
                _quantity = 0;
            }

            internal void SetQuantity(uint quantity)
            {
                if (_item == null)
                    throw new System.InvalidOperationException("Cannot set the quantity of an inventory slot that is empty");

                _quantity = quantity;
            }
        }
    }
}