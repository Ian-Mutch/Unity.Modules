using UnityEngine;

namespace Modules.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        private int _capacity;
        private Inventory _inventory;

        private static InventoryManager _instance;

        public static Inventory GetInventory()
        {
            EnsureInstanceExists();

            return _instance._inventory;
        }

        public static void SetInventory(Inventory inventory)
        {
            if(inventory == null)
                throw new System.ArgumentNullException(nameof(inventory));

            EnsureInstanceExists();

            _instance._inventory = inventory;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"A second instance of {nameof(InventoryManager)} exists in the scene, destroying...", this);
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            _inventory = new Inventory(_capacity);
        }

        private static void EnsureInstanceExists()
        {
            if (_instance == null)
                _instance = FindObjectOfType<InventoryManager>();

            if (_instance == null)
                throw new InventoryException($"No instance of {nameof(InventoryManager)} exists in the scene");
        }
    }
}
