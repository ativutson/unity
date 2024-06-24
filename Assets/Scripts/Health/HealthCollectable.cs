using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    void OnTriggerEnter(Collider c) {
        if(c.attachedRigidbody != null){
            HealthCollector hc = c.attachedRigidbody.gameObject.GetComponent<HealthCollector>();
            if(hc != null){
                //EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
                Destroy(this.gameObject);
                hc.ReceiveHealth();
            }
        }
    }
}
