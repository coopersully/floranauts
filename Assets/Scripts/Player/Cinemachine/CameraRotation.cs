using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gravity;

namespace Player
{
    /** if you need help on this script let me know
     * there is a lot of confusing math in it on Quaternions and Euler Angles
     * **/
    public class CameraRotation : MonoBehaviour
    {
        private MoveCineMachine playerMovement;
        private CineGravityControl gravityControl;

        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float rotationSensitivity = .5f;
        private float yInput;
        private Quaternion localRotation;
        private float xRotation;
        private float xRotationDifference = 0;
       
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
            gravityControl = GetComponent<CineGravityControl>();
            yInput = _cameraInput.y;
        }
    
        void LateUpdate()
        {
            Rotation();
        }
       

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }
        private void Rotation()
        {
            //left/right Rotation + player Rotation
            float rotateY = _cameraInput.x * rotationSensitivity;

            //if player is moving he is controlled by same movement as camera. otherwise camera can move around him without affecting him
            if (playerMovement._movementInput.y != 0 || playerMovement._movementInput.x != 0)
            {
                float yRotationDifference = localRotation.eulerAngles.y;
                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, yRotationDifference, 1f), Vector3.up); // aligns player to camera forward
                yRotationDifference = localRotation.eulerAngles.y;
                followTarget.transform.rotation *= Quaternion.AngleAxis(-yRotationDifference, Vector3.up); //realigns camera to player forward

                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates player and camera around y axis (or player up)
            }
            else
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates camera around y axis
            }



            //Look up/down Rotation
            float rotateX = _cameraInput.y * rotationSensitivity;

            localRotation = followTarget.transform.localRotation;
            xRotation = localRotation.eulerAngles.x;

            //clamp vertical rotation between 345 (-15) degrees and 50 degrees
            if (xRotation <= 50f || xRotation >= 345)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateX, .5f), Vector3.right); //rotates camera around x axis (or player left/right)
            }
            else if(xRotation > 50f && xRotation < 180f) //add negative rotation if camera rotation > 50
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(-.25f * rotationSensitivity , Vector3.right);
            }
            else if(xRotation < 345f && xRotation > 180f) //add positive rotation if camera rotation < 345 (-15)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(.25f * rotationSensitivity, Vector3.right);
            }

            //Z Rotation aligns camera keep player from rotating in view
            float zRotationDifference = localRotation.eulerAngles.z; //finds difference between camera and player rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(-zRotationDifference, Vector3.forward); //adds amount of rotation needed to align camera up and player up


        }
    }
}
