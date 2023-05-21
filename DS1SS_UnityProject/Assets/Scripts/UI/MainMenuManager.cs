using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public bool CanInput = true;

    public Animator SceneTransitionAnim;
    public string ActiveMenu = "Main";

    [Header("Main")]
    public RectTransform MainHightlightPos;
    public Animator MainHightlightAnim; 
    public int MainOrder = 1;

    [Header("New Game Message")]
    public GameObject NewGameMessage;
    public RectTransform NGHightlightPos;
    public Animator NGHightlightAnim;
    public bool NGOrder;

    [Header("Load Game")]
    public GameObject LoadGameText;
    public GameObject LoadGameTextGrey;
    public bool HasGameSave;

    [Header("Settings")]
    public GameObject Settings;
    public Animator SettingsExitAnim;
    public bool SettingsOpen = false;

    [Header("Achievements")]
    public GameObject Achievements;
    public Animator AchievementsExitAnim;
    public bool AchievementsOpen=false;

    private void Start()
    {
        ActiveMenu = "Main";
        MainOrder = 1;
        MoveMainHighlight();

        NewGameMessage.SetActive(false);
        NGOrder = false;
        MoveNGHighlight();

        LoadGameText.SetActive(HasGameSave);
        LoadGameTextGrey.SetActive(!HasGameSave);

        AchievementsOpen = false;
        Achievements.SetActive(false);
    }
    public void Up(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            Debug.Log("Up Button Pressed");
            switch (ActiveMenu)
            {
                case "Main":
                    MainOrder--;
                    MoveMainHighlight();
                    break;
           
            }
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            Debug.Log("Down Button Pressed");
            switch (ActiveMenu)
            {
                case "Main":
                    MainOrder++;
                    MoveMainHighlight();
                    break;
                default:
                    CanInput = true;
                    break;
            }
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            Debug.Log("Left Button Pressed");
            switch (ActiveMenu)
            {
                case "New Game Message":
                    NGOrder = true;
                    MoveNGHighlight();                  
                    break;
            }
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            CanInput = false;
            Debug.Log("Right Button Pressed");
            switch (ActiveMenu)
            {
                case "New Game Message":
                    NGOrder = false;
                    MoveNGHighlight();
                    CanInput = true;
                    break;
            }
        }
    }

    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            Debug.Log("A Button Pressed");
            CanInput = false;
            switch (ActiveMenu)
            { 
                
               case "New Game Message":
                    SelectNG();
                    break;

                case "Main":
                    SelectMain();                   
                    break;

               
            }
        }
    }
    public void B(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            Debug.Log("B Button Pressed");
            CanInput = false;
            switch (ActiveMenu)
            {
                case "New Game Message":
                    ActiveMenu = "Main";
                    NewGameMessage.SetActive(false);
                    NGOrder = false;
                    CanInput = true;
                    break;
                case "Achievements":
                    AchievementsExitAnim.SetTrigger("Active");
                    StartCoroutine(LoadAchievements());
                    break;
                case "Settings":
                    SettingsExitAnim.SetTrigger("Active");
                    StartCoroutine(LoadSettings());
                    break;
            }
        }
    }




    public void SelectMain()
    {
        MainHightlightAnim.SetTrigger("Active");
        switch (MainOrder)
        {
            case 1:
                if (HasGameSave)
                {
                    NewGameMessage.SetActive(true);
                    NGOrder = false;
                    MoveNGHighlight();
                    CanInput = true;
                    ActiveMenu = "New Game Message";
                }
                else { StartCoroutine(LoadNewGame()); }
                break;

            case 2:
                CanInput = true;
                break;

            case 3:
                StartCoroutine(LoadSettings());
                break;

            case 4:
                StartCoroutine(LoadAchievements());
                break;

            case 5:
               StartCoroutine(LoadCredits());
                break;

            case 6:
                StartCoroutine(QuitGame());
                break;
        }
       
    }
    public void SelectNG()
    {
        NGHightlightAnim.SetTrigger("Active");
        if (NGOrder)
        {
            StartCoroutine(LoadNewGame());
        }
        else
        {
            StartCoroutine(ExitNewGame());      
        }
    }



    public void MoveMainHighlight()
    {
        switch (MainOrder)
        {
            case 0:
                MainOrder = 6; MoveMainHighlight();
                break;

            case 1: 
                MainHightlightPos.anchoredPosition = new Vector2(0, -20);
                break;

            case 2:
                MainHightlightPos.anchoredPosition = new Vector2(0, -50);
                break;

            case 3:
                MainHightlightPos.anchoredPosition = new Vector2(0, -80);
                break;

            case 4:
                MainHightlightPos.anchoredPosition = new Vector2(0, -110);
                break;

            case 5:
                MainHightlightPos.anchoredPosition = new Vector2(0, -140);
                break;

            case 6:
                MainHightlightPos.anchoredPosition = new Vector2(0, -170);
                break;

            case 7:
                MainOrder = 1; MoveMainHighlight();
                break;
        }
        CanInput = true;
    }
    public void MoveNGHighlight()
    {

        if (NGOrder) 
        {
            NGHightlightPos.anchoredPosition = new Vector2(-80, -30);
        } 
        else
        {
            NGHightlightPos.anchoredPosition = new Vector2(80, -30);
        }

        CanInput = true;
    }


    IEnumerator LoadNewGame()
    {
        Debug.Log("Loading New Game");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.2f);
        //SceneManager.LoadScene("End Credits", LoadSceneMode.Single); Load new game scene
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    IEnumerator ExitNewGame()
    {     
        yield return new WaitForSeconds(0.25f);
        ActiveMenu = "Main";
        NewGameMessage.SetActive(false);
        NGOrder = false;
        CanInput = true;
    }
    IEnumerator LoadCredits()
    {
        Debug.Log("Loading End Credits");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("End Credits", LoadSceneMode.Single);
    }
    IEnumerator LoadAchievements()
    {
        if (!AchievementsOpen) { ActiveMenu = "Achievements"; } else { ActiveMenu = "Main"; }
        Debug.Log("Loading Achievements");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.2f);
        if (!AchievementsOpen) { Achievements.SetActive(true); } else { Achievements.SetActive(false); }
        CanInput = true;
        AchievementsOpen = !AchievementsOpen;
    }
    IEnumerator LoadSettings()
    {
        if (!SettingsOpen) { ActiveMenu = "Settings"; } else { ActiveMenu = "Main"; }
        Debug.Log("Loading Settings");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.2f);
        if (!SettingsOpen) { Settings.SetActive(true); } else { Settings.SetActive(false); }
        CanInput = true;
        SettingsOpen = !SettingsOpen;
    }
    IEnumerator QuitGame()
    {
        Debug.Log("Quiting Game");
        SceneTransitionAnim.SetTrigger("Active");
        yield return new WaitForSeconds(1.2f);
        Application.Quit();
    }
}
