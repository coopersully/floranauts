using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gravity;

namespace Player
{
    public class TargetMovement : MonoBehaviour
    {
        private MoveCineMachine playerMovement;
        private GravityAttractor gravityAttractor;
        private CineGravityControl gravityControl;

        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float rotationSensitivity = .5f;
        private float yInput;
        private new Rigidbody rigidbody;
        private Quaternion localRotation;
        private float xRotation;
        private Quaternion currentRotation;
        private Quaternion desiredRotation;
       
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
            rigidbody = GetComponentInParent<Rigidbody>();
            gravityControl = GetComponent<CineGravityControl>();
            yInput = _cameraInput.y;
            desiredRotation = followTarget.transform.rotation;
        }
        private void Update()
        {
            //gravityControl._planet.RotateFollowTarget(followTarget);
            PlayerRotation();

        }
        void FixedUpdate()
        {
            
            
        }

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }
        private void PlayerRotation()
        {
            
            //left/right Rotation + player Rotation
            var rotateY = _cameraInput.x * rotationSensitivity;
            if (playerMovement._movementInput.y > 0 || playerMovement._movementInput.x > 0)
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, 1f);
                transform.rotation *= Quaternion.AngleAxis(rotateY, Vector3.up); //rotates player with camera around y axis (or player up)
            }
            else
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(rotateY, Vector3.up); //rotates camera around y axis
                //transform.rotation *= Quaternion.AngleAxis(rotateY, Vector3.up); //rotates player with camera around y axis (or player up)

            }



            //Look up/down Rotation
            var rotateX = _cameraInput.y * rotationSensitivity;

            currentRotation = followTarget.transform.rotation;
            localRotation = followTarget.transform.localRotation;
            xRotation = localRotation.eulerAngles.x;

            if (xRotation <= 50f || xRotation >= 345) //clamps vertical rotation
            {
                
                followTarget.transform.rotation *= Quaternion.AngleAxis(rotateX, Vector3.right); //rotates camera around x axis (or player left/right)
                desiredRotation = currentRotation;

            }
            
            else //positions vertical rotation to last value within range
            {
                Debug.Log("greaterTthan");
                followTarget.transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, .25f);
                currentRotation = followTarget.transform.rotation;

            }

            //Z Rotation
            
           







        }
    }
}
