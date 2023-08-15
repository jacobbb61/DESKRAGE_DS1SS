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


    public Animator VictoryAnim;

    public EventReference BossKilledAudio;
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
        FMODinstance.release();
    }
    public void EnterArena() //called from door manager
    {
        doorT.SwitchDoorState("Fog");
        doorU.SwitchDoorState("Fog");
        doorT.inBossFight = true;
        doorU.inBossFight = true;
        doorT.ManualStart();
        doorU.ManualStart();

        if (currentState == "FirstTime")
        {
            StartCinematic();


           // SwitchState("Active");

            inBossFight = true;
            arenaIsActive = true;
           // Boss.Behaviour = "Hostile";
        }

        if (currentState == "Idle")
        {
            SwitchState("Active");
            inBossFight = true;
            arenaIsActive = true;
            Boss.Behaviour = "Hostile";
        }

        FMODinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_FirstPhase);
        FMODinstance.start();
        FMODinstance.release();

    }
    IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
    private void OnEnable()
    {
        SwitchState(currentState);
    }

    public void SwitchState(string state)
    {
        currentState = state;

        switch (currentState)
        {
            case "FirstTime":
                {   //Door states are "Open", "Closed", "Locked", "OneSided", "Fog", "FogEnter"
                    doorT.SwitchDoorState("Closed"); 
                    doorU.SwitchDoorState("Locked");
                    doorT.ManualStart();
                    doorU.ManualStart();
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
                    Boss.Behaviour = "FirstTime";
                    break;
                }
            case "Idle":
                {
                    doorT.SwitchDoorState("FogEnter");
                    doorU.SwitchDoorState("Fog");
                    doorT.ManualStart();
                    doorU.ManualStart();
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
                    Boss.UpdateUI();
                    Boss.StopAllCoroutines();
                    Boss.Behaviour = "Idle";
                    Boss.ManualStart();
                    StopMusic();
                    break;
                }
            case "Active":
                {
                    doorT.SwitchDoorState("Fog");
                    doorU.SwitchDoorState("Fog");
                    doorT.ManualStart();
                    doorU.ManualStart();
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
                   /*
                    FMODinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_FirstPhase);
                    FMODinstance.start();
                    FMODinstance.release();
                   */
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
                    break;
                }
        }
    }

    public void Victory()
    {
        RuntimeManager.PlayOneShot(BossKilledAudio);
        VictoryAnim.Play("Active");
    }
    public void BossKilled() //Called by boss script
    {
        SwitchState("Open");
        arenaIsActive = false;



        doorT.SwitchDoorState("Locked");
        doorT.ManualStart();

        doorU.SwitchDoorState("Closed");
        doorU.ManualStart();

        Bridge.currentState = "Open";
        Bridge.ManualStart();



        // Audio stuffs
        // Wait(time);
        // Disable boss health
        // Achievement and saving stuff
    }



    public void StartCinematic()
    {
        inBossFight = true;
        arenaIsActive = true;
        Boss.Behaviour = "Cinematic";
        Boss.Assets.SetActive(true);
        //trigger bridge wait to break
        Bridge.StartCoroutine(Bridge.WaitToBreak());
        //trigger bridge animation
        Bridge.Anim.Play("Breaking");
        //trigger boss animation
        Boss.Anim.Play("Intro");

        StartCoroutine(WaitToStartBoss());
    }

    IEnumerator WaitToStartBoss()
    {
        yield return new WaitForSeconds(2.5f);
        SwitchState("Active");        
    }

}
