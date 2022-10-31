using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;
    GravityControl gravityControl;

    public float gravity = -10f;


    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 10;
    public float jumpForce = 500;
    public LayerMask groundMask;
    public Transform groundCheck;

    public bool isGrounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Rigidbody rb;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        gravityControl = GetComponent<GravityControl>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;



        //hides mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       
    }

    private void Update()
    {
        //References Input Manager and rotates player based on where mouse or right joystick dictates
        transform.Rotate(Vector3.up * inputManager.cameraInput.x * mouseSensitivityX);
        verticalLookRotation += inputManager.cameraInput.y * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -30, -5);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        //references InputManager and moves player based on Input System
        Vector3 moveDirection = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput).normalized;
        Vector3 targetMoveAmount = moveDirection * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);


        //groundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
        //References Input Manager Jump bool and applies upward force if grounded
        if (inputManager.jump_Input == true)
        {
            if (isGrounded)
            {
                rb.AddForce(transform.up * jumpForce);
                inputManager.jump_Input = false;

            }

        }

       

    }

    void FixedUpdate()
    {
        

        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);
    }




}
