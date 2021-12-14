using UnityEngine;

namespace Modules.Inventory
{
    public abstract class ItemBase : ScriptableObject
    {
        public uint Id => _id;
        public string Name => _name;

        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _name;
    }
}
