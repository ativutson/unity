using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollector : MonoBehaviour
{
    public bool hasHealthCollectable = false;

    public void ReceiveHealth() {
        hasHealthCollectable = true; 
    }
}