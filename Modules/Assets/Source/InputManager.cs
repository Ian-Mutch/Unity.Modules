using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Modules
{
    public static class InputManager
    {
        private static readonly Dictionary<int, ActionListenerCollection> _listeners = new Dictionary<int, ActionListenerCollection>();

        public static void AddListener(int playerIndex, string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (playerIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(playerIndex), $"{playerIndex} cannot be less than zero");

            if (actionName == null)
                throw new ArgumentNullException(nameof(actionName));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var playerInput = PlayerInput.GetPlayerByIndex(playerIndex);
            if (playerInput == null)
                throw new ArgumentOutOfRangeException(nameof(playerIndex), $"No player input exists with the player index {playerIndex}");

            var action = playerInput.actions[actionName];
            if (action == null)
                throw new InvalidOperationException($"The action {actionName} does not exist on player input {playerIndex}");

            if (!_listeners.TryGetValue(playerIndex, out var actionListeners))
            {
                actionListeners = new ActionListenerCollection();
                _listeners.Add(playerIndex, actionListeners);

                action.performed += e =>
                {
                    var actionListeners = _listeners[playerInput.playerIndex];
                    actionListeners.Invoke(e.action.name, e);
                };
            }

            actionListeners.AddListener(actionName, callback);
        }

        public static void RemoveListener(int playerIndex, string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (!_listeners.TryGetValue(playerIndex, out var actionListeners))
                return;

            actionListeners.RemoveListener(actionName, callback);
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


        private class Listener
        {
            private Action<InputAction.CallbackContext> _listeners;

            public void Invoke(InputAction.CallbackContext e)
            {
                _listeners?.Invoke(e);
            }

            public void Add(Action<InputAction.CallbackContext> callback)
            {
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));

                _listeners += callback;
            }

            public void Remove(Action<InputAction.CallbackContext> callback)
            {
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));

                _listeners -= callback;
            }
        }

        private class ActionListenerCollection
        {
            private readonly Dictionary<string, Listener> _listeners = new Dictionary<string, Listener>();

            public void Invoke(string actionName, InputAction.CallbackContext e)
            {
                if (actionName == null)
                    throw new ArgumentNullException(nameof(actionName));

                if (_listeners.TryGetValue(actionName, out Listener listener))
                    listener.Invoke(e);
            }

            public void AddListener(string actionName, Action<InputAction.CallbackContext> callback)
            {
                if (actionName == null)
                    throw new ArgumentNullException(nameof(actionName));

                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));

                if (!_listeners.TryGetValue(actionName, out Listener listener))
                {
                    listener = new Listener();
                    _listeners.Add(actionName, listener);
                }

                listener.Add(callback);
            }

            public void RemoveListener(string actionName, Action<InputAction.CallbackContext> callback)
            {
                if (actionName == null)
                    throw new ArgumentNullException(nameof(actionName));

                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));

                if (_listeners.TryGetValue(actionName, out Listener listener))
                    listener.Remove(callback);
            }
        }

    }
}
