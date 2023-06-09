using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PlayerMenuManager : MonoBehaviour
{
    // this script will effect player movement and combat, while the menu ui is open the player can only walk, they cannot input for a roll or jump or attack as those buttons are used for the ui
    
    //if MainOpen is true, then player cannot do above


    public bool CanInput = true;
    public string ActiveMenu = "Main";


    [Header("Main")]
    public GameObject MainUI;
    public RectTransform MainHightlightPos;
    private int MainOrder = 1;
    private bool MainOpen;


    [Header("Quit")]
    public GameObject QuitUI;
    public RectTransform QuitHightlightPos;
    private int QuitOrder = 1;
    private bool QuitOpen;


    [Header("Settings")]
    public GameObject Settings;
    public RectTransform SettingsHightlightPos;
    private bool SettingsOpen = false;
    private int SettingsOrder;
    public bool HUDActive;
    public bool SubtitlesActive;

    public FMOD.Studio.VCA VcaMasterController;
    public FMOD.Studio.VCA VcaEffectsController;
    public FMOD.Studio.VCA VcaAmbienceController;
    public FMOD.Studio.VCA VcaMusicController;
    public FMOD.Studio.VCA VcaDialogueController;

    public float AudioMasterNum;
    public float AudioEffectsNum;
    public float AudioAmbienceNum;
    public float AudioMusicNum;
    public float AudioDialogNum;

    public TextMeshProUGUI HUDText;
    public TextMeshProUGUI SubtitlesText;
    public TextMeshProUGUI AudioMasterText;
    public TextMeshProUGUI AudioEffectsText;
    public TextMeshProUGUI AudioAmbienceText;
    public TextMeshProUGUI AudioMusicText;
    public TextMeshProUGUI AudioDialogText;





    private void Start()
    {

        MainOpen = false;
        MainUI.SetActive(false);
        ActiveMenu = "Closed";
        MainOrder = 1;
        MoveMainHighlight();

        SettingsOpen = false;
        Settings.SetActive(false);


        VcaMasterController = FMODUnity.RuntimeManager.GetVCA("vca:/MasterVCA");
        VcaEffectsController = FMODUnity.RuntimeManager.GetVCA("vca:/EffectsVCA");
        VcaAmbienceController = FMODUnity.RuntimeManager.GetVCA("vca:/AmbienceVCA");
        VcaMusicController = FMODUnity.RuntimeManager.GetVCA("vca:/MusicVCA");
        VcaDialogueController = FMODUnity.RuntimeManager.GetVCA("vca:/DialogueVCA");
    }


    public void Open(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true)
        {
            Debug.Log("Start Button Pressed");
            CanInput = false;
            LoadMain();
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            CanInput = false;
            Debug.Log("Up Button Pressed");
            switch (ActiveMenu)
            {
                case "Settings":
                    SettingsOrder--;
                    MoveSettingsHighlight();
                    break;
            }
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            CanInput = false;
            Debug.Log("Down Button Pressed");
            switch (ActiveMenu)
            {
                case "Settings":
                    SettingsOrder++;
                    MoveSettingsHighlight();
                    break;
            }
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            CanInput = false;
            Debug.Log("Left Button Pressed");
            switch (ActiveMenu)
            {
                case "Main":
                    MainOrder--;
                    MoveMainHighlight();
                    break;
                case "Quit":
                    QuitOrder--;
                    MoveQuitHighlight();
                    break;
                case "Settings":
                    UpdateSettings(true);
                    break;
            }
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            CanInput = false;
            Debug.Log("Right Button Pressed");
            switch (ActiveMenu)
            {
                case "Main":
                    MainOrder++;
                    MoveMainHighlight();
                    break;
                case "Quit":
                    QuitOrder++;
                    MoveQuitHighlight();
                    break;
                case "Settings":
                    UpdateSettings(false);
                    break;
            }
        }
    }

    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            Debug.Log("A Button Pressed");
            CanInput = false;
            switch (ActiveMenu)
            {
                case "Main":
                    SelectMain();
                    break;
                case "Quit":
                    SelectQuit();
                    break;
                default:
                    CanInput = true;
                    break;
            }
        }
    }
    public void B(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            Debug.Log("B Button Pressed");
            CanInput = false;
            switch (ActiveMenu)
            {
                case "Main":
                    LoadMain();
                    break;
                case "Quit":
                    LoadQuit();
                    break;
                case "Settings":
                    LoadSettings();
                    break;
                default:
                    CanInput = true;
                    break;
            }
        }
    }

    public void LoadMain()
    {
        if (!MainOpen) { ActiveMenu = "Main"; MainUI.SetActive(true); } else { ActiveMenu = "Closed"; MainUI.SetActive(false);
            QuitUI.SetActive(false);
            Settings.SetActive(false);
        }
        MainOrder = 1;
        CanInput = true;
        MainOpen = !MainOpen;
        MoveMainHighlight();
    }
    public void MoveMainHighlight()
    {
        switch (MainOrder)
        {     
            case 1:
                MainHightlightPos.anchoredPosition = new Vector2(-30, 12);
                break;
            case 2:
                MainHightlightPos.anchoredPosition = new Vector2(30, 12);
                break;
            case 3:
                MainOrder = 1; MoveMainHighlight();
                break;
            case 0:
                MainOrder = 2; MoveMainHighlight();
                break;
        }
        CanInput = true;
    }
    public void SelectMain()
    {
        switch (MainOrder)
        {
            case 1:
                LoadSettings();
                break;

            case 2:
                LoadQuit();
                break;

        }
        CanInput = true;
    }



    public void LoadSettings()
    {
        if (!SettingsOpen) { ActiveMenu = "Settings"; } else { ActiveMenu = "Main"; }

        UpdateSettingsText();
        if (!SettingsOpen) { Settings.SetActive(true); } else { Settings.SetActive(false); }
        SettingsOrder = 1;
        CanInput = true;
        SettingsOpen = !SettingsOpen;
        MainOrder = 1;
    }
    public void MoveSettingsHighlight()
    {
        switch (SettingsOrder)
        {
            case 0:
                SettingsOrder = 7; MoveSettingsHighlight();
                break;

            case 1:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 105);
                break;

            case 2:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 80);
                break;

            case 3:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 5);
                break;

            case 4:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -25);
                break;

            case 5:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -55);
                break;

            case 6:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -85);
                break;

            case 7:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -115);
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
                if (Left) { AudioMasterNum--; } else { AudioMasterNum++; };
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
        UpdateFMODSettings();
    }
    public void UpdateFMODSettings()
    {
        VcaMasterController.setVolume(AudioMasterNum / 10);
        VcaEffectsController.setVolume(AudioEffectsNum / 10);
        VcaAmbienceController.setVolume(AudioAmbienceNum / 10);
        VcaMusicController.setVolume(AudioMusicNum / 10);
        VcaDialogueController.setVolume(AudioDialogNum / 10);
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



    public void LoadQuit()
    {
        if (!QuitOpen) { ActiveMenu = "Quit"; QuitUI.SetActive(true);} else { ActiveMenu = "Main"; QuitUI.SetActive(false); }
        QuitOrder = 2;
        MoveQuitHighlight();
        CanInput = true;
        QuitOpen = !QuitOpen;
        MainOrder = 2;
    }
    public void MoveQuitHighlight()
    {
        switch (QuitOrder)
        {
            case 1:
                QuitHightlightPos.anchoredPosition = new Vector2(-85, -10);
                break;

            case 2:
                QuitHightlightPos.anchoredPosition = new Vector2(85, -10);
                break;

            case 3:
                QuitOrder = 1; MoveQuitHighlight();
                break;
            case 0:
                QuitOrder = 2; MoveQuitHighlight();
                break;
        }
        CanInput = true;
    }
    public void SelectQuit()
    {
        switch (QuitOrder)
        {
            case 1:
                Debug.Log("Quit");
                break;

            case 2:
                LoadQuit();
                break;

        }
        CanInput = true;
    }

}