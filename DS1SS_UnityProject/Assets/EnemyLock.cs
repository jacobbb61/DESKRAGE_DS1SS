using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLock : MonoBehaviour
{

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform nearestEnemyPos;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Assigns all enemy tagged objects into array
    }
    void Update()
    {
        nearestEnemyPos = GetNearestEnemy(); // Finds nearest enemy every frame
        //nearestEnemyPos.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red; // Turns enemy gameobject red for debugging
        
    }

    public Transform GetNearestEnemy()
    {
        Transform trans = null; // Used for enemy transform
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position); // Distance between attached gameobject and enemy

            if (enemyDistance < nearestDistance)
            {
                nearestDistance = enemyDistance;
                trans = enemy.transform; // Sets transform to closest enemy's transform
            }
        }

        return trans;
    }
}
