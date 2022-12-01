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
        [HideInInspector]
        public bool isGrounded;
        public float playerGravity = -10f;

        [Header("Items")]
        public Transform firePoint;
        public float _reloadTime = 5f;

        //jetPack
        public bool hasJetpack = false;
        public GameObject jetPack;
        //stick
        public bool hasStick = false;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        private bool _canSwingStick = true;
        //freezeRay
        public bool hasFreezeRay = false;
        public GameObject freezeRayGun;
        public GameObject freezeRayProjectile;
        public float _freezeTime = 10f;
        private bool _canShootFreezeRay = true;
        //rocketLauncher
        public bool hasRocketLauncher = false;
        public GameObject rocketLauncher;
        public GameObject rocket;
        public float _launchForce = 50f;
        private bool _canShootRocket = true;
        //speedboost
        public bool hasSpeedIncrease = false;
        public float _speedMultiplier = 2f;
        private bool _canSprint = true;
        private bool _isSprinting = false;


        // Knockback-related 
        [HideInInspector]
        public bool _inKnockBack = false;
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

            _inKnockBack = false;
            jetParticles.Stop();

        }

        private void Update()
        {
            //activates physical items based on bools
            stickObj.SetActive(hasStick);
            jetPack.SetActive(hasJetpack);
            freezeRayGun.SetActive(hasFreezeRay);
            rocketLauncher.SetActive(hasRocketLauncher);
            
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
                StartCoroutine(SwingAnimation());
            else if (hasSpeedIncrease && _canSprint)
                StartCoroutine(Sprint());
            else if (hasRocketLauncher && _canShootRocket && context.started)
                StartCoroutine(ShootRocketLauncher());
            else if (hasFreezeRay && _canShootFreezeRay && context.started)
                StartCoroutine(ShootFreezeRay());

        }



        public void JetPack()
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

        public IEnumerator SwingAnimation()
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
        
        public IEnumerator Sprint()
        {
            // Doubles player speed for short period, then has cooldown period before can be used again
            _canSprint = false;
            _isSprinting = true;
            _walkSpeed *= _speedMultiplier;
            yield return new WaitForSeconds(10f);
            _walkSpeed /= _speedMultiplier;
            yield return new WaitForSeconds(10f);
            _canSprint = true;
            _isSprinting = false;
        }
       

        public void ApplyKnockBack(Vector3 direction, float force)
        {
            // Takes in Vector3 direction value, applies force
            _knockBackCounter = KnockBackTime;
            _moveDirection = direction * force * KnockBackTime;
            _moveDirection.y = 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks Trigger, and starts knockback sequence.
            if (other.gameObject.CompareTag("KnockBack"))
            {
                Vector3 hitDirection = other.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                ApplyKnockBack(hitDirection, 10f);
                _animator.SetTrigger(Falling);
            }

            // Landing particles
            if (other.gameObject.CompareTag("InnerGravity")) 
                landParticles.Play();

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("FreezeRay"))
                StartCoroutine(FreezePlayer());
        }
        private void OnCollisionEnter(Collision collision)
        {
            if ( collision.gameObject.tag == "Rocket")
            {
                //applies more knockback than stick
                Vector3 hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                ApplyKnockBack(hitDirection, 20f);
                _animator.SetTrigger(Falling);
            }
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddForce(force, forceMode);
        }
        private IEnumerator ShootRocketLauncher()
        {
            _canShootRocket = false;
     
            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(rocket, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity = 
                transform.TransformDirection(Vector3.forward * (_launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(_reloadTime);
            _canShootRocket = true;
        }
        private IEnumerator ShootFreezeRay()
        {
            //identical to rocket launcher but shoots freezeRay projectile
            _canShootFreezeRay = false;

            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(freezeRayProjectile, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity =
                transform.TransformDirection(Vector3.forward * (_launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(_reloadTime);
            _canShootFreezeRay = true;
        }
        private IEnumerator FreezePlayer()
        {
            //slows down player for alloted time
            if(!_isSprinting )
                _walkSpeed = 3f;
            else
                _walkSpeed = 3f * _speedMultiplier;

            yield return new WaitForSeconds(_freezeTime);
            if (!_isSprinting)
                _walkSpeed = 13f;
            else
                _walkSpeed = 13f * _speedMultiplier;
        }
    }
}
