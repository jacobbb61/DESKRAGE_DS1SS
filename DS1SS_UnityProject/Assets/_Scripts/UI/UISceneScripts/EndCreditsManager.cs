using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;

public class EndCreditsManager : MonoBehaviour
{
    public bool CanInput = true;
    public bool CanSkip = false;
    public Animator SkipPromptAnim;
    public Animator SceneTransitionAnim;

    public EventReference EndCreditsSong;
    public EventReference PressCancel;
    public FMOD.Studio.EventInstance FMODinstance;

    void Start()
    {
        StartCoroutine(EndCredits());

        FMODinstance = FMODUnity.RuntimeManager.CreateInstance(EndCreditsSong);
        FMODinstance.start();
        FMODinstance.release();
    }

    IEnumerator EndCredits()
    {
        yield return new WaitForSeconds(200f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && CanSkip == false)
        {
            Debug.Log("Button Pressed");
            CanInput = false;
            StartCoroutine(OpenPrompt());
        }
    }
    public void Y(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && CanSkip==true)
        {
            Debug.Log("Button Pressed");
            CanInput = false;
            StartCoroutine(SkipCredits());
        }
    }

    IEnumerator OpenPrompt()
    {
        SkipPromptAnim.SetTrigger("On");     
        yield return new WaitForSeconds(0.5f);
        CanInput = true;
        CanSkip = true;
        yield return new WaitForSeconds(5f);
        CanSkip = false;
    }



    IEnumerator SkipCredits()
    {
        FMODinstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODinstance.release();
        SkipPromptAnim.SetTrigger("Active");
        SceneTransitionAnim.SetTrigger("Active");
        FMODUnity.RuntimeManager.PlayOneShot(PressCancel);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);

    }
}
