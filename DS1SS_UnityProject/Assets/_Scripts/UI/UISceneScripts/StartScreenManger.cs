using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;
public class StartScreenManger : MonoBehaviour
{
    public bool CanInput = false;
    public Animator StartSceneAnim;

    public EventReference PressGameStart;

    public void GameStart(InputAction.CallbackContext context)
    {
        bool change = context.action.triggered;
        if (change  && CanInput==true)
        {
            Debug.Log("Start Pressed");
            StartCoroutine(LoadMainMenu());
            CanInput = false;
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(5f);
        CanInput = true;
    }

    IEnumerator LoadMainMenu()
    {
        StartSceneAnim.SetTrigger("Active");
        FMODUnity.RuntimeManager.PlayOneShot(PressGameStart);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}


