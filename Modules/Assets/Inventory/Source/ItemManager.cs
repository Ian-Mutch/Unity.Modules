using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField]
        private List<ItemBase> _items = new List<ItemBase>();
        private static ItemManager _instance;

        public static ItemBase GetById(uint id) 
        {
            EnsureInstanceExists();

            var item = _instance._items.SingleOrDefault(x => x.Id == id);
            if(item is null)
                throw new InventoryException($"Item {id} not found");

            return item;
        }

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.LogWarning("A second instance of ItemManager exists in the scene, destroying...", this);
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
        }

        private static void EnsureInstanceExists()
        {
            if(_instance == null)
                _instance = Object.FindObjectOfType<ItemManager>();

            if(_instance == null)
                throw new InventoryException($"No instance of {nameof(ItemManager)} exists in the scene");
        }
    }
}
