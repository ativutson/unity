using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public AudioClip clip;

    void OnTriggerEnter(Collider c) {
        if(c.gameObject != null){
            HealthCollector hc = c.gameObject.GetComponent<HealthCollector>();
            if(hc != null){
                hc.ReceiveHealth();
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
}
