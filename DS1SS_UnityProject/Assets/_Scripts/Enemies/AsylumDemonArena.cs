using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AsylumDemonArena : MonoBehaviour
{
    [Tooltip("Doors in this array should be E, F1, M1, M2 and N, in that order")][SerializeField] DoorManager[] doors = new DoorManager[6];
    public bool inBossFight;
    public bool arenaIsActive;
    public bool IsSecondPhase;
    public string currentState;
    public int DemonDeaths;

    public GameObject BossUI;
    public GameObject BossDoorHint;
    public PlayerManager playerManager;
    public SpeedRunManager speedRunManager;

    public AsylumDemon Boss;
    public Animator VictoryAnim;

    public DoorOcludingSection Occlusion;

    public EventReference BossKilledAudio;
    public EventReference BossRoarAudio;
    public EventReference BossDeathAudio;
    public EventReference Theme_FirstPhase;
    public EventReference Theme_SecondPhase;
    public FMOD.Studio.EventInstance FMODinstance;

    public GameObject BossCam;

    public OscarManager Oscar;
    public void ManualStart()
    {

        if (DemonDeaths >= 3)
        {
            BossDoorHint.SetActive(true);

        }



        SwitchState(currentState);
        doors[0].DemonArena = this;
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].DemonArena = this;
        }
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

        RuntimeManager.PlayOneShot(BossRoarAudio, transform.position);
        IsSecondPhase = true;
        StopMusic();
        FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_SecondPhase);
        FMODinstance.start();
    }



    public void EnterArena() //called from door manager
    {

        BossCam.SetActive(true);

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].inBossFight = true;
                doors[i].ManualStart();
            }

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
    }


    public void StartCinematic()
    {
        inBossFight = true;
        arenaIsActive = true;
        Boss.Behaviour = "Cinematic";
        Boss.Assets.SetActive(true);
        //trigger boss animation
        Boss.Anim.Play("Intro");

        StartCoroutine(WaitToStartBoss());
    }

    IEnumerator WaitToStartBoss()
    {
        yield return new WaitForSeconds(1.5f);
        Boss.Anim.Play("AsylumDemonAnim_Idle");
        SwitchState("Active");
    }



    IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    private void OnEnable()
    {
        SwitchState(currentState);
        if (currentState == "Open")
        {
            Occlusion.Open();
        }
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
                    Boss.UpdateUI();
                    Boss.StopAllCoroutines();
                    Boss.Behaviour = "Idle";
                    Boss.ManualStart();
                    StopMusic();
                    BossCam.SetActive(false);
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
                    Boss.gameObject.SetActive(true);
                    Boss.IsActive = true;
                    Boss.IsDead = false;
                    Boss.IsTurning = false;
                    Boss.IsCoolingDown = false;
                    Boss.Health = Boss.MaxHealth;
                    Boss.Behaviour = "Hostile";
                    FMODinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    FMODinstance = FMODUnity.RuntimeManager.CreateInstance(Theme_FirstPhase);
                    FMODinstance.start();
                    FMODinstance.release();
                    doors[0].SwitchDoorState("Fog"); //Door E
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
                    Occlusion.Open();

                    break;
                }
        }
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].ManualStart();
        }
    }

    public void Victory()
    {
        RuntimeManager.PlayOneShot(BossKilledAudio, transform.position);
        RuntimeManager.PlayOneShot(BossDeathAudio, transform.position);
        VictoryAnim.Play("Active");

        Occlusion.RevealArea();
    }

    public void BossKilled() //Called by boss script
    {
        speedRunManager.DemonKilled();
        SwitchState("Open");
        arenaIsActive = false;


        doors[0].SwitchDoorState("Open"); //Door E
        doors[1].SwitchDoorState("Open"); //Door F1
        doors[2].SwitchDoorState("Open"); //Door M1
        doors[3].SwitchDoorState("Open"); //Door M2
        doors[4].SwitchDoorState("Closed"); //Door N
        doors[5].SwitchDoorState("Open"); //Door M1

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].inBossFight = false;
            doors[i].FogAssets.SetActive(false);
            doors[i].ManualStart();
        }

        Oscar.KilledDemon();

        BossCam.SetActive(false);


        WorldSaveGameManager.Instance.Player = playerManager;
        WorldSaveGameManager.Instance.SaveGame();


        // Audio stuffs
        // Wait(time);
        // Disable boss health
        // Achievement and saving stuff
    }
}
