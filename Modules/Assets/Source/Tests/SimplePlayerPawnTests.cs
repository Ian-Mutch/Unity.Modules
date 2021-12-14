using NUnit.Framework;
using UnityEngine;

namespace Modules.Tests
{
    public class SimplePlayerPawnTests
    {
        private GameObject _pawnObj;
        private SimplePlayerPawn _pawn;

        [SetUp]
        public void Setup()
        {
            _pawnObj = new GameObject("SimplePlayerPawn");
            _pawn = _pawnObj.AddComponent<SimplePlayerPawn>();
            var cameraObj = new GameObject("Camera", typeof(Camera));
            cameraObj.tag = "MainCamera";
            cameraObj.transform.SetParent(_pawnObj.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.5f, 0);
        }

        [TearDown]
        public void TearDown()
        {
            if (_pawnObj != null)
                Object.DestroyImmediate(_pawnObj);
        }

        [Test]
        public void Move_Success()
        {
            // Arrange
            var movement = new Vector3(.45f, 0, .73f);

            // Act
            _pawn.Move(movement);

            //Assert
            Assert.AreEqual(movement, _pawn._movement);
        }

        [Test]
        public void ConsumeMovement_Success()
        {
            // Arrange
            var movement = new Vector3(.45f, 0, .73f);
            _pawn.Move(movement);

            // Act
            var result = _pawn.ConsumeMovement();

            //Assert
            Assert.AreEqual(result, movement);
            Assert.AreEqual(Vector3.zero, _pawn._movement);
        }

        [Test]
        public void Move_Internal_Success()
        {
            // Arrange
            var movement = new Vector3(.45f, 0, .73f);
            var deltaTime = 0.02f;
            var movementSpeed = 2f;
            var position = _pawn.transform.position;

            // Act
            _pawn.Move_Internal(movement, deltaTime, movementSpeed);

            //Assert
            var expectedXDelta = movement.x * deltaTime * movementSpeed;
            Assert.IsTrue(Mathf.Approximately(position.x, _pawn.transform.position.x - expectedXDelta));

            var expectedZDelta = movement.z * deltaTime * movementSpeed;
            Assert.IsTrue(Mathf.Approximately(position.z, _pawn.transform.position.z - expectedZDelta));
        }

        [Test]
        public void Rotate_Success()
        {
            // Arrange
            var roll = .45f;
            var yaw = .73f;

            // Act
            _pawn.Rotate(roll, yaw);

            //Assert
            Assert.AreEqual(_pawn._roll, roll);
            Assert.AreEqual(_pawn._yaw, yaw);
        }

        [Test]
        public void ConsumeRotation_Success()
        {
            // Arrange
            var roll = .45f;
            var yaw = .73f;
            _pawn.Rotate(roll, yaw);

            // Act
            _pawn.ConsumeRotation(out var r, out var y);

            //Assert
            Assert.AreEqual(roll, r);
            Assert.AreEqual(yaw, y);
        }

        [Test]
        public void Rotate_Internal_Success()
        {
            // TODO: Revisit this test, potentially better way to do this

            // Arrange
            var allowedError = 1f;
            var roll = .45f;
            var yaw = .73f;
            var rotationSpeed = 2f;

            var cameraRotation = Camera.main.transform.localRotation;
            var pawnRotation = _pawn.transform.rotation;

            // Act
            _pawn.Rotate_Internal(roll, yaw, rotationSpeed);

            //Assert
            var cameraAngle = Quaternion.Angle(cameraRotation, Camera.main.transform.localRotation);
            Assert.IsTrue(Mathf.Abs(cameraAngle - yaw * rotationSpeed) < allowedError);

            var pawnAngle = Quaternion.Angle(pawnRotation, _pawn.transform.rotation);
            Assert.IsTrue(Mathf.Abs(pawnAngle - yaw * rotationSpeed) < allowedError);
        }
    }
}