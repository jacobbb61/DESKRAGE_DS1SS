using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 

public class AnimationAudio : MonoBehaviour
{
    public EventReference Audio1Ref;
    public EventReference Audio2Ref;
    public EventReference Audio3Ref;
    public EventReference Audio4Ref;
    public EventReference Audio5Ref;
    public EventReference Audio6Ref;
    public EventReference Audio7Ref;
    public EventReference Audio8Ref;
    public EventReference Audio9Ref;


    public void Audio1()
    {
        RuntimeManager.PlayOneShot(Audio1Ref, transform.position);
    }
    public void Audio2()
    {
        RuntimeManager.PlayOneShot(Audio2Ref, transform.position);
    }
    public void Audio3()
    {
        RuntimeManager.PlayOneShot(Audio3Ref, transform.position);
    }
    public void Audio4()
    {
        RuntimeManager.PlayOneShot(Audio4Ref, transform.position);
    }
    public void Audio5()
    {
        RuntimeManager.PlayOneShot(Audio5Ref, transform.position);
    }
    public void Audio6()
    {
        RuntimeManager.PlayOneShot(Audio6Ref, transform.position);
    }
    public void Audio7()
    {
        RuntimeManager.PlayOneShot(Audio7Ref, transform.position);
    }
    public void Audio8()
    {
        RuntimeManager.PlayOneShot(Audio8Ref, transform.position);
    }
    public void Audio9()
    {
        RuntimeManager.PlayOneShot(Audio9Ref, transform.position);
    }
}
