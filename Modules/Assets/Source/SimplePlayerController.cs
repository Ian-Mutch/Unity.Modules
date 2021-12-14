using UnityEngine;

namespace Modules
{
    public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField]
        private SimplePlayerPawn _pawn;

        private void Update()
        {
            if (!_pawn)
                return;

            var roll = Input.GetAxis("Mouse X");
            var yaw = -Input.GetAxis("Mouse Y");
            _pawn.Rotate(roll, yaw);

            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _pawn.Move(movement);
        }
    }
}