using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Modules
{
    public static class InputManager
    {
        private static readonly Dictionary<int, Dictionary<string, Action<InputAction.CallbackContext>>> _listeners = new Dictionary<int, Dictionary<string, Action<InputAction.CallbackContext>>>();

        private static void OnActionTriggered(int playerIndex, InputAction.CallbackContext e)
        {
            if (!_listeners.TryGetValue(playerIndex, out var actionCallbacks))
                return;

            if (!actionCallbacks.TryGetValue(e.action.name, out var callbacks))
                return;

            callbacks?.Invoke(e);
        }

        public static void AddListener(int playerIndex, string actionName, Action<InputAction.CallbackContext> callback)
        {
            var playerInput = PlayerInput.GetPlayerByIndex(playerIndex);
            if (playerInput == null)
                throw new ArgumentOutOfRangeException(nameof(playerIndex), $"No player input exists with the player index {playerIndex}");

            var action = playerInput.actions[actionName];
            if (action == null)
                throw new InvalidOperationException($"The action {actionName} does not exist on player input {playerIndex}");

            if(!_listeners.TryGetValue(playerIndex, out var actionCallbacks))
            {
                actionCallbacks = new Dictionary<string, Action<InputAction.CallbackContext>>();
                _listeners.Add(playerIndex, actionCallbacks);

                playerInput.onActionTriggered += (e) => OnActionTriggered(playerInput.playerIndex, e);
            }
                
            if (!actionCallbacks.TryGetValue(actionName, out var callbacks))
                actionCallbacks.Add(actionName, callback);
            else
                callbacks += callback;
        }

        public static void RemoveListener(int playerIndex, string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (!_listeners.TryGetValue(playerIndex, out var actionCallbacks))
                return;

            if (actionCallbacks.TryGetValue(actionName, out var callbacks))
                callbacks -= callback;
        }

        public static void SwitchActionMap(int playerIndex, string map)
        {
            var playerInput = PlayerInput.GetPlayerByIndex(playerIndex);
            if (playerInput == null)
                throw new ArgumentOutOfRangeException(nameof(playerIndex), $"No player input exists with the player index {playerIndex}");

            playerInput.SwitchCurrentActionMap(map);
        }

        public static T ReadValue<T>(int playerIndex, string actionName)
            where T : struct
        {
            var playerInput = PlayerInput.GetPlayerByIndex(playerIndex);
            if (playerInput == null)
                throw new ArgumentOutOfRangeException(nameof(playerIndex), $"No player input exists with the player index {playerIndex}");

            var action = playerInput.actions[actionName];
            if (action == null)
                throw new InvalidOperationException($"The action {actionName} does not exist on player input {playerIndex}");

            return action.ReadValue<T>();
        }
    }
}
