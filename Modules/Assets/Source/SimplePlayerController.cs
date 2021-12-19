using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules
{
    public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField]
        private SimplePlayerPawn _pawn;
        private bool _canMove = true;

        public SimplePlayerPawn GetPawn() => _pawn;

        public void SetPawn(SimplePlayerPawn pawn) => _pawn = pawn;

        private void OnEnable()
        {
            InputManager.AddListener(0, "Start", OnStartPerformed);
        }

        private void OnStartPerformed(InputAction.CallbackContext obj)
        {
            _canMove = !_canMove;
        }

        private void Update()
        {
            if (!_canMove)
                return;

            var rotationInput = InputManager.ReadValue<Vector2>(0, "Look");
            _pawn.Rotate(rotationInput.x, -rotationInput.y);

            var movementInput = InputManager.ReadValue<Vector2>(0, "Move");
            _pawn.Move(new Vector3(movementInput.x, 0, movementInput.y));
        }
    }
}