using System;
using Gravity;
using Interfaces;
using System.Collections;
using Planets;
using UnityEngine;
using UnityEngine.InputSystem;
using Audio;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private GravityControl _gravityControl;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private UIAudioManager _audio;
        private Transform playerTransform;

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

        private float _walkSpeed = 15f;
        private const float JumpForce = 1200f;

        [Header("Grounding")]
        public LayerMask groundMask;
        public Transform groundCheck;
        [HideInInspector]
        public bool isGrounded;
        public float playerGravity = -10f;

        [Header("Items")]
        public PlayerItems playerItems;
        public Transform firePoint;
        public float reloadTime = 5f;

        //jetPack
        public bool hasJetpack;
        public GameObject jetPack;
        private bool _canJetPack = true;
        
        //stick
        public bool hasStick;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        private bool _canSwingStick = true;
        
        //freezeRay
        public bool hasFreezeRay;
        public GameObject freezeRayGun;
        public GameObject freezeRayProjectile;
        public float freezeTime = 10f;
        private bool _canShootFreezeRay = true;
        
        //rocketLauncher
        public bool hasRocketLauncher;
        public GameObject rocketLauncher;
        public GameObject rocket;
        public float launchForce = 50f;
        private bool _canShootRocket = true;
        
        //speedboost
        public bool hasSpeedIncrease;
        public GameObject energyCan;
        public float speedMultiplier = 2f;
        private bool _canSprint = true;
        [HideInInspector]
        public bool isSprinting;


        // Knockback-related 
        [HideInInspector]
        public bool inKnockBack;
        private const float KnockBackTime = .75f;
        private float _knockBackCounter;
        
        [Header("Particles")]
        public ParticleSystem landParticles;
        private bool _canPlayLandParticles = true;
        public ParticleSystem jumpParticles;
        public ParticleSystem walkParticles;
        public ParticleSystem jetParticles;
        public ParticleSystem speedTrail;
        public ParticleSystem frozenParticles;
        
        // Player-related animation triggers
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Attack = Animator.StringToHash("SwingAttack");
        private static readonly int Shoot = Animator.StringToHash("ShootPistol");
        private static readonly int Drink = Animator.StringToHash("Drink");

        private void Awake()
        {
            jetParticles.Stop();
            speedTrail.Stop();
            frozenParticles.Stop();
            _canPlayLandParticles = true;

            // Initialize components
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;

            // Hides mouse cursor
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;

            _canPlayLandParticles = true;
        }

        private void Update()
        {
            hasStick = false;
            hasJetpack = false;
            hasFreezeRay = false;
            hasRocketLauncher = false;
            hasSpeedIncrease = false;
            
            switch (playerItems.selectedItem)
            {
                case PlanetType.None:
                    break;
                case PlanetType.Stick:
                    hasStick = true;
                    break;
                case PlanetType.Jetpack:
                    hasJetpack = true;
                    break;
                case PlanetType.FreezeGun:
                    hasFreezeRay = true;
                    break;
                case PlanetType.RocketLauncher:
                    hasRocketLauncher = true;
                    break;
                case PlanetType.SpeedIncrease:
                    hasSpeedIncrease = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //activates physical items based on bools
            stickObj.SetActive(hasStick);
            jetPack.SetActive(hasJetpack);
            freezeRayGun.SetActive(hasFreezeRay);
            rocketLauncher.SetActive(hasRocketLauncher);
            energyCan.SetActive(hasSpeedIncrease);

            if (PauseManager.Instance == null) return;
            if (PauseManager.Instance.isPaused) return;

            // Checks if player is in knockback sequence, sets Bool, and counts down if inKnockBack
            if (_knockBackCounter > 0)
            {
                inKnockBack = true;
                _knockBackCounter -= Time.deltaTime * 2;
            }
            else inKnockBack = false;

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
            transform.Rotate(Vector3.up * _cameraInput.x * mouseSensitivityX);
            _verticalLookRotation += _cameraInput.y * mouseSensitivityY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -30, -15);
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
            if (!isGrounded || inKnockBack) return;
            
            _animator.SetTrigger(Jump); 
            jumpParticles.Play();

            // Applies upward force and directional force depending on direction player is moving
            _rigidbody.AddForce(transform.up * JumpForce);
            _rigidbody.AddForce(transform.forward * JumpForce * _movementInput);
        }

        public void UseItemAction(InputAction.CallbackContext context)
        {
            if (hasJetpack && context.started && _canJetPack && !inKnockBack)
                StartCoroutine(JetPack());
            else if (hasStick && context.started && _canSwingStick)  // If the key was not pressed this frame, ignore it.
                StartCoroutine(SwingAnimation());
            else if (hasSpeedIncrease && _canSprint && !inKnockBack)
                StartCoroutine(Sprint());
            else if (hasRocketLauncher && _canShootRocket && context.started)
                StartCoroutine(ShootRocketLauncher());
            else if (hasFreezeRay && _canShootFreezeRay && context.started)
                StartCoroutine(ShootFreezeRay());

        }


        private IEnumerator JetPack()
        {
            // Apply force while jetpack input is activated
            _canJetPack = false;
            AudioManager.Instance.fx.JetPack();
            jetParticles.Play(); // Needs edits based on hold

            _animator.SetBool(IsGrounded, isGrounded);
            _rigidbody.AddForce(transform.up * (JumpForce) );
            if(_movementInput.y > 0)
             _rigidbody.AddForce(transform.forward * (JumpForce * 2f));
            else if(_movementInput.y < 0)
                _rigidbody.AddForce(transform.forward * (JumpForce * -2f)); // if moving backward apply force backward


            _animator.SetTrigger(Falling); // Transitions walking animation to falling without having to go through Jump
            Debug.Log("jetpack");
            yield return new WaitForSeconds(.5f);
            _canJetPack = true;
            _gravityControl.NearestPlanet();
        }

        private IEnumerator SwingAnimation()
        {
            // Player cannot swing stick again until animation plays through
            _canSwingStick = false;
            _canPlayLandParticles = false; 
            _animator.SetTrigger(Attack);
            yield return new WaitForSeconds(.5f);
            AudioManager.Instance.fx.StickSwoosh();

            // Activates stick and deactivates after the animation plays out
            stickKnockBack.SetActive(true);
            yield return new WaitForSeconds(1f);
            stickKnockBack.SetActive(false);
            _canSwingStick = true;
            _canPlayLandParticles = true;

        }

        private IEnumerator Sprint()
        {
            // Doubles player speed for short period, then has cooldown period before can be used again
            _animator.SetTrigger(Drink);

            _canSprint = false;
            AudioManager.Instance.fx.EnergyDrink();
            speedTrail.Play();
            isSprinting = true;
            _walkSpeed *= speedMultiplier;
            yield return new WaitForSeconds(20f);
            _walkSpeed /= speedMultiplier;
            speedTrail.Stop();

            yield return new WaitForSeconds(10f);
            _canSprint = true;
            isSprinting = false;

        }
        
        private void ApplyKnockBack(Vector3 direction, float force)
        {
            // Takes in Vector3 direction value, applies force
            _knockBackCounter = KnockBackTime;
            _moveDirection = direction * force;
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
            if (other.gameObject.CompareTag("InnerGravity") && _canPlayLandParticles) 
                landParticles.Play();

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("FreezeRay"))
                StartCoroutine(FreezePlayer());
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Rocket")) return;
            
            //applies more knockback than stick
            Vector3 hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            ApplyKnockBack(hitDirection, 20f);
            _animator.SetTrigger(Falling);
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddForce(force, forceMode);
        }
        private IEnumerator ShootRocketLauncher()
        {
            AudioManager.Instance.fx.RocketLaunch();
            
            _canShootRocket = false;
            _animator.SetTrigger(Shoot);
           // _audio.RocketLaunch();
     
            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(rocket, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity = 
                transform.TransformDirection(Vector3.forward * (launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(reloadTime);
            _canShootRocket = true;
        }
        private IEnumerator ShootFreezeRay()
        {
            AudioManager.Instance.fx.FreezeRayLaunch();
            
            //identical to rocket launcher but shoots freezeRay projectile
            _canShootFreezeRay = false;
            _animator.SetTrigger(Shoot);
            //_audio.FreezeRayLaunch();


            //instatiates projectile and adds velocity
            var projectileObj = Instantiate(freezeRayProjectile, firePoint.position, Quaternion.identity);
            projectileObj.GetComponent<Rigidbody>().velocity =
                transform.TransformDirection(Vector3.forward * (launchForce));

            //wait before can shoot again
            yield return new WaitForSeconds(reloadTime);
            _canShootFreezeRay = true;
        }
        private IEnumerator FreezePlayer()
        {
            //slows down player for alloted time
            if(!isSprinting )
                _walkSpeed = 3f;
            else
                _walkSpeed = 3f * speedMultiplier;
            frozenParticles.Play();

            yield return new WaitForSeconds(freezeTime);
            if (!isSprinting)
                _walkSpeed = 15f;
            else
                _walkSpeed = 15f * speedMultiplier;
            frozenParticles.Stop();

        }
    }
}
