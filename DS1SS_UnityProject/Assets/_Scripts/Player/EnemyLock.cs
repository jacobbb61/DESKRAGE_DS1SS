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
    public Transform EnemyLockPos;
    public GameObject EnemyLockedOnTo;
    public float EnemyDistance;
    public float LockedOnEnemyDistance;
    public float ClosestEnemyDistance;









    public PlayerControllerV2 Pc;
    public GameObject LockOnSymbol;
    public bool enemyToRight;
    public bool LockedOn;
    public bool LockedOnToBoss;
   // private PlayerController playerController;
    public bool running;


    void Update()
    {



        if (LockedOn && EnemyLockPos!=null) // Placeholder input, this needs to be set to left arrow on dpad
        {
            

            // Finds nearest enemy every frame
            LockOnSymbol.transform.position = EnemyLockPos.position;
            if (EnemyLockPos.transform.position.x < transform.position.x) // Detects if enemy is to right of player
            {
                enemyToRight = true;
            }
            else
            {
                enemyToRight = false; // Enemy is to left
            }

            // ||
            if (AsylumArena.currentState == "Active" || PursuerArena.currentState == "Active") // Boss is active
            {
                LockedOnEnemyDistance = Vector3.Distance(transform.position, EnemyLockPos.position);
            }
            if (AsylumArena.currentState != "Active" && PursuerArena.currentState != "Active") // Boss not active
            {
                if (LockedOnToBoss) // still locked onto boss, turn lock on off
                {
                    LockedOnToBoss = false;
                    TurnOffLockOn();
                    Debug.Log("Boss dead");
                }
            }





            if (AsylumArena.currentState != "Active" && PursuerArena.currentState != "Active" && EnemyLockedOnTo!=null) // Boss not active AND ther is an enemy to lock on to
            {
               
                    if (EnemyLockedOnTo.GetComponent<EnemySaveManager>().IsLockOnAble == false)
                    {
                        GetNearestEnemy();
                        Debug.Log("Change to new lock on");
                    }
                    else
                    {
                        LockedOnEnemyDistance = Vector3.Distance(transform.position, EnemyLockPos.position);
                    }
            }


            if (LockedOnEnemyDistance > 15) // out off range
            {
                TurnOffLockOn();
            }
        }     
    }

    public void TurnOffLockOn()
    {
        EnemyLockedOnTo = null;
        LockedOn = false;
        Pc.IsLockedOn = LockedOn;
        LockOnSymbol.SetActive(LockedOn);
    }



    public void ToggleLockOn()
    {
        if (!LockedOn)
        {
            GetNearestEnemy();
        }
        else
        {
            LockedOn = false;
            Pc.IsLockedOn = false;
            LockOnSymbol.SetActive(false);
        }
    }



    public void GetNearestEnemy()
    {
        EnemyLockPos = null;
        AllEnemies = null;
        EnemyDistance = 1001;
        if (AsylumArena.currentState != "Active" && PursuerArena.currentState != "Active")
        {
            //////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////// get list of enemies
            AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            //////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////
            ClosestEnemyDistance = 1000;
            foreach (GameObject enemy in AllEnemies)
            {
                EnemyDistance = Vector3.Distance(transform.position, enemy.transform.position); // Distance between attached gameobject and enemy

                if (EnemyDistance < 15) // in range
                {
                    if (EnemyDistance < ClosestEnemyDistance) // is the closest
                    {
                        if (enemy.GetComponent<EnemySaveManager>().IsLockOnAble == true) // can be locked onto
                        {
                             ClosestEnemyDistance = EnemyDistance;
                             EnemyLockedOnTo = enemy;
                             EnemyLockPos = enemy.transform;

                             LockedOn = true;
                             Pc.IsLockedOn = true;
                             LockOnSymbol.SetActive(true);
                             LockedOnToBoss = false;
                        }
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (PursuerArena.currentState == "Active")
        {
            if (PursuerArena.Boss.IsDead == false)
            {
                LockedOnToBoss = true;
                EnemyLockPos = PursuerLockOnPos.transform;
                EnemyLockedOnTo = PursuerLockOnPos;

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
        else
        {
            if (AsylumArena.Boss.IsDead == false)
            {
                LockedOnToBoss = true;
                EnemyLockPos = AsylumDemonLockOnPos.transform;
                EnemyLockedOnTo = AsylumDemonLockOnPos;

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
    }
}
