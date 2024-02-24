using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOcludingSection : MonoBehaviour
{
    public string DoorParentTag;
    public GameObject Cover;
    public Animator Anim;


    public void ManualStart()
    {
        switch (DoorParentTag)
        {
            case "N":
                if (GetComponent<DoorManager>().CurrentDoorState_This == "Open")
                {
                    Open();
                }
                break;
            case "O":
                if (GetComponent<CollapseBridge>().currentState == "Broken")
                {
                    Open();
                }
                break;
            case "H":
                if (GetComponent<DoorManager>().CurrentDoorState_This == "Open")
                {
                    Open();
                }
                break;
        }
    }
    public void RevealArea()
    {
        StartCoroutine(Reveal());
    }

    IEnumerator Reveal()
    {
        Anim.Play("AreaOcclusionReveal");

        yield return new WaitForSeconds(2.5f);
        Open();
    }

    public void Open()
    {
        Cover.SetActive(false);
    }
}
