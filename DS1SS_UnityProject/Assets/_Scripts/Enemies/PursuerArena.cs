using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PursuerArena : MonoBehaviour
{
    [Tooltip("Doors in this array should be , , ,  and , in that order")]
    [SerializeField] DoorManager doorT;
    [SerializeField] DoorManager doorU;
    [SerializeField] CollapseBridge Bridge; 
    public bool inBossFight;
    public bool arenaIsActive; 
    public bool IsSecondPhase;
    public string currentState;

    public GameObject BossUI;

    public Pursuer Boss;

    public EventReference Theme_FirstPhase;
    public EventReference Theme_SecondPhase;
    public FMOD.Studio.EventInstance FMODinstance;

    public void ManualStart()
    {
        SwitchState(currentState);
        doorT.PursuerArena = this;
        doorU.PursuerArena = this;

        Boss.ManualStart();
    }
    private void OnDisable()
    {
        StopMusic();
    }
    public void StopMusic()
    {
        FMODinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void SecondPhase()
    {
        IsSecondPhase = true;
        StopMusic();
        FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_SecondPhase);
        FMODinstance.start();
    }
    public void EnterArena() //called from door manager
    {
        doorT.SwitchDoorState("Fog");
        doorU.SwitchDoorState("Fog");
        doorT.inBossFight = true;
        doorU.inBossFight = true;
        doorT.ManualStart();
        doorU.ManualStart();


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
                    doorT.SwitchDoorState("Closed"); 
                    doorU.SwitchDoorState("Closed");
                    Bridge.currentState="Closed";
                    Bridge.ManualStart();

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
                    doorT.SwitchDoorState("FogEnter");
                    doorU.SwitchDoorState("Fog");
                    Bridge.currentState = "Open";
                    Bridge.ManualStart();

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
                    doorT.SwitchDoorState("Fog");
                    doorU.SwitchDoorState("Fog");
                    Bridge.currentState = "Open";
                    Bridge.ManualStart();

                    inBossFight = true;
                    arenaIsActive = true;
                    BossUI.SetActive(true);
                    Boss.IsActive = true;
                    Boss.IsDead = false;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.Health = Boss.MaxHealth;
                    Boss.Behaviour = "Hostile";
                    FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_FirstPhase);
                    FMODinstance.start();
                    // FMODinstance.release();
                    break;
                }
            case "Open":
                {
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
    }

    public void BossKilled() //Called by boss script
    {
        SwitchState("Open");
        arenaIsActive = false;

        doorT.SwitchDoorState("Locked");
        doorU.SwitchDoorState("Closed");
        Bridge.currentState = "Open";
        Bridge.ManualStart();




        // Audio stuffs
        // Wait(time);
        // Disable boss health
        // Achievement and saving stuff
    }
}
