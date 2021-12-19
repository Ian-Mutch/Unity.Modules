using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Modules
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _loadButton;
        private CancellationTokenSource _cancellationTokenSource;
        private static PlayerHUD _instance;

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(OnSaveClickedAsync);
            _loadButton.onClick.AddListener(OnLoadClickedAsync);

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            InputManager.AddListener(0, "Start", OnStartPerformed);
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(OnSaveClickedAsync);
            _loadButton.onClick.RemoveListener(OnLoadClickedAsync);

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            InputManager.RemoveListener(0, "Start", OnStartPerformed);
        }

        private void Awake()
        {
            LockAndHideCursor(true);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnStartPerformed(InputAction.CallbackContext obj)
        {
            if (gameObject.activeSelf)
            {
                InputManager.SwitchActionMap(0, "UI");
                LockAndHideCursor(false);
            }
            else
            {
                InputManager.SwitchActionMap(0, "Player");
                LockAndHideCursor(true);
            }
        }

        private static void LockAndHideCursor(bool lockedAndHidden)
        {
            if(lockedAndHidden)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            CancelAndDisposeCancellation();
        }

        private async void OnSaveClickedAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await GameManager.SaveDataAsync(cancellationToken: _cancellationTokenSource.Token);
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            _cancellationTokenSource.Dispose();
        }

        private async void OnLoadClickedAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await GameManager.LoadGameAsync(cancellationToken: _cancellationTokenSource.Token);
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            _cancellationTokenSource.Dispose();
        }

        private void OnApplicationQuit()
        {
            CancelAndDisposeCancellation();
        }

        private void CancelAndDisposeCancellation()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
