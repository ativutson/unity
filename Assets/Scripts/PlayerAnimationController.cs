using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    //public GameObject lossTextObject;
    
    void Start()
    {
        //lossTextObject.SetActive(false);

        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");

        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");
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
        bool isGrounded = IsGrounded || CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        anim.speed = animationSpeed;
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isFalling", !isGrounded);
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;

            // Generate an event that might play a sound, generate a particle effect, etc.
            //EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
        /*else if (collision.transform.gameObject.tag == "Monster")
        {
            lossTextObject.SetActive(true);
        }*/
						
    }

    private void OnTriggerExit(Collider collision)
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

        bool isGrounded = IsGrounded || CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

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

        newRootPosition = Vector3.LerpUnclamped(anim.rootPosition, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(anim.rootRotation, newRootRotation, rootTurnSpeed);
        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }

    public static bool CheckGroundNear(
        Vector3 charPos,       
        float jumpableGroundNormalMaxAngle, 
        float rayDepth, //how far down from charPos will we look for ground?
        float rayOriginOffset, //charPos near bottom of collider, so need a fudge factor up away from there
        out bool isJumpable
    ) 
    {

        bool ret = false;
        bool _isJumpable = false;


        float totalRayLen = rayOriginOffset + rayDepth;

        Ray ray = new Ray(charPos + Vector3.up * rayOriginOffset, Vector3.down);

        int layerMask = 1 << LayerMask.NameToLayer("Default");


        RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask);

        RaycastHit groundHit = new RaycastHit();

        foreach(RaycastHit hit in hits)
        {

            if (hit.collider.gameObject.CompareTag("ground"))
            {           

                ret = true;

                groundHit = hit;

                _isJumpable = Vector3.Angle(Vector3.up, hit.normal) < jumpableGroundNormalMaxAngle;

                break; //only need to find the ground once

            }

        }

        DrawRay(ray, totalRayLen, hits.Length > 0, groundHit, Color.magenta, Color.green);

        isJumpable = _isJumpable;

        return ret;
    }

    public static void DrawRay(Ray ray, float rayLength, bool hitFound, RaycastHit hit, Color rayColor, Color hitColor) {
            
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayLength, rayColor); 
                       
            if (hitFound)
            {
                //draw an X that denotes where ray hit
                const float ZBufFix = 0.01f;
                const float edgeSize = 0.2f;
                Color col = hitColor;

                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.forward * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.left * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.right * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.back * edgeSize, col);
            }
        }

}
