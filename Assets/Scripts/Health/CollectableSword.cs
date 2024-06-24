using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSword : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        SwordCollector bc = null;

        if (c.attachedRigidbody != null)
        {
            bc = c.attachedRigidbody.gameObject.GetComponent<SwordCollector>();
        }

        if (bc != null)
        {
            Destroy(this.gameObject);
            bc.ReceiveSword();
        }
    }
}
