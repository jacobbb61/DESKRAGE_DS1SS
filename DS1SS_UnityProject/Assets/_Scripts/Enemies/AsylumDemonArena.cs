using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AsylumDemonArena : MonoBehaviour
{
    [Tooltip("Doors in this array should be E, F1, M1, M2 and N, in that order")][SerializeField] DoorManager[] doors = new DoorManager[5];
    public bool inBossFight;
    public bool arenaIsActive;
    public string currentState;

    public GameObject BossUI;

    public AsylumDemon Boss;

    public EventReference AsylumTheme;
    public FMOD.Studio.EventInstance FMODinstance;

    public OscarManager Oscar;
    public void ManualStart()
    {
        SwitchState(currentState);
        doors[0].DemonArena = this;
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].DemonArena = this;
        }
        Boss.ManualStart();
    }
    public void StopMusic()
    {
        FMODinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void EnterArena() //called from door manager
    {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].inBossFight = true;
                doors[i].ManualStart();
            }

            if (currentState == "FirstTime" || currentState == "Idle")
            {
                SwitchState("Active");
                inBossFight = true;
                arenaIsActive = true;
                Boss.Behaviour = "Hostile";
            }
            if (inBossFight)
            {
                // Display boss health
                // Play boss music
                // Achievement and saving stuff
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
                    inBossFight = false;
                    arenaIsActive = false;
                    BossUI.SetActive(false);
                    Boss.IsActive = false;
                    Boss.IsDead = false;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.Health = Boss.MaxHealth;
                    Boss.Behaviour = "Idle";
                    break;
                }
            case "Idle":
                {
                    Debug.Log("Bos set to idle");
                    doors[0].SwitchDoorState("FogEnter"/*FogEnter not implemented yet*/); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("FogEnter"/*FogEnter not implemented yet*/); //Door M1
                    doors[3].SwitchDoorState("Fog"); //Door M2
                    doors[4].SwitchDoorState("Fog"); //Door N
                    inBossFight = false;
                    arenaIsActive = false;
                    BossUI.SetActive(false);
                    Boss.IsActive = false;
                    Boss.IsDead = false;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.Health = Boss.MaxHealth;
                    Boss.StopAllCoroutines();
                    Boss.Behaviour = "Idle";
                    Boss.ManualStart();
                    FMODinstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                   // FMODinstance.release();
                    break;
                }
            case "Active":
                {
                    doors[0].SwitchDoorState("Fog"); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Fog"); //Door M1
                    doors[3].SwitchDoorState("Fog"); //Door M2
                    doors[4].SwitchDoorState("Fog"); //Door N
                    inBossFight = true;
                    arenaIsActive = true;
                    BossUI.SetActive(true);
                    Boss.IsActive = true;
                    Boss.IsDead = false;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.Health = Boss.MaxHealth;
                    Boss.Behaviour = "Hostile";
                    FMODinstance = FMODUnity.RuntimeManager.CreateInstance(AsylumTheme);
                    FMODinstance.start();
                   // FMODinstance.release();
                    break;
                }
            case "Open":
                {
                  /*  doors[0].SwitchDoorState("Open"); //Door E
                    doors[1].SwitchDoorState("Open"); //Door F1
                    doors[2].SwitchDoorState("Open"); //Door M1
                    doors[3].SwitchDoorState("Open"); //Door M2
                   doors[4].SwitchDoorState("Open"); //Door N */ //door save manager will save these
                    inBossFight = false;
                    arenaIsActive = false;
                    BossUI.SetActive(false);
                    Boss.IsActive = false;
                    Boss.IsDead = true;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.gameObject.SetActive(false);
                    Boss.Behaviour = "Dead";
                    FMODinstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                   // FMODinstance.release();
                    break;
                }
        }
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].ManualStart();
        }
    }

    public void BossKilled() //Called by boss script
    {
        SwitchState("Open");
        arenaIsActive = false;

        doors[0].SwitchDoorState("Open"); //Door E
        doors[1].SwitchDoorState("Open"); //Door F1
        doors[2].SwitchDoorState("Open"); //Door M1
        doors[3].SwitchDoorState("Open"); //Door M2
        doors[4].SwitchDoorState("Closed"); //Door N

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].inBossFight = false;
            doors[i].FogAssets.SetActive(false);
            doors[i].ManualStart();
        }

        Oscar.KilledDemon();

        // Audio stuffs
        // Wait(time);
        // Disable boss health
        // Achievement and saving stuff
    }
}
