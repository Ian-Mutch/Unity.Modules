using UnityEngine;

namespace Modules
{
    public class SaveData
    {
        public string Scene { get; set; }
        public Vector3 PlayerPawnPosition { get; set; }
        public Vector3 PlayerCameraRotation { get; set; }
        public Vector3 PlayerPawnRotation { get; set; }
        public Inventory.Inventory Inventory { get; set; }
    }
}
