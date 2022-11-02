using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    Animator animator;

    public Vector2 movementInput;
    private float moveAmount;
   
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;


    public float verticalInput;
    public float horizontalInput;
    int horizontal;
    int vertical;

    public bool jump_Input;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            //references different control in the Input Action "PlayerControls". This will work for computer and controllers
            playerControls.PlayerMovement.HorizontalMovement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Jump.performed += i => jump_Input = true;

        }

        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Awake()
    {
        Animator animator = GetComponent<Animator>();

        
    }
    void Update()
    {
        MovementInput();
        
        
    }

    private void MovementInput() //calculates inputs for movement and camera whic hchanges based on character rotation
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        //UpdateAnimatorValues();


        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
    }
    public void UpdateAnimatorValues()
    {

        this.animator.SetFloat("Horizontal", horizontalInput);
        this.animator.SetFloat("Vertical", verticalInput);

    }

}
