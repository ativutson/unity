using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{

    public Transform swordContainer;
    public bool equipped;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            Debug.Log("sword trigger");
            equipped = true;
            other.attachedRigidbody.isKinematic = true;
            other.transform.SetParent(swordContainer);
            other.transform.localPosition = new Vector3(.5f,2.5f,.5f);
            other.transform.localRotation = Quaternion.Euler(new Vector3(45, 0, 0));
            other.transform.localScale = new Vector3(5,5,5);
        }
    }
}
