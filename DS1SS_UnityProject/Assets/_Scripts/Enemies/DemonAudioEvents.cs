using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DemonAudioEvents : MonoBehaviour
{

    public EventReference Audio_Land_Ref;
    public EventReference Audio_Step_Ref;
    public EventReference Audio_Swing1_Ref;
    public EventReference Audio_Swing2_Ref;
    public EventReference Audio_Swing3_Ref;
    public EventReference Audio_ClubLand_Ref;
    public EventReference Audio_WingFlap_Ref;

    public EventReference Audio_Roar1_Ref;
    public EventReference Audio_Roar2_Ref;
    public EventReference Audio_Growl_Ref;
    public EventReference Audio_GrowlLong_Ref;
    public EventReference Audio_FallLand_Ref;
    public EventReference Audio_Crash1_Ref;
    public EventReference Audio_Crash2_Ref;
    public EventReference Audio_CrashWeapon_Ref;
    public EventReference Audio_Death_Ref;



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Audio_Land()
    {
        RuntimeManager.PlayOneShot(Audio_Land_Ref, transform.position);
    }
    public void Audio_Step()
    {
        RuntimeManager.PlayOneShot(Audio_Step_Ref, transform.position);
    }
    public void Audio_Swing1()
    {
        RuntimeManager.PlayOneShot(Audio_Swing1_Ref, transform.position);
    }
    public void Audio_Swing2()
    {
        RuntimeManager.PlayOneShot(Audio_Swing2_Ref, transform.position);
    }
    public void Audio_Swing3()
    {
        RuntimeManager.PlayOneShot(Audio_Swing3_Ref, transform.position);
    }
    public void Audio_ClubLand()
    {
        RuntimeManager.PlayOneShot(Audio_ClubLand_Ref, transform.position);
    }
    public void Audio_WingFlap()
    {
        RuntimeManager.PlayOneShot(Audio_WingFlap_Ref, transform.position);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Audio_Roar1()
    {
        RuntimeManager.PlayOneShot(Audio_Roar1_Ref, transform.position);
    }
    public void Audio_Roar2()
    {
        RuntimeManager.PlayOneShot(Audio_Roar2_Ref, transform.position);
    }
    public void Audio_Growl()
    {
        RuntimeManager.PlayOneShot(Audio_Growl_Ref, transform.position);
    }
    public void Audio_GrowlLong()
    {
        RuntimeManager.PlayOneShot(Audio_GrowlLong_Ref, transform.position);
    }
    public void Audio_FallLand()
    {
        RuntimeManager.PlayOneShot(Audio_FallLand_Ref, transform.position);
    }
    public void Audio_Crash1()
    {
        RuntimeManager.PlayOneShot(Audio_Crash1_Ref, transform.position);
    }
    public void Audio_Crash2()
    {
        RuntimeManager.PlayOneShot(Audio_Crash2_Ref, transform.position);
    }

    public void Audio_CrashWeapon()
    {
        RuntimeManager.PlayOneShot(Audio_CrashWeapon_Ref, transform.position);
    }

    public void Audio_Death()
    {
        RuntimeManager.PlayOneShot(Audio_Death_Ref, transform.position);
    }
















}

