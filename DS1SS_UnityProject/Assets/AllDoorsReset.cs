using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDoorsReset : MonoBehaviour
{

    public DoorManager[] DoorManagers;
    public DoorManager DoorManagerM1;
    public AsylumDemonArena AsylumDemonArena;

    private void Start()
    {

        DoorManagers = GetComponentsInChildren<DoorManager>();
    }


    private void OnEnable()
    {      
        

        DoorManagers = GetComponentsInChildren<DoorManager>();
        foreach (DoorManager DoorManager in DoorManagers)
        {
            DoorManager.ManualStart();
        }


        if (AsylumDemonArena.currentState == "Idle" || AsylumDemonArena.currentState == "Active")
        {
            DoorManagerM1.ParticleSystem.Play();
            StartCoroutine(Wait(0.5f));
        }
    }
    private IEnumerator Wait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        DoorManagerM1.ParticleSystem.Play();

    }
}
