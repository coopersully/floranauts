using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;
using UnityEngine.InputSystem;

namespace Player
{

    public class MoveCineMachine : MonoBehaviour
    {
        private CineGravityControl _gravityControl;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;

        public float mouseSensitivityX = 1f;
        public float mouseSensitivityY = 1f;

        private Vector3 _moveAmount;
        private Vector3 _smoothMoveVelocity;
        [HideInInspector]
        public Vector3 _moveDirection;

        private float _verticalLookRotation;
        private Transform _cameraTransform;

        public Vector2 _movementInput;
        public Vector2 _cameraInput;

        [HideInInspector]
        public float _walkSpeed = 15f;
        [HideInInspector]
        public float JumpForce = 1200f;

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
            _capsuleCollider = GetComponent<CapsuleCollider>();
            //_cameraTransform = GetComponentInChildren<Camera>().transform;
            _smoothMoveVelocity = new Vector3(0.25f, 0.25f, 0.25f);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGroundedValue();
            ApplyMovement();

            Shader.SetGlobalVector("_PlayerPosition", groundCheck.transform.position + Vector3.up * _capsuleCollider.radius);
            //if (!isGrounded) Debug.Log("not Grounded");
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
        public void UseItemAction(InputAction.CallbackContext context)
        {
            Debug.Log("Use Item");
        }

    }
}
