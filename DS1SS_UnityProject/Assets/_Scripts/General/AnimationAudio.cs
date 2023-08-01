using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 

public class AnimationAudio : MonoBehaviour
{
    [Header("Combat")]
    public EventReference ShortSwingAudioRef;
    public EventReference MedSwingAudioRef;
    public EventReference LongSwingAudioRef;
    public EventReference PlayerDamageAudioRef;


    [Header("Walking")]
    public EventReference WalkAudioRef;
    public EventReference Walk_Stone_AudioRef;
    public EventReference Walk_WetStone_AudioRef;
    public EventReference Walk_Wood_AudioRef;
    public EventReference Walk_Snow_AudioRef;

    [Header("Movement")]
    public EventReference RollAudioRef;

    [Header("Hitting Things")]
    public EventReference HitWoodRef;
    public EventReference HitStoneRef;
    public EventReference HitMetalRef;
    public EventReference HitMeatRef;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        switch (GetComponentInParent<PlayerControllerV2>().GroundType)
        {
            case "Wood":
                RuntimeManager.PlayOneShot(Walk_Wood_AudioRef, transform.position);
                break;
            case "Stone":
                RuntimeManager.PlayOneShot(Walk_Stone_AudioRef, transform.position);
                break;
            case "WetStone":
                RuntimeManager.PlayOneShot(Walk_WetStone_AudioRef, transform.position);
                break;
            case "Snow":
                RuntimeManager.PlayOneShot(Walk_Snow_AudioRef, transform.position);
                break;
            default:
                RuntimeManager.PlayOneShot(WalkAudioRef, transform.position);
                break;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RollAudio()
    {
        RuntimeManager.PlayOneShot(RollAudioRef, transform.position);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void HitObjectAudio()
    {
        switch (GetComponentInParent<PlayerControllerV2>().WallHitType)
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
