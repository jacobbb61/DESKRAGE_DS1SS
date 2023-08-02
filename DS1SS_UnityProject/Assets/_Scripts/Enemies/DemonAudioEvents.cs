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
}

