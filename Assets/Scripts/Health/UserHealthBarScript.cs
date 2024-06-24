using System;
using UnityEngine;

public class UserHealthBarScript : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] FloatingHeathBar healthBar;
    private float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHeathBar>();
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            TakeDamage(.2f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        
    }

    // Note a heal is negative damage taken
    void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        healthBar.updateHealthBar(currentHealth, maxHealth);
        // @TODO add something when health hits 0
    }
}
