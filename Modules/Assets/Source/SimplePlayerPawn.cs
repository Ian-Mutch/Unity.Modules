using UnityEngine;

namespace Modules
{
    [RequireComponent(typeof(CharacterController))]
    public class SimplePlayerPawn : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField]
        private float _movementSpeed = 2.0f;
        [SerializeField]
        private float _rotationSpeed = 2.0f;
        [SerializeField]
        private float _horizontalRotationLimit = 45.0f;

        internal Vector3 _movement;
        internal float _roll;
        internal float _yaw;
        private Vector3 _currentRotation;

        public void Move(Vector3 movement)
        {
            _movement = movement;
        }

        public void Rotate(float roll, float yaw)
        {
            _roll = roll;
            _yaw = yaw;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ConsumeRotation(out var roll, out var yaw);
            Rotate_Internal(roll, yaw, _rotationSpeed);

            var deltaTime = Time.deltaTime;
            var movementDelta = ConsumeMovement();
            Move_Internal(movementDelta, deltaTime, _movementSpeed);
        }

        internal void Rotate_Internal(float roll, float yaw, float rotationSpeed)
        {
            _currentRotation.y += roll * rotationSpeed;
            _currentRotation.x += yaw * rotationSpeed;
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_horizontalRotationLimit, _horizontalRotationLimit);

            Camera.main.transform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);
            transform.localRotation = Quaternion.Euler(0, _currentRotation.y, 0);
        }

        internal void ConsumeRotation(out float roll, out float yaw)
        {
            roll = _roll;
            yaw = _yaw;

            _roll = 0;
            _yaw = 0;
        }

        internal void Move_Internal(Vector3 movementDelta, float deltaTime, float movementSpeed)
        {
            var horizontalDelta = movementDelta.x;
            var verticalDelta = movementDelta.z;
            var velocity = transform.forward * verticalDelta;
            velocity += transform.right * horizontalDelta;

            _controller.Move(velocity * deltaTime * movementSpeed);
        }

        internal Vector3 ConsumeMovement()
        {
            var movement = _movement;
            _movement = Vector3.zero;
            return movement;
        }
    }
}