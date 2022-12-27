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

        
        //virtual camera
        public CinemachineVirtualCamera virtualCamera;
        private CinemachineFramingTransposer framingTransposer;
        private float normalOffset;
        private float aimOffset;
        private bool inAim = false;
        private Vector3 damping;
        
        

        public GameObject followTarget;
        public Vector2 _cameraInput;
        public float mouseSensitivityY = .5f;
        public float mouseSensitivityX = .5f;
        private Quaternion localRotation;
        private float xRotation;
        public float yRotationDifference;
       
        void Awake()
        {
            playerMovement = GetComponent<MoveCineMachine>();
            gravityControl = GetComponent<CineGravityControl>();

            //set aim variables
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            normalOffset = framingTransposer.m_CameraDistance;
            damping = new Vector3(framingTransposer.m_XDamping, framingTransposer.m_YDamping, framingTransposer.m_ZDamping);
        }
        private void Update()
        {
            yRotationDifference = localRotation.eulerAngles.y;

        }

        void LateUpdate()
        {
            RotateCamera();
        }
       

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }
        
        
        public void AimAction(InputAction.CallbackContext context)
        {
            aimOffset = normalOffset/3;
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    framingTransposer.m_ZDamping = .5f;
                    framingTransposer.m_CameraDistance = Mathf.Lerp(normalOffset, aimOffset, Time.deltaTime);
                    inAim = false;
                    break;
                case InputActionPhase.Started:
                    framingTransposer.m_ZDamping = .5f;
                    framingTransposer.m_CameraDistance = Mathf.Lerp(aimOffset, normalOffset, Time.deltaTime);
                    inAim = true;
                    break;
                case InputActionPhase.Canceled:
                    framingTransposer.m_CameraDistance = Mathf.Lerp(normalOffset, aimOffset, Time.deltaTime);
                    framingTransposer.m_ZDamping = damping.z;

                    inAim = false;
                    break;
            }
            //framingTransposer.m_ZDamping = damping.z;
           
        }
        
        
        
        private void RotateCamera()
        {
            //left/right Rotation + player Rotation
            float rotateY = _cameraInput.x * mouseSensitivityX; //x input rotates around y axis

            //if player is moving he is controlled by same movement as camera. otherwise camera can move around him without affecting him
            if (playerMovement._movementInput.y != 0 || playerMovement._movementInput.x != 0 || inAim)
            {
                AlignPlayer();
                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates player and camera around y axis (or player up)
            }
            else
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateY, .5f), Vector3.up); //rotates camera around y axis
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
            else if(xRotation > 50f && xRotation < 180f && rotateX <= 0 ) //only allow negative rotation if camera rotation > 50
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateX, .5f), Vector3.right); 
            }
            else if(xRotation < 345f && xRotation > 180f && rotateX >= 0) //only allow positive rotation if camera rotation < 345 (-15)
            {
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, rotateX, .5f), Vector3.right); 
            }

            //Z Rotation aligns camera keep player from rotating in view
            float zRotationDifference = localRotation.eulerAngles.z; //finds difference between camera and player rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(-zRotationDifference, Vector3.forward); //adds amount of rotation needed to align camera up and player up


        }
        private void AlignPlayer()
        {
            yRotationDifference = localRotation.eulerAngles.y;
            if(yRotationDifference >= 180)
            {
                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, -yRotationDifference, 2f * Time.fixedDeltaTime), Vector3.up); // aligns player to camera forward
                yRotationDifference = localRotation.eulerAngles.y;
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, yRotationDifference, 2f * Time.fixedDeltaTime), Vector3.up); //realigns camera to player forward
            }
            else
            {
                transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, yRotationDifference, 12f * Time.fixedDeltaTime), Vector3.up); // aligns player to camera forward
                yRotationDifference = localRotation.eulerAngles.y;
                followTarget.transform.rotation *= Quaternion.AngleAxis(Mathf.Lerp(0, -yRotationDifference, 12f * Time.fixedDeltaTime), Vector3.up); //realigns camera to player forward
            }
        }

    }
}
