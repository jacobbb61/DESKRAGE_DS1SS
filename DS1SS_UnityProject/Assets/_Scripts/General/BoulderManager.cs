using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BoulderManager : MonoBehaviour
{
    public GameObject Assets;
    public Animator Anim;
    public DoorManager DM;

    public bool BoulderUsed;

    public EventReference Boulder0Audio;
    public EventReference Boulder1Audio;
    public EventReference Boulder2Audio;
    private void Start()
    {
        if (BoulderUsed) { Assets.SetActive(false); GetComponent<BoxCollider2D>().enabled = false; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Send();   
        }
    }

    void Send() 
    {
        BoulderUsed = true;
        Anim.Play("BoulderActive");
        StartCoroutine(Roll());
    }
    IEnumerator Roll()
    {
   
        yield return new WaitForSeconds(2f); 
        
        RuntimeManager.PlayOneShot(Boulder0Audio);

        yield return new WaitForSeconds(0.5f);

        RuntimeManager.PlayOneShot(Boulder1Audio);

        yield return new WaitForSeconds(0.5f);

        RuntimeManager.PlayOneShot(Boulder2Audio);

        yield return new WaitForSeconds(0.5f);
        Assets.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
        DM.CurrentDoorState_This = "Closed";
        DM.UseDoor();
    }
}
