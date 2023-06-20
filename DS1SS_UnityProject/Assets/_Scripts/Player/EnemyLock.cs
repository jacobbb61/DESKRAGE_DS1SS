using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
public class EnemyLock : MonoBehaviour
{
    public GameObject[] AllEnemies;
    public List<GameObject> LayerEnemies;
    public Transform nearestEnemyPos;
    private Transform EnemyLockPos;
    private GameObject EnemyLockedOnTo;
    public float EnemyDistance;
    public PlayerControllerV2 Pc;
    public GameObject LockOnSymbol;
    public bool enemyToRight;
    public bool LockedOn;
   // private PlayerController playerController;
    public bool running;

    void Update()
    {
        if (LockedOn && EnemyLockPos!=null) // Placeholder input, this needs to be set to left arrow on dpad
        {
             // Finds nearest enemy every frame
            LockOnSymbol.transform.position = EnemyLockPos.position;
            if (nearestEnemyPos.transform.position.x < transform.position.x) // Detects if enemy is to right of player
            {
                enemyToRight = true;
            }
            else
            {
                enemyToRight = false; // Enemy is to left
            }


            if (EnemyLockedOnTo.activeInHierarchy == false)
            {
                ToggleLockOn();
            }

            EnemyDistance = Vector3.Distance(transform.position, EnemyLockPos.position);
            if (EnemyDistance >= 20)
            {
                LockedOn = false;
                Pc.IsLockedOn = LockedOn;
                LockOnSymbol.SetActive(LockedOn);
            }

        }

    }

    public void ToggleLockOn()
    {
        Debug.Log("Toggle Lock On");
        if (!LockedOn)
        {
            nearestEnemyPos = GetNearestEnemy();
        }
        else
        {
            LockedOn = false;
            Pc.IsLockedOn = false;
            LockOnSymbol.SetActive(false);
        }
    }



    public Transform GetNearestEnemy()
    {

        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        LayerEnemies = new List<GameObject>();
        foreach (GameObject enemy in AllEnemies)
        {
            if (enemy.GetComponentInChildren<SpriteRenderer>().sortingLayerName == Pc.gameObject.GetComponent<SortingGroup>().sortingLayerName)
            {
                if (enemy.activeInHierarchy)
                {
                    LayerEnemies.Add(enemy);
                }
            }         
        }

        Transform trans = null; // Used for enemy transform
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject enemy in LayerEnemies)
        {
                EnemyDistance = Vector3.Distance(transform.position, enemy.transform.position); // Distance between attached gameobject and enemy

            if (EnemyDistance < 20)
            {
                nearestDistance = EnemyDistance;
                trans = enemy.transform; // Sets transform to closest enemy's transform
                LockOnSymbol.GetComponent<SpriteRenderer>().sortingLayerName = enemy.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
                EnemyLockPos = enemy.GetComponentInChildren<TextMeshProUGUI>().transform;
                EnemyLockedOnTo = enemy;
                LockedOn = true;
                Pc.IsLockedOn = true;
                LockOnSymbol.SetActive(true);      
            }
            else
            {
                LockedOn = false;
                Pc.IsLockedOn = false;
                LockOnSymbol.SetActive(false);
            }
        }

        LayerEnemies = null;

        return trans;
    }
}
