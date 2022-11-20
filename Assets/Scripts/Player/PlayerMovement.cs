using Gravity;
using Interfaces;
using System.Collections;
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
        [HideInInspector] public Rigidbody rb;

        private float _verticalLookRotation;
        private Transform _cameraTransform;
        //movement
        public float mouseSensitivityX = 1;
        public float mouseSensitivityY = 1;
        private Vector3 _moveAmount;
        private Vector3 _smoothMoveVelocity;
        private Vector3 moveDirection;

        private float walkSpeed = 13;
        private float jumpForce = 1200;

        //groundCheck
        public LayerMask groundMask;
        public Transform groundCheck;
        public bool isGrounded;
        public float playerGravity;
        
        //animation
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        [Header("Objects")]
        public bool hasJetpack = false;
        public GameObject jetPack;
        public bool hasStick = false;
        private bool canSwingStick = true;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        public bool hasFreezeRay = false;
        public GameObject freezeRay;
        public bool hasRocketLauncher = false;
        public bool hasSpeedIncrease = false;
        private bool canSprint = true;

        //KnockBack
        private bool _inKnockBack = false;
        private float knockBackForce = 10f;
        private float knockBackTime = .75f;
        private float knockBackCounter;
        private static readonly int Attack = Animator.StringToHash("SwingAttack");

        [Header("Particles")]
        public ParticleSystem landParticles;
        public ParticleSystem jumpParticles;
        public ParticleSystem walkParticles;
        public ParticleSystem jetParticles;



        private void Awake()
        {
            _anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            playerGravity = -10f;
            _cameraTransform = GetComponentInChildren<Camera>().transform;

            
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Object Defaults
            //hasJetpack = true;
            jetParticles.Stop();
            //hasStick = true;
            //hasSpeedIncrease = true;
            //hasFreezeRay = true;
            //hasRocketLauncher = true;

            
        }

        private void Update()
        {
            
            stickObj.SetActive(hasStick); //Shows physical stick if item is activated
            jetPack.SetActive(hasJetpack);
            freezeRay.SetActive(hasFreezeRay);
            
            if (PauseManager.Instance.isPaused) return;

            // Checks if player is in knockback sequence, sets Bool, and counts down if inKnockBack
            if (knockBackCounter > 0)
            {
                _inKnockBack = true;
                knockBackCounter -= Time.deltaTime * 2;
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
            _anim.SetBool(IsGrounded, isGrounded);
            //Debug.Log("isGrounded" + isGrounded);
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
            

            if (!_inKnockBack)
            {
                // If player is not in knockBack, Move player based on Input System
                moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
                _anim.SetFloat(Horizontal, movementInput.x);
                _anim.SetFloat(Vertical, movementInput.y);
            }
           
            var targetMoveAmount = moveDirection * walkSpeed;

            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);

            //Plays walking particles if palyer is on a planet and moving
            if(movementInput.magnitude > 0.3 && isGrounded)
                walkParticles.Play();
            else 
                walkParticles.Stop();

            /**Sets player gravity which accelerates over time if not grounded
             * (accesed by gravityAttractor script)**/
            if (isGrounded)
                playerGravity = -10;
            else
                playerGravity -= 50 * Time.deltaTime;
            //Debug.Log("Player Gravity: " + playerGravity);

        }

        public void Move(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
             // If the player is not grounded or is in Knock Back sequence, ignore the jump event.
            if (!isGrounded || _inKnockBack) return;
            
             _anim.SetTrigger(Jump1);
             jumpParticles.Play();

            //applies upward force and directional force depending on direction player is moving
             rb.AddForce(transform.up * jumpForce);
             rb.AddForce(transform.forward * jumpForce * movementInput);
        }

        public void UseItem(InputAction.CallbackContext context)
        {
            if (hasJetpack && context.started)
                JetPack();
            else if (hasStick && context.started && canSwingStick)  // If the key was not pressed this frame, ignore it.
                SwingAttack();
            else if (hasSpeedIncrease && canSprint)
                StartCoroutine(Sprint());
        }

        public void JetPack() 
        {
            // Apply force while jetpack input is activated
            if (!hasJetpack || _inKnockBack) return;

            jetParticles.Play(); // needs edits based on hold

            _anim.SetBool(IsGrounded, isGrounded);
            rb.AddForce(transform.up * jumpForce );
            rb.AddForce(transform.forward * jumpForce * movementInput * 5f);


            _anim.SetTrigger("Falling"); //Transitions walking animation to falling without having to go through Jump
            
            Debug.Log("jetpack");
        }

        public void SwingAttack()
        {
            // If the player is currently in knockback, ignore this event. 
            if ( _inKnockBack ) return;
            
            StartCoroutine(SwingAnimation());
        }
        private IEnumerator SwingAnimation()
        {
            //player cannot swing stick again until animation plays through
            canSwingStick = false;
            _anim.SetTrigger(Attack);
            yield return new WaitForSeconds(.75f);

            //activates stick and deactivates after the animation plays out
            stickObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            stickObj.SetActive(false);
            canSwingStick = true;
        }
        private IEnumerator Sprint()
        {
            //doubles player speed for short period, then has cooldown period before can be used again
            canSprint = false;
            walkSpeed *= 2;
            yield return new WaitForSeconds(10f);
            walkSpeed /= 2;
            yield return new WaitForSeconds(20f);
            canSprint = true;
            Debug.Log("can Sprint");
        }

        private Vector3 KnockBack(Vector3 direction)
        {
            //takes in direction value, applies force
            knockBackCounter = knockBackTime;
            moveDirection = direction * knockBackForce;
            moveDirection.y = 2f;
            return moveDirection;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks Trigger, and starts knockback sequence.
            if (other.gameObject.CompareTag("KnockBack"))
            {
                Vector3 hitDirection = other.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                KnockBack(hitDirection);
                _anim.SetTrigger("Falling");
            }

            if (other.gameObject.CompareTag("InnerGravity"))
            {
                //landing particles
                landParticles.Play();
            }

        }
    }
}
