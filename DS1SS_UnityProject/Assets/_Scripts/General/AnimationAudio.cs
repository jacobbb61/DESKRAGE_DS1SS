using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AnimationAudio : MonoBehaviour
{
    [Header("Combat")]
    public EventReference ShortSwingAudioRef;
    public EventReference MedSwingAudioRef;
    public EventReference LongSwingAudioRef;
    public EventReference ParryAudioRef;
    public EventReference PlayerDamageAudioRef;


    [Header("Walking")]
    public EventReference WalkAudioRef;


    [Header("Movement")]
    public EventReference RollAudioRef;
    public EventReference WaterRollAudioRef;


    [Header("Misc")]
    public EventReference EstusAudioRef;

    [Header("Hitting Things")]
    public EventReference HitWoodRef;
    public EventReference HitStoneRef;
    public EventReference HitMetalRef;
    public EventReference HitMeatRef;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EstusAudio()
    {
        RuntimeManager.PlayOneShot(EstusAudioRef, transform.position);
    }

    public void ShortSwingAudio()
    {
        RuntimeManager.PlayOneShot(ShortSwingAudioRef, transform.position);
    }
    public void MedSwingAudio()
    {
        RuntimeManager.PlayOneShot(MedSwingAudioRef, transform.position);
    }
    public void LongSwingAudio()
    {
        RuntimeManager.PlayOneShot(LongSwingAudioRef, transform.position);
    }
    public void PlayerDamageAudio()
    {
        RuntimeManager.PlayOneShot(PlayerDamageAudioRef, transform.position);
    }
    public void WalkAudio()
    {
        EventInstance walk = RuntimeManager.CreateInstance(WalkAudioRef);
        RuntimeManager.AttachInstanceToGameObject(walk, transform, GetComponentInParent<Rigidbody2D>());

        if (GetComponentInParent<PlayerControllerV2>().State == "Running")
        {
            walk.setParameterByName("WalkorRun", 1);
        }
        else
        {
            walk.setParameterByName("WalkorRun", 0);
        }

        switch (GetComponentInParent<PlayerControllerV2>().GroundType)
        {
            case "Grass":
                walk.setParameterByName("Terrain", 0);
                break;
            case "StoneDirty":
                walk.setParameterByName("Terrain", 1); 
                break;
            case "Stone":
                walk.setParameterByName("Terrain", 2);
                break;
            case "Wood":
                walk.setParameterByName("Terrain", 3);
                break;
            case "Snow":
                walk.setParameterByName("Terrain", 5);
                break;
            case "WetStone":
                walk.setParameterByName("Terrain", 6);
                break;
            default:
                //RuntimeManager.PlayOneShot(WalkAudioRef, transform.position);
                break;
        }

        walk.start();
        walk.release();

    }

    public void LandAudio()
    {
        EventInstance walk = RuntimeManager.CreateInstance(WalkAudioRef);
        RuntimeManager.AttachInstanceToGameObject(walk, transform, GetComponentInParent<Rigidbody2D>());


        walk.setParameterByName("WalkorRun", 1);


        switch (GetComponentInParent<PlayerControllerV2>().GroundType)
        {
            case "Grass":
                walk.setParameterByName("Terrain", 0);
                break;
            case "StoneDirty":
                walk.setParameterByName("Terrain", 1);
                break;
            case "Stone":
                walk.setParameterByName("Terrain", 2);
                break;
            case "Wood":
                walk.setParameterByName("Terrain", 3);
                break;
            case "WetStone":
                walk.setParameterByName("Terrain", 6);
                break;
            case "Snow":
                walk.setParameterByName("Terrain", 5);
                break;
            default:
                //RuntimeManager.PlayOneShot(WalkAudioRef, transform.position);
                break;
        }

        walk.start();
        walk.release();

    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RollAudio()
    {
        switch (GetComponentInParent<PlayerControllerV2>().GroundType)
        {
            case "WetStone":
                RuntimeManager.PlayOneShot(WaterRollAudioRef, transform.position);
                break;
            default:
                RuntimeManager.PlayOneShot(RollAudioRef, transform.position);
                break;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void HitObjectAudio(string type)
    {
        switch (type)
        {
            case "Wood":
                RuntimeManager.PlayOneShot(HitWoodRef, transform.position);
                break;
            case "Stone":
                RuntimeManager.PlayOneShot(HitStoneRef, transform.position);
                break;
            case "Metal":
                RuntimeManager.PlayOneShot(HitMetalRef, transform.position);
                break;
            default:
                RuntimeManager.PlayOneShot(HitStoneRef, transform.position);
                break;
        }
    }
    public void HitMetal()
    {
        RuntimeManager.PlayOneShot(HitMetalRef, transform.position);
    }
    public void HitFlesh()
    {
        RuntimeManager.PlayOneShot(HitMeatRef, transform.position);
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
}
