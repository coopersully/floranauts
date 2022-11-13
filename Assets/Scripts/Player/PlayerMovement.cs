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

        [Header("Objects")]
        public bool _jetPack = false;
        public bool _stick = false;
        public GameObject stickObj;
        public bool _freezeRay = false;
        public bool _rocketLauncher = false;


        public Vector3 moveDirection;
        private bool inKnockBack = false;
        public float knockBackForce;
        public float knockBackTime;
        public float knockBackCounter;


     
        

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            
            //hides mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //etObject Defaults
            _jetPack = true;
            _stick = true;
            stickObj.SetActive(false);

            _freezeRay = false;
            _rocketLauncher = false;

            knockBackTime = .25f;
            knockBackForce = 10f;
        }

        private void Update()
        {
            if (PauseManager.Instance.isPaused) return;

            //Checks if player is in knockback sequence, sets Bool, and counts down if inKnockBack
            if (knockBackCounter > 0)
            {
                inKnockBack = true;
                knockBackCounter -= Time.deltaTime;
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
            

            if (!inKnockBack)
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
            if (!isGrounded || inKnockBack) return;
            
             _anim.SetTrigger(Jump1);
             rb.AddForce(transform.up * jumpForce);
            
        }

        public void JetPack(InputAction.CallbackContext context) // callbackcontext is not working for a hold function
        {
            //apply force while jetpack input is activated
            if (_jetPack && !inKnockBack)
            {
                rb.AddForce(transform.up * jumpForce);
                Debug.Log("jetpack");
            }
        }

        public void SwingAttack(InputAction.CallbackContext context)
        {
            if (_stick && !inKnockBack)
            {
                _anim.SetTrigger("SwingAttack");
                Debug.Log("attack");
                StartCoroutine(SwingAnimation());
            }
        }
        public Vector3 KnockBack(Vector3 direction)
        {
            
            knockBackCounter = knockBackTime;
            moveDirection = direction * knockBackForce;
            moveDirection.y = knockBackForce;
            return moveDirection;
        }
        IEnumerator SwingAnimation()
        {
            //activates stick and deactivates after the animation plays out
            stickObj.SetActive(true);
            yield return new WaitForSeconds(2f);
            stickObj.SetActive(false);

        }

        private void OnTriggerEnter(Collider other)
        {
            //Checks Trigger, and starts knockback sequence
            if(other.gameObject.tag == "KnockBack")
            {
                Vector3 hitDirection = other.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                KnockBack(hitDirection);
            }
        }



    }
}
