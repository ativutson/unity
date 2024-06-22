using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(20);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            Heal(20);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Math.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healingAmount = Math.Clamp(healthAmount, 0, 100f);

        healthBar.fillAmount = healingAmount / 100f;
    }
}
