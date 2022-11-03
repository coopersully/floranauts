using UnityEngine;
using UnityEngine.InputSystem;

namespace Gravity
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 movementInput;
        public Vector2 cameraInput;

        private GravityAttractor _gravityAttractor;
        private Animator _anim;
        
        public float mouseSensitivityX = 1;
        public float mouseSensitivityY = 1;
        public float walkSpeed = 10;
        public float jumpForce = 500;
        public LayerMask groundMask;
        public Transform groundCheck;

        public bool isGrounded;
        private Vector3 _moveAmount;
        private Vector3 _smoothMoveVelocity;
        private float _verticalLookRotation;
        private Transform _cameraTransform;
        private Rigidbody _rigidbody;
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        private void Awake()
        {
            _anim = GetComponent<Animator>();

            _rigidbody = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateGroundedValue();
            ApplyMovement();
        }

        /* Check for if the player is on the ground
         and apply the appropriate animation effect(s). */
        private void UpdateGroundedValue()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
            _anim.SetBool(IsGrounded, isGrounded);
        }

        
        private void FixedUpdate()
        {
            // Apply movement to rigidbody
            var localMove = transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + localMove);
        }

        public void Camera(InputAction.CallbackContext context)
        {
            Debug.Log("Camera moved.");
            cameraInput = context.ReadValue<Vector2>();
        }

        private void ApplyMovement()
        {
            // Rotate player based on where mouse or right joystick dictates
            transform.Rotate(Vector3.up * (cameraInput.x * mouseSensitivityX));
            _verticalLookRotation += cameraInput.y * mouseSensitivityY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -45, -25);
            _cameraTransform.localEulerAngles = Vector3.left * _verticalLookRotation;
            _anim.SetFloat(Horizontal, movementInput.x);
            _anim.SetFloat(Vertical, movementInput.y);

            // Move player based on Input System
            var moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
            var targetMoveAmount = moveDirection * walkSpeed;
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);
        }
        
        public void Move(InputAction.CallbackContext context)
        {
            Debug.Log("Player moved.");
            movementInput = context.ReadValue<Vector2>();
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
            Debug.Log("Player jumped.");
            // If the player is not grounded, ignore the jump event.
            if (!isGrounded) return;
            
            _anim.SetTrigger(Jump1);
            _rigidbody.AddForce(transform.up * jumpForce);
        }
    }
}
