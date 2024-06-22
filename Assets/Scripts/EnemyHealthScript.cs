using System;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(.20f);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(-.20f);
        }
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
