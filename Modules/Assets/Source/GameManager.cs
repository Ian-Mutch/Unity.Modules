using Modules.Inventory;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules
{
    public class GameManager : MonoBehaviour
    {
        public static event Action OnBeforeSave;
        public static event Action<bool> OnAfterSave;

        private static GameManager _instance;
        private static readonly ISaveDataUtility _saveDataUtility = new BinarySaveDataUtility();
        [SerializeField]
        private GameObject _playerControllerPrefab;
        private SimplePlayerController _playerController;
        [SerializeField]
        private GameObject _playerPawnPrefab;
        [SerializeField]
        private Vector3 _startingPawnPosition;
        [SerializeField]
        private Vector3 _startingPawnRotationEuler;
        [SerializeField]
        private Vector3 _startingCameraRotationEuler;
        [SerializeField]
        private string _startingScene;
        private static CancellationTokenSource _cancellationTokenSource;

        public static async Task<bool> SaveDataAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            OnBeforeSave?.Invoke();

            var saveData = GetDataToSave();
            var result = await _saveDataUtility.WriteAsync(saveData, cancellationToken: cancellationToken);

            OnAfterSave?.Invoke(result.Success);

            return result.Success;
        }

        public static async void NewGameAsync()
        {
            await LoadSceneAsync(_instance._startingScene);

            InstantiatePlayerControllerWithPawn();
        }

        public static async void LoadGameAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await LoadGameAsync(cancellationToken: _cancellationTokenSource.Token);

            _cancellationTokenSource.Dispose();
        }

        public static async Task LoadGameAsync(CancellationToken cancellationToken = default)
        {
            var result = await _saveDataUtility.ReadAsync(cancellationToken: cancellationToken);

            if (!result.Success)
                return;

            var saveData = result.Data;
            await LoadSceneAsync(saveData.Scene);

            InstantiatePlayerControllerWithPawn();

            InventoryManager.SetInventory(saveData.Inventory);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quite();
#endif
        }

        private static void InstantiatePlayerControllerWithPawn()
        {
            _instance._playerController = FindObjectOfType<SimplePlayerController>();
            if (_instance._playerController == null)
                _instance._playerController = CreatePlayerController();

            var playerPawn = FindObjectOfType<SimplePlayerPawn>();
            if (playerPawn == null)
                playerPawn = CreatePlayerPawn();

            _instance._playerController.SetPawn(playerPawn);
        }

        private static SimplePlayerController CreatePlayerController()
        {
            var playerControllerObj = Instantiate(_instance._playerControllerPrefab);
            return playerControllerObj.GetComponent<SimplePlayerController>();
        }

        private static SimplePlayerPawn CreatePlayerPawn()
        {
            var playerPawnObj = Instantiate(_instance._playerPawnPrefab, _instance._startingPawnPosition, Quaternion.Euler(_instance._startingPawnRotationEuler));
            Camera.main.transform.rotation = Quaternion.Euler(_instance._startingCameraRotationEuler);
            return playerPawnObj.GetComponent<SimplePlayerPawn>();
        }

        private static SaveData GetDataToSave()
        {
            var scene = SceneManager.GetActiveScene().name;
            var playerPawn = _instance._playerController.GetPawn();
            var position = playerPawn.transform.position;
            var cameraRotation = Camera.main.transform.eulerAngles;
            var pawnRotation = playerPawn.transform.eulerAngles;
            var inventory = InventoryManager.GetInventory();

            return new SaveData()
            {
                Scene = scene,
                PlayerPawnPosition = position,
                PlayerCameraRotation = cameraRotation,
                PlayerPawnRotation = pawnRotation,
                Inventory = inventory
            };
        }

        private static async Task LoadSceneAsync(string name, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var operation = SceneManager.LoadSceneAsync(name);
            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f)
                if (cancellationToken.IsCancellationRequested)
                    return;

            operation.allowSceneActivation = true;

            await Task.CompletedTask;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"A second instance of {nameof(GameManager)} exists in the scene, destroying...", this);
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private static void EnsureInstanceExists()
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
                throw new InventoryException($"No instance of {nameof(InventoryManager)} exists in the scene");
        }
    }
}
