using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1_CharacterController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;

    // hold our two blend tree params
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float acceleration; // speed up character
    public float deceleration; // slow dowm character
    public float maxWalkVelocity = 0.5f; // set max speeds
    public float maxRunVelocity = 2.0f; // set max speeds

    // ground check
    public Vector3 boxsize; // for our box raycast to check grounding
    public float maxDistance; // distance to ground
    public LayerMask layermask; // layer mask to help identify ground

    // change our mapping to a circle for blend
    public bool InputMapToCircular = true;

    //private float randomMoveCounter = 10.0f; // used to generate random time between movement

    private int groundContactCount = 0;
    private int velocityZHash;
    private int velocityXHash;

    bool m_Started; // overlap box collider detector


    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        m_Started = true;

        // grab the monster Animator Controller
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("Animator could not be found");
        else
            Debug.Log("Animator found");

        velocityZHash = Animator.StringToHash("VelocityZ");
        velocityXHash = Animator.StringToHash("VelocityX");

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.Log("Rigid body could not be found");
        else
            Debug.Log("RB found");
    }



    // handles all increase/decrease to velocity

    void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed,
        bool backPressed, bool runPressed, float currentWalkVelocity) {

        if (forwardPressed && velocityZ < currentWalkVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (backPressed && velocityZ > -currentWalkVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (leftPressed && velocityX > -currentWalkVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightPressed && velocityX < currentWalkVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }


        // slow down when player not pressing forward/back keys
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (!backPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }


    }

    // handler to ensure our max speed has a cap
    void resetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed,
     bool backPressed, bool runPressed, float currentWalkVelocity)
    {
        // if neither left/right are pressed, character should stop turning/moving left and right
        if (!rightPressed && !leftPressed && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // if neither front/back are pressed, character should stop moving
        if (!forwardPressed && !backPressed && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }

        // handle max running speed

        // for forward and backward
        if (forwardPressed && runPressed && velocityZ > currentWalkVelocity)
        {
            velocityZ = currentWalkVelocity;
        }

        // slow down if running not pressed
        else if (forwardPressed && !runPressed && velocityZ > currentWalkVelocity)
        {
            // bring below max value
            velocityZ -= Time.deltaTime * deceleration;
            // if slightly over our max, set to max
            if (velocityZ > currentWalkVelocity && velocityZ < (currentWalkVelocity + 0.05))
            {
                velocityZ = currentWalkVelocity;
            }
        }
        // if slightly below, set to the correct max
        else if (forwardPressed && velocityZ > (currentWalkVelocity - 0.05) && velocityZ < currentWalkVelocity)
        {
            velocityZ = currentWalkVelocity;
        }

        // back run
        if (backPressed && runPressed && velocityZ < -currentWalkVelocity)
        {
            velocityZ = -currentWalkVelocity;
        }
        // slow down if running not pressed
        else if (backPressed && !runPressed && velocityZ < -currentWalkVelocity)
        {
            // bring up over min value
            velocityZ += Time.deltaTime * deceleration;
        }

        // left run locked
        // for forward and backward
        if (leftPressed && runPressed && velocityX < -currentWalkVelocity)
        {
            velocityX = -currentWalkVelocity;
        }

        // slow down if running not pressed
        else if (leftPressed && !runPressed && velocityX < -currentWalkVelocity)
        {
            // if between -2 and -2.05
            velocityX += Time.deltaTime * deceleration;
            // if slightly over our max, set to max
            if (velocityX < -currentWalkVelocity && velocityX > -(currentWalkVelocity + 0.05))
            {
                velocityX = -currentWalkVelocity;
            }
        }
        // if between -1.95 and -2 
        else if (leftPressed && velocityX < -(currentWalkVelocity - 0.05) && velocityX > -currentWalkVelocity)
        {
            velocityX = -currentWalkVelocity;
        }

        // right run locked
        // for forward and backward
        if (rightPressed && runPressed && velocityX > currentWalkVelocity)
        {
            velocityX = currentWalkVelocity;
        }

        // slow down if running not pressed
        else if (rightPressed && !runPressed && velocityX > currentWalkVelocity)
        {
            // if between 2 and 2.05
            velocityX -= Time.deltaTime * deceleration;
            // if slightly over our max, set to max
            if (velocityX > currentWalkVelocity && velocityX < (currentWalkVelocity + 0.05))
            {
                velocityX = currentWalkVelocity;
            }
        }
        // if between 1.95 and 2 
        else if (rightPressed && velocityX > (currentWalkVelocity - 0.05) && velocityX < currentWalkVelocity)
        {
            velocityX = currentWalkVelocity;
        }
    }

    //bool isGrounded()
    //{

        
    //    if ()
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    // visualize our boxcast
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position-transform.up*maxDistance,boxsize);
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    bool isGrounded()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2,
            Quaternion.identity, layermask);

        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }

        if (hitColliders.Length > 0)
        {
            return true;
        }
        else {
            return false;
        }
    }



    // Update is called once per frame
    void Update()
    {

        //isCurrentlyWalking = anim.GetBool("isWalking");

        // WASD input
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool backPressed = Input.GetKey("s");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift"); // to hold our run bool

        // if run pressed, set to Running velocity, else to walk velocity.

        float currentWalkVelocity = runPressed ? maxRunVelocity: maxWalkVelocity;

        changeVelocity( forwardPressed,  leftPressed,  rightPressed,
            backPressed,  runPressed,  currentWalkVelocity);
        resetVelocity( forwardPressed,  leftPressed,  rightPressed,
            backPressed,  runPressed,  currentWalkVelocity);

        anim.SetFloat("VelocityZ", velocityZ);
        anim.SetFloat("VelocityX", velocityX);

        bool leftTurnPressed = Input.GetKey("q");
        bool rightTurnPressed = Input.GetKey("e");

        float rotateAngle = 0f;

        if(leftTurnPressed) {
            rotateAngle += -0.5f;
        }
        else if (rightTurnPressed)
        {
            rotateAngle += 0.5f;
            //rotateAngle = rotateAngle;
        }


        // move forward only if grounded

        
        transform.Translate(Vector3.forward *25 * Time.deltaTime * velocityZ);
        transform.Translate(Vector3.left * 2 * Time.deltaTime * -velocityX);

        transform.Rotate(new Vector3(0, rotateAngle, 0),Space.Self);
        


    }

    void FixedUpdate()
    {
        Debug.Log("Ground check: " + isGrounded());
    }


}
