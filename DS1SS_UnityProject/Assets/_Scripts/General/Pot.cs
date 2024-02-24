using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Pot : MonoBehaviour
{
    public Animator Anim;
    public bool Broken;
    public Collider2D Collider;
    public EventReference PotAudioRef;

    public Key key;

    private void OnEnable()
    {
        if (Broken) { gameObject.SetActive(false); } 
        else
        {
            ResetPot();
        }
    }

    public void ResetPot()
    {
        Collider.enabled = true;
        gameObject.SetActive(true);
        Anim.Play("Pot_Idle");
        Broken = false;
 
    }
    public void BreakPot()
    {
        if (key != null && WorldSaveGameManager.Instance.Player.NKey==false)
        {
            key.ShowPopUp = true;
            key.ShowPop();
        }

        RuntimeManager.PlayOneShot(PotAudioRef, transform.position);
        Collider.enabled = false;
        Anim.SetTrigger("Break");
        Broken = true;
        StartCoroutine(WaitToTurnOff());
    }

    IEnumerator WaitToTurnOff()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
