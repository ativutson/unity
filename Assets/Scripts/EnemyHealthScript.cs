using System;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] FloatingHeathBar healthBar;
    private float currentHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHeathBar>();
        CurrentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(50f);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(-50f);
        }
    }

    // Note a heal is negative damage taken
    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        healthBar.updateHealthBar(currentHealth, maxHealth);
        // @TODO add something when health hits 0
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with sword.");
        // if encounteirng collider of the longsword, take damage
        if (other.gameObject.GetComponent<LongSword>() != null)
        {
            TakeDamage(20f);
        }
    }
}
