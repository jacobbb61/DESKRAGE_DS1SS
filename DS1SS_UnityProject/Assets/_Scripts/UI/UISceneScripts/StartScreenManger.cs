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
    private bool CanInput = true;
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

    IEnumerator LoadMainMenu()
    {
        StartSceneAnim.SetTrigger("Active");
        FMODUnity.RuntimeManager.PlayOneShot(PressGameStart);
        Debug.Log("Loading");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}


