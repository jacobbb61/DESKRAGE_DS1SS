using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class PusuerAduioEvents : MonoBehaviour
{

    public EventReference Audio_Land_Ref;
    public EventReference Audio_Bash_Ref;



    public EventReference Audio_SwooshShort_Ref;
    public EventReference Audio_SwooshMedium_Ref;

    public EventReference Audio_Turn_Ref;
    public EventReference Audio_Movement_Ref;

    public EventReference Audio_Slam_Ref;





    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Audio_Land()
    {
        RuntimeManager.PlayOneShot(Audio_Land_Ref, transform.position);
    }
    public void Audio_Bash()
    {
        RuntimeManager.PlayOneShot(Audio_Bash_Ref, transform.position);
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Audio_SwooshShort()
    {
        RuntimeManager.PlayOneShot(Audio_SwooshShort_Ref, transform.position);
    }
    public void Audio_SwooshMedium()
    {
        RuntimeManager.PlayOneShot(Audio_SwooshMedium_Ref, transform.position);
    }
    public void Audio_Turn()
    {
        RuntimeManager.PlayOneShot(Audio_Turn_Ref, transform.position);
    }
    public void Audio_Movement()
    {
        RuntimeManager.PlayOneShot(Audio_Movement_Ref, transform.position);
    }
    public void Audio_Slam()
    {
        RuntimeManager.PlayOneShot(Audio_Slam_Ref, transform.position);
    }
}


