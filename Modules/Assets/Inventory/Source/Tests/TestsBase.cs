using NUnit.Framework;
using UnityEngine;

namespace Modules.Inventory.Tests
{
    public abstract class TestsBase
    {
        protected ItemManager ItemManager { get; private set; }

        private GameObject _itemManagerObj;

        [SetUp]
        public void Setup()
        {
            _itemManagerObj = new GameObject("InventoryManager");
            ItemManager = _itemManagerObj.AddComponent<ItemManager>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_itemManagerObj != null)
                Object.DestroyImmediate(_itemManagerObj.gameObject);
        }
    }
}
