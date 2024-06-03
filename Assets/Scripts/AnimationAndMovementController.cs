using UnityEngine.InputSystem;
using UnityEngine;

public class AnimationAndMovementController : MonoBehaviour
{
    
    PlayerInput playerInput;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private CharacterController characterController;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private float rotationFactorPerFrame = 15.0f;

    bool isJumpPressed;
    private bool isJumping;
    private float maxJumpTime = 1f;
    private float maxJumpHeight = 1f;
    private float initalJumpVelocity;

    private float gravity = -9.8f;
    private float groundedGravity = -.05f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        playerInput.CharacterControlls.Move.started += context =>
        {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
        };
        playerInput.CharacterControlls.Move.canceled += context =>
        {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
        };
        playerInput.CharacterControlls.Jump.started += onJump;
        playerInput.CharacterControlls.Jump.canceled += onJump;

        setUpJoumpVars();
    }

    private void setUpJoumpVars()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            animator.SetBool(IsJump, true);
            currentMovement.y = initalJumpVelocity;
        } else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            animator.SetBool(IsJump, false);
            isJumping = false;
        }
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        
        
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
        }
        else
        {
            currentMovement.y += gravity * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovementPressed)
        {
            animator.SetBool(IsWalking, true);
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
        handleRotation();
        handleGravity();
        handleJump();
        characterController.Move(currentMovement * (Time.deltaTime * 100.0f));
    }

    private void OnEnable()
    {
        playerInput.CharacterControlls.Enable();
    }
    
    private void OnDisable()
    {
        playerInput.CharacterControlls.Disable();
    }
}
