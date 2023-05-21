using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class StartScreenManger : MonoBehaviour
{
    private bool CanInput = true;
    public Animator StartSceneAnim;

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
        Debug.Log("Loading");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}


