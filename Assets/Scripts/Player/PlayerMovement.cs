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
        
        public float mouseSensitivityX = 1;
        public float mouseSensitivityY = 1;
        private float walkSpeed = 13;
        private float jumpForce = 1000;
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

        [Header("Objects")]
        public bool hasJetpack = false;
        public bool hasStick = false;
        public GameObject stickObj;
        public bool hasFreezeRay = false;
        public bool hasRocketLauncher = false;


        public Vector3 moveDirection;
        private bool _inKnockBack = false;
        public float knockBackForce;
        public float knockBackTime;
        public float knockBackCounter;
        private static readonly int Attack = Animator.StringToHash("SwingAttack");


        private void Awake()
        {
            _anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //etObject Defaults
            hasJetpack = true;
            hasStick = true;
            stickObj.SetActive(false);

            hasFreezeRay = false;
            hasRocketLauncher = false;

            knockBackTime = .25f;
            knockBackForce = 10f;
        }

        private void Update()
        {
            if (PauseManager.Instance.isPaused) return;

            // Checks if player is in knockback sequence, sets Bool, and counts down if inKnockBack
            if (knockBackCounter > 0)
            {
                _inKnockBack = true;
                knockBackCounter -= Time.deltaTime;
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
             rb.AddForce(transform.up * jumpForce);
        }

        public void JetPack(InputAction.CallbackContext context) // callbackcontext is not working for a hold function
        {
            // Apply force while jetpack input is activated
            if (!hasJetpack || _inKnockBack) return;
            
            rb.AddForce(transform.up * jumpForce);
            Debug.Log("jetpack");
        }

        public void SwingAttack(InputAction.CallbackContext context)
        {
            // If the key was not pressed this frame, ignore it.
            if (!context.started) return;

            /* If the player doesn't have a stick OR is currently
             in knockback, ignore this event. */
            if (!hasStick || _inKnockBack) return;
            
            _anim.SetTrigger(Attack);
            Debug.Log("attack");
            StartCoroutine(SwingAnimation());
        }

        private Vector3 KnockBack(Vector3 direction)
        {
            knockBackCounter = knockBackTime;
            moveDirection = direction * knockBackForce;
            moveDirection.y = knockBackForce;
            return moveDirection;
        }

        private IEnumerator SwingAnimation()
        {
            //activates stick and deactivates after the animation plays out
            stickObj.SetActive(true);
            yield return new WaitForSeconds(2f);
            stickObj.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            /* Checks Trigger, and starts knockback sequence.
             If it doesn't have the correct tag, ignore the event. */
            if (!other.gameObject.CompareTag("KnockBack")) return;
            
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            KnockBack(hitDirection);
        }



    }
}
