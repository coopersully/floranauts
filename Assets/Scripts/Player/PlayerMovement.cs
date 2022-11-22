using Gravity;
using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private GravityAttractor _gravityAttractor;
        private Animator _animator;
        private Rigidbody _rigidbody;

        [Header("Basics")]
        public float mouseSensitivityX = 1f;
        public float mouseSensitivityY = 1f;
        
        private Vector3 _moveAmount;
        private Vector3 _smoothMoveVelocity;
        private Vector3 _moveDirection;
        
        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private float _verticalLookRotation;
        private Transform _cameraTransform;

        private float _walkSpeed = 13f;
        private const float JumpForce = 1200f;

        [Header("Grounding")]
        public LayerMask groundMask;
        public Transform groundCheck;
        public bool isGrounded;
        public float playerGravity = -10f;

        [Header("Items")]
        public bool hasJetpack = false;
        public GameObject jetPack;
        public bool hasStick = false;
        private bool _canSwingStick = true;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        public bool hasFreezeRay = false;
        public GameObject freezeRay;
        public bool hasRocketLauncher = false;
        public bool hasSpeedIncrease = false;
        private bool _canSprint = true;

        // Knockback-related 
        private bool _inKnockBack = false;
        private const float KnockBackForce = 10f;
        private const float KnockBackTime = .75f;
        private float _knockBackCounter;
        
        [Header("Particles")]
        public ParticleSystem landParticles;
        public ParticleSystem jumpParticles;
        public ParticleSystem walkParticles;
        public ParticleSystem jetParticles;
        
        // Player-related animation triggers
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Attack = Animator.StringToHash("SwingAttack");


        private void Awake()
        {
            // Initialize components
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;

            // Hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Object Defaults
            //hasJetpack = true;
            jetParticles.Stop();
            //hasStick = true;
            //hasSpeedIncrease = true;
            //hasFreezeRay = true;
            //hasRocketLauncher = true;
        }

        private void Update()
        {
            stickObj.SetActive(hasStick); // Shows physical stick if item is activated
            jetPack.SetActive(hasJetpack);
            freezeRay.SetActive(hasFreezeRay);
            
            if (PauseManager.Instance.isPaused) return;

            // Checks if player is in knockback sequence, sets Bool, and counts down if inKnockBack
            if (_knockBackCounter > 0)
            {
                _inKnockBack = true;
                _knockBackCounter -= Time.deltaTime * 2;
            }
            else _inKnockBack = false;

            UpdateGroundedValue();
            ApplyMovement();
        }

        /* Check for if the player is on the ground
         and apply the appropriate animation effect(s). */
        private void UpdateGroundedValue()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
            _animator.SetBool(IsGrounded, isGrounded);
            //Debug.Log("isGrounded" + isGrounded);
        }

        private void FixedUpdate()
        {
            // Apply movement to rigidbody
            var localMove = transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + localMove);
        }

        public void CameraAction(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
        }

        private void ApplyMovement()
        {
            // Rotate player based on where mouse or right joystick dictates
            transform.Rotate(Vector3.up * (_cameraInput.x * mouseSensitivityX));
            _verticalLookRotation += _cameraInput.y * mouseSensitivityY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -45, -25);
            _cameraTransform.localEulerAngles = Vector3.left * _verticalLookRotation;
            

            if (!_inKnockBack)
            {
                // If player is not in knockBack, Move player based on Input System
                _moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y).normalized;
                _animator.SetFloat(Horizontal, _movementInput.x);
                _animator.SetFloat(Vertical, _movementInput.y);
            }
           
            var targetMoveAmount = _moveDirection * _walkSpeed;

            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);

            // Plays walking particles if player is on a planet and moving
            if (_movementInput.magnitude > 0.3 && isGrounded) walkParticles.Play();
            else walkParticles.Stop();
            
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
            if (!isGrounded || _inKnockBack) return;
            
            _animator.SetTrigger(Jump); 
            jumpParticles.Play();

            // Applies upward force and directional force depending on direction player is moving
            _rigidbody.AddForce(transform.up * JumpForce);
            _rigidbody.AddForce(transform.forward * JumpForce * _movementInput);
        }

        public void UseItemAction(InputAction.CallbackContext context)
        {
            if (hasJetpack && context.started)
                JetPack();
            else if (hasStick && context.started && _canSwingStick)  // If the key was not pressed this frame, ignore it.
                SwingAttack();
            else if (hasSpeedIncrease && _canSprint)
                StartCoroutine(Sprint());
        }

        private void JetPack()
        {
            // Apply force while jetpack input is activated
            if (!hasJetpack || _inKnockBack) return;

            jetParticles.Play(); // Needs edits based on hold

            _animator.SetBool(IsGrounded, isGrounded);
            _rigidbody.AddForce(transform.up * JumpForce );
            _rigidbody.AddForce(transform.forward * JumpForce * _movementInput * 5f);


            _animator.SetTrigger(Falling); // Transitions walking animation to falling without having to go through Jump
            
            Debug.Log("jetpack");
        }

        private void SwingAttack()
        {
            // If the player is currently in knockback, ignore this event. 
            if ( _inKnockBack ) return;
            
            StartCoroutine(SwingAnimation());
        }
        
        private IEnumerator SwingAnimation()
        {
            // Player cannot swing stick again until animation plays through
            _canSwingStick = false;
            _animator.SetTrigger(Attack);
            yield return new WaitForSeconds(.75f);

            // Activates stick and deactivates after the animation plays out
            stickObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            stickObj.SetActive(false);
            _canSwingStick = true;
        }
        
        private IEnumerator Sprint()
        {
            // Doubles player speed for short period, then has cooldown period before can be used again
            _canSprint = false;
            _walkSpeed *= 2;
            yield return new WaitForSeconds(10f);
            _walkSpeed /= 2;
            yield return new WaitForSeconds(20f);
            _canSprint = true;
            Debug.Log("can Sprint");
        }

        private void ApplyKnockBack(Vector3 direction)
        {
            // Takes in Vector3 direction value, applies force
            _knockBackCounter = KnockBackTime;
            _moveDirection = direction * KnockBackForce;
            _moveDirection.y = 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks Trigger, and starts knockback sequence.
            if (other.gameObject.CompareTag("KnockBack"))
            {
                Vector3 hitDirection = other.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                ApplyKnockBack(hitDirection);
                _animator.SetTrigger(Falling);
            }

            // Landing particles
            if (other.gameObject.CompareTag("InnerGravity")) landParticles.Play();
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddForce(force, forceMode);
        }
    }
}
