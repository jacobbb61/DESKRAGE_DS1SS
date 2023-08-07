using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class PusuerAduioEvents : MonoBehaviour
{

    public EventReference Audio_Land_Ref;
    public EventReference Audio_Bash_Ref;




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

}


