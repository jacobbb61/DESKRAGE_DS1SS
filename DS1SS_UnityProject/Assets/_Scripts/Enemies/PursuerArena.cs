using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuerArena : MonoBehaviour
{
    [Tooltip("Doors in this array should be P, T, U in that order")][SerializeField] DoorManager[] doors = new DoorManager[3];
    public bool bossDead;
    public bool inBossFight;
    public bool arenaIsActive;
    public string currentState;

    private void Start()
    {
        SwitchState("FirstTime");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].inBossFight = true;
            }

            if (!bossDead)
            {
                SwitchState("Active");
                inBossFight = true;
                arenaIsActive = true;
            }
            // Display boss health
            // Play boss music
            // Achievement and saving stuff
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!bossDead)
            {
                arenaIsActive = false;
                SwitchState("Idle");
            }
            //Saving stuff
        }
    }

    IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    public void SwitchState(string state)
    {
        currentState = state;

        switch (currentState)
        {
            case "FirstTime":
                {   //Door states are "Open", "Closed", "Locked", "OneSided", "Fog", "FogEnter"
                    doors[0].SwitchDoorState("Closed"); //Door P
                    doors[1].SwitchDoorState("Closed"); //Door T
                    doors[2].SwitchDoorState("Locked"); //Door U
                    break;
                }
            case "Idle":
                {
                    doors[0].SwitchDoorState("Open"); //Door P
                    doors[1].SwitchDoorState("Fog"/*FogEnter not implemented yet*/); //Door T
                    doors[2].SwitchDoorState("Locked"); //Door U
                    break;
                }
            case "Active":
                {
                    doors[0].SwitchDoorState("Open"); //Door P
                    doors[1].SwitchDoorState("Fog"); //Door T
                    doors[2].SwitchDoorState("Fog"); //Door U
                    break;
                }
            case "Open":
                {
                    doors[0].SwitchDoorState("Open"); //Door P
                    doors[1].SwitchDoorState("OneSided"); //Door T
                    doors[2].SwitchDoorState("Closed"); //Door U
                    break;
                }
        }
    }

    public void BossKilled() //Called by boss script
    {
        bossDead = true;
        arenaIsActive = false;
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].inBossFight = false;
            SwitchState("Open");
        }
        // Audio stuffs
        // Wait(time);
        // Disable boss health
        // Achievement and saving stuff
    }
}
