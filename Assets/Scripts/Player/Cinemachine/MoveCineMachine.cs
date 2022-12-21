using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;
using UnityEngine.InputSystem;

namespace Player
{

    public class MoveCineMachine : MonoBehaviour
    {
        private GravityControl _gravityControl;
        private Animator _animator;
        private Rigidbody _rigidbody;

        public float mouseSensitivityX = 1f;
        public float mouseSensitivityY = 1f;

        private Vector3 _moveAmount;
        private Vector3 _smoothMoveVelocity;
        private Vector3 _moveDirection;

        private float _verticalLookRotation;
        private Transform _cameraTransform;

        public Vector2 _movementInput;
        public Vector2 _cameraInput;

        private float _walkSpeed = 15f;
        private const float JumpForce = 1200f;

        public LayerMask groundMask;
        public Transform groundCheck;
        [HideInInspector]
        public bool isGrounded;
        public float playerGravity = -10f;

        private bool inKnockBack = false;
        public bool isSprinting = false;


        // Player-related animation triggers
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Attack = Animator.StringToHash("SwingAttack");
        private static readonly int Shoot = Animator.StringToHash("ShootPistol");
        private static readonly int Drink = Animator.StringToHash("Drink");
        void Start()
        {
            // Initialize components
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
            //_cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGroundedValue();
            ApplyMovement();
        }
        private void FixedUpdate()
        {
            // Apply movement to rigidbody
            var localMove = transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + localMove);
        }

        private void UpdateGroundedValue()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
            _animator.SetBool(IsGrounded, isGrounded);
        }
        
        private void ApplyMovement()
        {

            // Rotate player based on where mouse or right joystick dictates
            //transform.Rotate(Vector3.up * _cameraInput.x * mouseSensitivityX);
            // _verticalLookRotation += _cameraInput.y * mouseSensitivityY;
            // _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -30, -15);
            // _cameraTransform.localEulerAngles = Vector3.left * _verticalLookRotation;


            if (!inKnockBack)
            {
                // If player is not in knockBack, Move player based on Input System
                _moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y).normalized;
                _animator.SetFloat(Horizontal, _movementInput.x);
                _animator.SetFloat(Vertical, _movementInput.y);
            }

            var targetMoveAmount = _moveDirection * _walkSpeed;

            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);




            // Sets player gravity which accelerates over time if
            // not grounded (accessed by GravityAttractor script)
            if (isGrounded)
                playerGravity = -10;
            else
                playerGravity -= 50 * Time.deltaTime;
            //Debug.Log("Player Gravity: " + playerGravity);
        }

        public void MoveAction(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }
        public void JumpAction(InputAction.CallbackContext context)
        {
            // If the player is not grounded or is in Knock Back sequence, ignore the jump event.
            if (!isGrounded || inKnockBack) return;

            _animator.SetTrigger(Jump);
            //jumpParticles.Play();

            // Applies upward force and directional force depending on direction player is moving
            _rigidbody.AddForce(transform.up * JumpForce);
            _rigidbody.AddForce(transform.forward * JumpForce * _movementInput);
        }

    }
}
