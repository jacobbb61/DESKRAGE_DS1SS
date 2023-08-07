using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
public class EnemyLock : MonoBehaviour
{
    public AsylumDemonArena AsylumArena;
    public GameObject AsylumDemonLockOnPos;

    public PursuerArena PursuerArena;
    public GameObject PursuerLockOnPos;

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
                LockedOn = false;
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
        Transform trans = null;

        if (AsylumArena.currentState != "Active" && PursuerArena.currentState != "Active")
        {
            AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            LayerEnemies = new List<GameObject>();
            foreach (GameObject enemy in AllEnemies)
            {
                if (enemy.activeInHierarchy)
                {
                    LayerEnemies.Add(enemy);
                }
            }

            float Closest = 1000;

            foreach (GameObject enemy in LayerEnemies)
            {
                EnemyDistance = Vector3.Distance(transform.position, enemy.transform.position); // Distance between attached gameobject and enemy
                
                if (Closest == 1000) { Closest = EnemyDistance; }

                if (EnemyDistance < Closest)
                {
                    Closest = EnemyDistance;
                }
                // Debug.Log(enemy.name + " is " + EnemyDistance + " far away" + "the closest is " + Closest);
                if (EnemyDistance == Closest)
                {
                   // Debug.Log(enemy.name + " is " + EnemyDistance + " far away");

                    if (Closest < 20) //in camera range
                    {
                        trans = enemy.transform; // Sets transform to closest enemy's transform
                        EnemyLockPos = enemy.transform;
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
                        return trans;
                    }

                }
            }

            LayerEnemies = null;

            return trans;
        } 

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        else if (PursuerArena.currentState == "Active")
        {
            if (PursuerArena.Boss.IsDead == false)
            {
                trans = PursuerLockOnPos.transform;

                EnemyLockPos = PursuerLockOnPos.transform;
                EnemyLockedOnTo = PursuerLockOnPos;

                LockedOn = true;
                Pc.IsLockedOn = true;
                LockOnSymbol.SetActive(true);

                return trans;
            }
            else
            {
                LockedOn = false;
                Pc.IsLockedOn = false;
                LockOnSymbol.SetActive(false);
                return trans;
            }
        }
        else
        {
            if (AsylumArena.Boss.IsDead == false)
            {
                trans = AsylumDemonLockOnPos.transform;

                EnemyLockPos = AsylumDemonLockOnPos.transform;
                EnemyLockedOnTo = AsylumDemonLockOnPos;

                LockedOn = true;
                Pc.IsLockedOn = true;
                LockOnSymbol.SetActive(true);

                return trans;
            }
            else
            {
                LockedOn = false;
                Pc.IsLockedOn = false;
                LockOnSymbol.SetActive(false);
                return trans;
            }
    }
    }
}
