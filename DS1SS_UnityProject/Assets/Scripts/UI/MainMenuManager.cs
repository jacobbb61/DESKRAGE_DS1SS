using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private bool CanInput = true;

    public Animator SceneTransitionAnim;
    public RectTransform HightlightPos;
    public Animator HightlightAnim;

    public int MainOrder = 1;
    public string ActiveMenu = "Main";

    public void Up(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            MainOrder--;
            MoveMainHighlight();
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            MainOrder++;
            MoveMainHighlight();
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            Debug.Log("Button Pressed");
            CanInput = false;
            if (ActiveMenu == "Main") { SelectMain(); }
        }
    }

    public void MoveMainHighlight()
    {
        switch (MainOrder)
        {
            case 0:
                MainOrder = 5; MoveMainHighlight();
                break;

            case 1: 
                HightlightPos.anchoredPosition = new Vector2(0, -50);
                break;

            case 2:
                HightlightPos.anchoredPosition = new Vector2(0, -80);
                break;

            case 3:
                HightlightPos.anchoredPosition = new Vector2(0, -110);
                break;

            case 4:
                HightlightPos.anchoredPosition = new Vector2(0, -140);
                break;

            case 5:
                HightlightPos.anchoredPosition = new Vector2(0, -170);
                break;

            case 6:
                MainOrder = 1; MoveMainHighlight();
                break;
        }
        CanInput = true;
    }

    public void SelectMain()
    {
        HightlightAnim.SetTrigger("Active");
        switch (MainOrder)
        {
            case 1:
                CanInput = true;
                break;

            case 2:
                CanInput = true;
                break;

            case 3:
                CanInput = true;
                break;

            case 4:
                StartCoroutine(LoadCredits());
                break;

            case 5:
                StartCoroutine(QuitGame());
                break;
        }
       
    }

    IEnumerator LoadCredits()
    {
        Debug.Log("Loading End Credits");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("End Credits", LoadSceneMode.Single);
    }
    IEnumerator QuitGame()
    {
        Debug.Log("Quiting Game");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
