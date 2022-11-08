using Gravity;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
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
        [HideInInspector] public Rigidbody rb;
        
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        public bool jetPack = true;

        

        private void Awake()
        {
              jetPack = true;

            _anim = GetComponent<Animator>();

            rb = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (PauseManager.Instance.isPaused) return;
            
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
            rb.MovePosition(rb.position + localMove);
        }

        public void Camera(InputAction.CallbackContext context)
        {
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
            movementInput = context.ReadValue<Vector2>();
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
             // If the player is not grounded, ignore the jump event.
             if (!isGrounded) return;

             _anim.SetTrigger(Jump1);
             rb.AddForce(transform.up * jumpForce);
        }

        public void JetPack(InputAction.CallbackContext context) // callbackcontext is not working for a hold function
        {
            //apply force while jetpack input is activated
            if (jetPack)
            {
                rb.AddForce(transform.up * jumpForce);
                Debug.Log("jetpack");
            }
        }

        
    }
}
