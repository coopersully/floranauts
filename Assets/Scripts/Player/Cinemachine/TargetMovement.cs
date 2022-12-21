using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class TargetMovement : MonoBehaviour
    {
        private MoveCineMachine playerMovement;
        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float rotationPower = 1f;
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
        }

        void Update()
        {
            PlayerRotation();
        }

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }
        private void PlayerRotation()
        {
            //Follow Transform Rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(_cameraInput.x * rotationPower, Vector3.up);

            //Vertical Rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(_cameraInput.y * rotationPower, Vector3.right);

            
            var angles = followTarget.transform.localEulerAngles;
            angles.z = 0;

            var x_angle = followTarget.transform.localEulerAngles.x;

            //clamp up/down
            if (x_angle > 180 && x_angle < 340)
            {
                angles.x = 340;
            }
            else if (x_angle < 180 && x_angle > 40)
            {
                angles.x = 40;
            }

            if(playerMovement._movementInput.x == 0 && playerMovement._movementInput.y == 0)
            {
                //transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                //transform.rotation.eulerAngles.y = followTarget.transform.rotation.eulerAngles.y;
                //followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }
            


        }
    }
}
