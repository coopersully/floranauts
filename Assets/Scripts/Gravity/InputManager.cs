using UnityEngine;

namespace Gravity
{
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        public Vector2 movementInput;
        private float moveAmount;
   
        public Vector2 cameraInput;
        public float cameraInputX;
        public float cameraInputY;


        public float verticalInput;
        public float horizontalInput;

        public bool jump_Input;

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                //references different control in the Input Action "PlayerControls". This will work for computer and controllers
                playerControls.PlayerMovement.HorizontalMovement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerMovement.Jump.performed += i => jump_Input = true;

            }

            playerControls.Enable();
        }
        private void OnDisable()
        {
            playerControls.Disable();
        }
        void Update()
        {
            MovementInput();
        
        }

        private void MovementInput() //calculates inputs for movement and camera whic hchanges based on character rotation
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            //animatorManager.UpdateAnimatorValues(0, moveAmount);

            cameraInputY = cameraInput.y;
            cameraInputX = cameraInput.x;
        }

    }
}
