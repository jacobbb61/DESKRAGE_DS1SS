using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one aduio manager in the scene");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference Sound, Vector3 WorldPos)
    {
        RuntimeManager.PlayOneShot(Sound, WorldPos);
    }

}
