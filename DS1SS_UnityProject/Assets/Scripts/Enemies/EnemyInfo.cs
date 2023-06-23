using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{

    [SerializeField]int maxHealth = 10;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
            Die();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Add if statement for whatever weapons/ damages the enemy

        ChangeHealth(-2); // add variable to change based on weapon's damage

    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
