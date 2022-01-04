using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.GUI
{
    public class InventoryWidget : MonoBehaviour
    {
        [SerializeField]
        private GameObject _inventorySlotPrefab;
        private int _inventorySize;
        private List<InventorySlotWidget> _inventorySlots;

        public void Show(Inventory.Inventory inventory)
        {
            if (inventory.Capacity != _inventorySize)
            {
                _inventorySize = inventory.Capacity;
                RedrawInventory();
            }

            gameObject.SetActive(true);
        }

        private void RedrawInventory()
        {
            foreach (var slot in _inventorySlots)
                Destroy(slot.gameObject);
            _inventorySlots = new List<InventorySlotWidget>(_inventorySize);

            for (int i = 0; i < _inventorySize; i++)
            {
                var inventorySlot = Instantiate(_inventorySlotPrefab, transform);
                _inventorySlots.Add(inventorySlot.GetComponent<InventorySlotWidget>());
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
