using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollector : MonoBehaviour
{
    public bool hasHealthCollectable = false;

    public void ReceiveHealth() {
        hasHealthCollectable = true; 
        //UserHealthBarScript health = GetComponent<UserHealthBarScript>();
        //health.TakeDamage(-10f);
    }
}