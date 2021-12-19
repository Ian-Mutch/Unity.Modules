using UnityEngine;

namespace Modules
{
    public class InventoryWidget : MonoBehaviour
    {
        private static InventoryWidget _instance;

        public static void Show()
        {
            _instance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _instance.gameObject.SetActive(false);
        }

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.LogWarning("", this);
                Destroy(gameObject);
            }

            _instance = this;
        }
    }
}
