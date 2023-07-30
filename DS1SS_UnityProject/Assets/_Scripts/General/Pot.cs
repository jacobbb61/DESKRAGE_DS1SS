using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public Animator Anim;
    public bool Broken;



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
        gameObject.SetActive(true);
        Anim.Play("Pot_Idle");
        Broken = false;
 
    }
    public void BreakPot()
    {
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
