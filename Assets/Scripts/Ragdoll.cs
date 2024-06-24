using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies; // hold all the ragdoll body parts
    Collider[] ragdollColliders;
    CharacterJoint[] ragdollJoints;

    Animator anim;
    Collider mainCollider;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // get all rigidbodies from the ragdoll
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        if(rigidbodies.Length == 0)
        {
            Debug.Log("Missing Ragdoll rigidbodies!");
        }
        ragdollJoints = GetComponentsInChildren<CharacterJoint>();
        if (ragdollJoints.Length == 0)
        {
            Debug.Log("Missing Ragdoll Joints!");
        }

        ragdollColliders = GetComponentsInChildren<Collider>();
        if (ragdollColliders.Length == 0)
        {
            Debug.Log("Missing Ragdoll colliders!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("Missing animator!");
        }
        mainCollider = GetComponent<Collider>();
        if (mainCollider == null)
        {
            Debug.Log("Missing mainCollider!");
        }


        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("Missing rb!");
        }


        DeactivateRagdoll(); // default starting state
    }

    public void ActivateRagdoll()
    {
        Debug.Log("Activating ragdoll!");
        anim.enabled = false;

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = true;

        }
        foreach (var j in ragdollJoints)
        {
            j.enableCollision = true;

        }

        mainCollider.enabled = false;

        rb.isKinematic = true;

        return;


    }

    // handles deactivating ragdoll elements and activating kinematic ones
    public void DeactivateRagdoll()
    {
        anim.enabled = true;

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
            
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = false;

        }

        foreach (var j in ragdollJoints)
        {
            j.enableCollision = false;

        }

        mainCollider.enabled = true;
        rb.isKinematic = true;


    }
}
