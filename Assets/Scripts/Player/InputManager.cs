using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls _playerControls;

        public Vector2 movementInput;
        private float _moveAmount;
   
        public Vector2 cameraInput;
        public float cameraInputX;
        public float cameraInputY;

        public float verticalInput;
        public float horizontalInput;

        public bool jumpInput;

        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();

                /* References different control in the Input Action "PlayerControls".
                 This will work for both mouse/keyboard and game controllers. */
                _playerControls.PlayerMovement.HorizontalMovement.performed += i => movementInput = i.ReadValue<Vector2>();
                _playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                _playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
                _playerControls.PlayerMovement.JetPack.performed += i => jumpInput = true;

            }

            _playerControls.Enable();
        }
        
        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void Update()
        {
            MovementInput();
        }

        /* Calculates inputs for movement and camera
         which changes based on character rotation. */
        private void MovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            // animatorManager.UpdateAnimatorValues(0, moveAmount);

            cameraInputY = cameraInput.y;
            cameraInputX = cameraInput.x;
        }

    }
}
