using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerMovement : MonoBehaviour
{
    Animator animator;
    float vely = 0.0f;
    float velx = 0.0f;
    bool isJump = false;
    public float acceleration = 2.0f;
    public float deceleration = 4.0f;
    CharacterController characterController;
    Rigidbody rb;
    public float jumpPower;

    bool isGrounded;

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

        if(!isGrounded){
            if(isJump){
                isJump = false;
            }
        }
        else if(isJump){
            isGrounded = false;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            isJump = true;
        }

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

        animator.SetFloat("vely", vely);
        animator.SetFloat("velx", velx);
        animator.SetBool("isJump", isJump);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.transform.CompareTag("ground") && !isGrounded){
            isGrounded = true;
        }
    }
}
