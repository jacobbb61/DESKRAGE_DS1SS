using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsylumDemonArena : MonoBehaviour
{
    [Tooltip("Doors in this array should be E, F1, M1, M2 and N, in that order")][SerializeField] DoorManager[] doors = new DoorManager[5];
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
                    doors[0].SwitchDoorState("Closed"); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Open"); //Door M1
                    doors[3].SwitchDoorState("OneSided"); //Door M2
                    doors[4].SwitchDoorState("Locked"); //Door N
                    break;
                }
            case "Idle":
                {
                    doors[0].SwitchDoorState("Fog"/*FogEnter not implemented yet*/); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Fog"/*FogEnter not implemented yet*/); //Door M1
                    doors[3].SwitchDoorState("Fog"); //Door M2
                    doors[4].SwitchDoorState("Fog"); //Door N
                    break;
                }
            case "Active":
                {
                    doors[0].SwitchDoorState("Fog"); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Fog"); //Door M1
                    doors[3].SwitchDoorState("Fog"); //Door M2
                    doors[4].SwitchDoorState("Fog"); //Door N
                    break;
                }
            case "Open":
                {
                    doors[0].SwitchDoorState("Open"); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Open"); //Door M1
                    doors[3].SwitchDoorState("Open"); //Door M2
                    doors[4].SwitchDoorState("Closed"); //Door N
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
