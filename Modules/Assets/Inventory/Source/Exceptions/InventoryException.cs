using System;

namespace Modules.Inventory
{
    public class InventoryException : Exception
    {
        public InventoryException() { }
        public InventoryException(string message) : base(message) { }
        public InventoryException(string message, Exception innerException) { }
    }
}
