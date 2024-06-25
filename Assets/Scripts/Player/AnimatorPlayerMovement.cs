using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerMovement : MonoBehaviour
{
    Animator animator;
    float vely = 0.0f;
    float velx = 0.0f;
    public bool isJump = false;
    public float acceleration = 2.0f;
    public float deceleration = 4.0f;
    CharacterController characterController;
    Rigidbody rb;
    public float jumpPower;

    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        //bool jumpPressed = Input.GetKey("space");

        if(forwardPressed){
            vely += Time.deltaTime * acceleration;
        }

        if(leftPressed){
            velx -= Time.deltaTime * acceleration;
        }

        if(rightPressed){
            velx += Time.deltaTime * acceleration;
        }

        if(!forwardPressed && vely > 0.0f){
            vely -= Time.deltaTime * deceleration;
        }

        if(!forwardPressed && vely < 0.0f){
            vely = 0.0f;
        }

        if(!leftPressed && velx < 0.0f){
            velx += Time.deltaTime * deceleration;
        }

        if(!rightPressed && velx > 0.0f){
            velx -= Time.deltaTime * deceleration;
        }

        if(!leftPressed && !rightPressed && velx != 0.0f && (velx > -0.05f && velx < 0.05f)){
            velx = 0;
        }

        animator.SetFloat("vely", vely);
        animator.SetFloat("velx", velx);
        
        handleJump();

        /*if(jumpPressed){
            if(!characterController.isGrounded){
                isJump = false;
                return;
            }
            else{
                Debug.Log("Jump");
                isJump = true;
                rb.velocity += new Vector3(velx, 2.0f, vely);
            }
        }
        
        if(!jumpPressed){
            isJump = false;
            rb.velocity += new Vector3(velx, 0.0f, vely);
        }*/
    }

    private void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.transform.CompareTag("ground") && !isGrounded){
            isGrounded = true;
        }
    }

    void handleJump(){
        if(Input.GetKey(KeyCode.Space)){
            isJump = true;
            if(IsGrounded()){
                isGrounded = true;
            }
            else{
                isGrounded = false;
            }
            if(!isGrounded){
                isJump = false;
            }
        }
        else{
            if(IsGrounded()){
                isGrounded = true;
            }
            else{
                isGrounded = false;
            }
            isJump = false;
        }

        animator.SetBool("isJump", isJump);
        animator.SetBool("isGrounded", isGrounded);
    }

    public bool IsGrounded() {
        RaycastHit hit;
        float rayLength = 1.1f; // Adjust based on your character's size
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength)) {
            return true;
        }
        return false;
    }
}
