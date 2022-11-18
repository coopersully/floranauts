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
        
        //animation
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        [Header("Objects")]
        public bool hasJetpack = false;
        public bool hasStick = false;
        public GameObject stickObj;
        public GameObject stickKnockBack;
        public bool hasFreezeRay = false;
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



        private void Awake()
        {
            _anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;

            
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Object Defaults
            //hasJetpack = true;
            //hasStick = true;
            hasSpeedIncrease = true;
            //hasFreezeRay = true;
            //hasRocketLauncher = true;

            
        }

        private void Update()
        {
            stickObj.SetActive(hasStick); //Shows physical stick if item is activated
            
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

             rb.AddForce(transform.up * jumpForce);
             rb.AddForce(transform.forward * jumpForce);
        }

        public void UseItem(InputAction.CallbackContext context)
        {
            if (hasJetpack)
                JetPack();
            else if (hasStick && context.started)  // If the key was not pressed this frame, ignore it.
                SwingAttack();
            else if (hasSpeedIncrease && canSprint)
                StartCoroutine(Sprint());
        }

        public void JetPack() 
        {
            // Apply force while jetpack input is activated
            if (!hasJetpack || _inKnockBack) return;

            _anim.SetBool(IsGrounded, isGrounded);
            rb.AddForce(transform.up * jumpForce);
            rb.AddForce(transform.forward * jumpForce);
            
            Debug.Log("jetpack");
        }

        public void SwingAttack()
        {
            /* If the player doesn't have a stick OR is currently
             in knockback, ignore this event. */
            if (!hasStick || _inKnockBack) return;
            
            StartCoroutine(SwingAnimation());
        }
        private IEnumerator SwingAnimation()
        {
            //activates stick and deactivates after the animation plays out
            _anim.SetTrigger(Attack);
            yield return new WaitForSeconds(.75f);

            stickObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            stickObj.SetActive(false);
        }
        private IEnumerator Sprint()
        {
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
            moveDirection.y = knockBackForce / 2;
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
            }

           


        }

       




    }
}
