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
    private int MainOrder = 1;

    [Header("New Game Message")]
    public GameObject NewGameMessage;
    public RectTransform NGHightlightPos;
    public Animator NGHightlightAnim;
    private bool NGOrder;

    [Header("Load Game")]
    public GameObject LoadGameText;
    public GameObject LoadGameTextGrey;
    public bool HasGameSave;

    [Header("Settings")]
    public GameObject Settings; 
    public RectTransform SettingsHightlightPos;
    public Animator SettingsExitAnim;
    private bool SettingsOpen = false;
    private int SettingsOrder;
    public bool HUDActive;
    public bool SubtitlesActive;
    public int AudioMasterNum;
    public int AudioEffectsNum;
    public int AudioAmbienceNum;
    public int AudioMusicNum;
    public int AudioDialogNum;
    public TextMeshProUGUI HUDText;
    public TextMeshProUGUI SubtitlesText;
    public TextMeshProUGUI AudioMasterText;
    public TextMeshProUGUI AudioEffectsText;
    public TextMeshProUGUI AudioAmbienceText;
    public TextMeshProUGUI AudioMusicText;
    public TextMeshProUGUI AudioDialogText;

    [Header("Achievements")]
    public GameObject Achievements;
    public Animator AchievementsExitAnim;
    private bool AchievementsOpen=false;

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

        SettingsOpen = false;
        Settings.SetActive(false);

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
                case "Settings":
                    SettingsOrder--;
                    MoveSettingsHighlight();
                    break;
                default:
                    CanInput = true;
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
                case "Settings":
                    SettingsOrder++;
                    MoveSettingsHighlight();
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
                case "Settings":
                    UpdateSettings(true);
                    break;
                default:
                    CanInput = true;
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
                case "Settings":
                    UpdateSettings(false);
                    break;
                default:
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
    public void MoveSettingsHighlight()
    {
        switch (SettingsOrder)
        {
            case 0:
                SettingsOrder = 7; MoveSettingsHighlight();
                break;

            case 1:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, 80);
                break;

            case 2:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, 55);
                break;

            case 3:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, -20);
                break;

            case 4:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, -50);
                break;

            case 5:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, -80);
                break;

            case 6:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, -110);
                break;

            case 7:
                SettingsHightlightPos.anchoredPosition = new Vector2(-50, -140);
                break;

            case 8:
                SettingsOrder = 1; MoveSettingsHighlight();
                break;
        }
        CanInput = true;
    }


    public void UpdateSettings(bool Left)
    {
        switch (SettingsOrder)
        {
            case 1:
                HUDActive = !HUDActive;
                break;
            case 2:
                SubtitlesActive = !SubtitlesActive;
                break;
            case 3:
                if (Left) { AudioMasterNum--; } else { AudioMasterNum++; } ;
                break;
            case 4:
                if (Left) { AudioEffectsNum--; } else { AudioEffectsNum++; };
                break;
            case 5:
                if (Left) { AudioAmbienceNum--; } else { AudioAmbienceNum++; };
                break;
            case 6:
                if (Left) { AudioMusicNum--; } else { AudioMusicNum++; };
                break;
            case 7:
                if (Left) { AudioDialogNum--; } else { AudioDialogNum++; };
                break;
            default:
                CanInput = true;
                break;
        }
        AudioMasterNum = Mathf.Clamp(AudioMasterNum, 0, 10);
        AudioEffectsNum = Mathf.Clamp(AudioEffectsNum, 0, 10);
        AudioAmbienceNum = Mathf.Clamp(AudioAmbienceNum, 0, 10);
        AudioMusicNum = Mathf.Clamp(AudioMusicNum, 0, 10);
        AudioDialogNum = Mathf.Clamp(AudioDialogNum, 0, 10);

        CanInput = true;
        UpdateSettingsText();
    }
 
    public void UpdateSettingsText()
    {
        if (HUDActive) { HUDText.text = "ON"; } else { HUDText.text = "OFF"; }
        if (SubtitlesActive) { SubtitlesText.text = "ON"; } else { SubtitlesText.text = "OFF"; }
        AudioMasterText.text = AudioMasterNum.ToString();
        AudioEffectsText.text = AudioEffectsNum.ToString();
        AudioAmbienceText.text = AudioAmbienceNum.ToString();
        AudioMusicText.text = AudioMusicNum.ToString();
        AudioDialogText.text = AudioDialogNum.ToString();
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
        UpdateSettingsText();
        if (!SettingsOpen) { Settings.SetActive(true); } else { Settings.SetActive(false); }
        SettingsOrder = 1;
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
