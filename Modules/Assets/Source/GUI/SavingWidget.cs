using UnityEngine;

namespace Modules
{
    public class SavingWidget : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.OnBeforeSave += OnBeforeGameSaved;
            GameManager.OnAfterSave += OnAfterGameSaved;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeSave += OnBeforeGameSaved;
            GameManager.OnAfterSave += OnAfterGameSaved;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnAfterGameSaved(bool obj)
        {
            gameObject.SetActive(false);
        }

        private void OnBeforeGameSaved()
        {
            gameObject.SetActive(true);
        }
    }
}
