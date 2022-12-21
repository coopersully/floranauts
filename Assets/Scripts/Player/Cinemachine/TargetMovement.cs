using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class TargetMovement : MonoBehaviour
    {
        public GameObject player;
        private MoveCineMachine playerMovement;
        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float rotationPower = .5f;
        private float yInput;
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
            yInput = _cameraInput.y;
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
            transform.rotation *= Quaternion.AngleAxis(_cameraInput.x * rotationPower, Vector3.up); //rotates player around y axis (or player up)

            float xRotation = followTarget.transform.eulerAngles.x;
            
            //Vertical Rotation
            if (xRotation > -30 && xRotation < 60)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(_cameraInput.y * rotationPower, Vector3.right); //rotates camera around x axis (or player left/right)
            }
            //needs work for smoother transition
            else if (xRotation <= -30)
            {
                followTarget.transform.Rotate(Mathf.Lerp(xRotation, (-30 - xRotation), .25f), 0, 0);
            }
            else if (xRotation >= 60)
            {
                followTarget.transform.Rotate(Mathf.Lerp(xRotation,(xRotation - 60), 1f), 0, 0);
            }

            //Z Rotation
            var playerZAngle = transform.localEulerAngles.z;
            //followTarget.transform.rotation = Quaternion.Euler(followTarget.transform.eulerAngles.x, followTarget.transform.eulerAngles.y, transform.eulerAngles.z);


        }
    }
}
