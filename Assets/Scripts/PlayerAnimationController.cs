using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(InputCharacterScript))]
public class PlayerAnimationController : MonoBehaviour
{

    private Animator anim;	
    private Rigidbody rbody;
    private InputCharacterScript cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1f;

    float _inputForward = 0f;
    float _inputTurn = 0f;

    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;


    private int groundContactCount = 0;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    public GameObject lossTextObject;
    
    void Start()
    {
        lossTextObject.SetActive(false);
    }

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<InputCharacterScript>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
    }

    private void Update()
    {
        if (cinput.enabled)
        {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;
        }
    }

    void FixedUpdate()
    {
        bool isGrounded = IsGrounded;

        anim.speed = animationSpeed;
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isFalling", !isGrounded);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;

            // Generate an event that might play a sound, generate a particle effect, etc.
            //EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
        else if (collision.transform.gameObject.tag == "Monster")
        {
            lossTextObject.SetActive(true);
        }
						
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded;

        if (isGrounded)
        {
         	//use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;        
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //Here, you could scale the difference in position and rotation to make the character go faster or slower

        // old way
        //this.transform.position = newRootPosition;
        //this.transform.rotation = newRootRotation;
        newRootPosition = Vector3.LerpUnclamped(anim.rootPosition, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(anim.rootRotation, newRootRotation, rootTurnSpeed);
        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }


}
