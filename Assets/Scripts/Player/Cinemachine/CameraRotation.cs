using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gravity;
using Cinemachine;

namespace Player
{
    /** if you need help on this script let me know
     * there is a lot of confusing math in it on Quaternions and Euler Angles
     * **/
    public class CameraRotation : MonoBehaviour
    {
        private MoveCineMachine playerMovement;
        private CineGravityControl gravityControl;

        /**
        //virtual camera
        public CinemachineVirtualCamera virtualCamera;
        private CinemachineOrbitalTransposer orbitalTransposer;
        private Vector3 normalOffset;
        private Vector3 aimOffset;
        private bool inAim = false;
        **/
        

        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float mouseSensitivityY = .5f;
        public float mouseSensitivityX = .5f;
        private Quaternion localRotation;
        private float xRotation;
       
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
            gravityControl = GetComponent<CineGravityControl>();
            //orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            //normalOffset = orbitalTransposer.m_FollowOffset;
        }
    
        void LateUpdate()
        {
            RotateCamera();
        }
       

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }
        
        /**
        public void AimAction(InputAction.CallbackContext context)
        {
            //switches camera offest to aim position
            if (!inAim)
            {
                aimOffset = new Vector3(normalOffset.x, normalOffset.y, normalOffset.z + 5);
                orbitalTransposer.m_FollowOffset = aimOffset;
                inAim = true;
            }
            //switches camera offset back to normal
            else
            {
                orbitalTransposer.m_FollowOffset = normalOffset;
                inAim = false;
            }
        }
        **/
        
        
        private void RotateCamera()
        {
            //left/right Rotation + player Rotation
            float rotateY = _cameraInput.x * mouseSensitivityX; //x input rotates around y axis

            //if player is moving he is controlled by same movement as camera. otherwise camera can move around him without affecting him
            if (playerMovement._movementInput.y != 0 || playerMovement._movementInput.x != 0)
            {
                AlignPlayer();
                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates player and camera around y axis (or player up)
            }
            else
            {
                //followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates camera around y axis
            }



            //Look up/down Rotation
            float rotateX = _cameraInput.y * mouseSensitivityY; //y input rotates around x axis

            localRotation = followTarget.transform.localRotation;
            xRotation = localRotation.eulerAngles.x;
            

            //clamp vertical rotation between 345 (-15) degrees and 50 degrees
            if (xRotation <= 50f || xRotation >= 345)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateX, .5f), Vector3.right); //rotates camera around x axis (or player left/right)
            }
            else if(xRotation > 50f && xRotation < 180f) //add negative rotation if camera rotation > 50
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(-0.25f * mouseSensitivityY, Vector3.right);
            }
            else if(xRotation < 345f && xRotation > 180f) //add positive rotation if camera rotation < 345 (-15)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(0.25f * mouseSensitivityY, Vector3.right);
            }

            //Z Rotation aligns camera keep player from rotating in view
            float zRotationDifference = localRotation.eulerAngles.z; //finds difference between camera and player rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(-zRotationDifference, Vector3.forward); //adds amount of rotation needed to align camera up and player up


        }
        private void AlignPlayer()
        {
            float yRotationDifference = localRotation.eulerAngles.y;
            transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, yRotationDifference, 1f), Vector3.up); // aligns player to camera forward
            yRotationDifference = localRotation.eulerAngles.y;
            followTarget.transform.rotation *= Quaternion.AngleAxis(-yRotationDifference, Vector3.up); //realigns camera to player forward
        }

    }
}
