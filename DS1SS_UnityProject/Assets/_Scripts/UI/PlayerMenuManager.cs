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
    public PlayerManager PM;
    public PlayerControllerV2 PC;
    public CanvasManager CanvasManager;
    public AsylumDemonArena AsylumDemonArena;
    public PursuerArena PursuerArena;

    public EventReference PressSelect;
    public EventReference PressMove;
    public EventReference PressOk;
    public EventReference PressCancel;
    public FMOD.Studio.EventInstance FMODinstance;

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
    public TextMeshProUGUI TimePlayedText;


    [Header("Settings")]
    public InputDeviceManager InputDeviceManager;
    public GameObject Settings;
    public GameObject Subtitles;
    public RectTransform SettingsHightlightPos;
    private bool SettingsOpen = false;
    private int SettingsOrder;
    public bool HUDActive;
    public bool SubtitlesActive;
    public string ControlLayout;

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
    public TextMeshProUGUI ControlsText;
    public TextMeshProUGUI AudioMasterText;
    public TextMeshProUGUI AudioEffectsText;
    public TextMeshProUGUI AudioAmbienceText;
    public TextMeshProUGUI AudioMusicText;
    public TextMeshProUGUI AudioDialogText;





    private void Start()
    {
        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

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

        UpdateSettings();
        UpdateFMODSettings();
    }


    public void Open(InputAction.CallbackContext context)
    {
        if (PC.State == "Idle" || PC.State == "Walking" || PC.State == "Running" || PC.State == "MenuOpen")
        {
            if (context.action.triggered && CanInput == true)
            {
                GameSaveGameManager.Instance.SaveSettings(HUDActive, SubtitlesActive,ControlLayout, AudioMasterNum, AudioEffectsNum, AudioAmbienceNum, AudioMusicNum, AudioDialogNum);
                Debug.Log("Start Button Pressed");
                CanInput = false;
                PC.State = "MenuOpen";
                LoadMain();
            }
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
                    PlayAudioPressMove();
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
        if (context.action.triggered && CanInput == true && MainOpen)
        {
       
            CanInput = false;
            Debug.Log("Down Button Pressed");
            switch (ActiveMenu)
            {
                case "Settings":
                    SettingsOrder++;
                    PlayAudioPressMove();
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
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            CanInput = false;
            Debug.Log("Left Button Pressed");
            switch (ActiveMenu)
            {
                case "Main":
                    MainOrder--;
                    MoveMainHighlight();
                    PlayAudioPressMove();
                    break;
                case "Quit":
                    QuitOrder--;
                    MoveQuitHighlight();
                    PlayAudioPressMove();
                    break;
                case "Settings":
                    ChangeSettings(true);
                    PlayAudioPressSelect();
                    break;
                default:
                    CanInput = true;
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
                    PlayAudioPressMove();
                    break;
                case "Quit":
                    QuitOrder++;
                    MoveQuitHighlight();
                    PlayAudioPressMove();
                    break;
                case "Settings":
                    ChangeSettings(false);                  
                    PlayAudioPressSelect();
                    break;
                default:
                    CanInput = true;
                    break;
            }
        }
    }

    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && MainOpen)
        {
            PlayAudioPressOk();
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
            PlayAudioPressCancel();
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
                    GameSaveGameManager.Instance.SaveSettings(HUDActive, SubtitlesActive,ControlLayout, AudioMasterNum, AudioEffectsNum, AudioAmbienceNum, AudioMusicNum, AudioDialogNum);
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
        if (!MainOpen) { ActiveMenu = "Main"; MainUI.SetActive(true); PlayAudioPressOk(); PC.IsUiOpen = true; } else { ActiveMenu = "Closed"; MainUI.SetActive(false);
            QuitUI.SetActive(false);
            Settings.SetActive(false);
            PlayAudioPressCancel();
            PC.IsUiOpen = false;
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
        MoveSettingsHighlight();
    }
    public void MoveSettingsHighlight()
    {
        switch (SettingsOrder)
        {
            case 0:
                SettingsOrder = 8; MoveSettingsHighlight();
                break;

            case 1:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 105);
                break;

            case 2:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 80);
                break;

            case 3:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 55);
                break;

            case 4:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, 5);
                break;

            case 5:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -25);
                break;

            case 6:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -55);
                break;

            case 7:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -85);
                break;

            case 8:
                SettingsHightlightPos.anchoredPosition = new Vector2(-7, -115);
                break;

            case 9:
                SettingsOrder = 1; MoveSettingsHighlight();
                break;
        }
        CanInput = true;
    }
    public void UpdateSettings()
    {

        HUDActive = GameSaveGameManager.Instance.GameSaveData.HUD;
        SubtitlesActive = GameSaveGameManager.Instance.GameSaveData.Subtitles;
        ControlLayout = GameSaveGameManager.Instance.GameSaveData.Controls;
        if (ControlLayout == "") { ControlLayout = "Auto"; }
        if (ControlLayout == null) { ControlLayout = "Auto"; }

        AudioMasterNum = GameSaveGameManager.Instance.GameSaveData.Master;
        AudioEffectsNum = GameSaveGameManager.Instance.GameSaveData.Effects;
        AudioAmbienceNum = GameSaveGameManager.Instance.GameSaveData.Ambience;
        AudioMusicNum = GameSaveGameManager.Instance.GameSaveData.Music;
        AudioDialogNum = GameSaveGameManager.Instance.GameSaveData.Dialog;

        Debug.Log("Updated settigns");
    }
    public void ChangeSettings(bool Left)
    {
        switch (SettingsOrder)
        {
            case 1:
                HUDActive = !HUDActive;
                CanvasManager.ToggleHUD();
                break;
            case 2:
                SubtitlesActive = !SubtitlesActive;
                Subtitles.SetActive(!Subtitles.activeInHierarchy);
                break;


            case 3:
                if (Left)
                {
                    if (ControlLayout == "Keyboard") { ControlLayout = "Auto"; InputDeviceManager.SwitchToAuto(); break; }
                    if (ControlLayout == "Xbox") { ControlLayout = "Keyboard"; InputDeviceManager.SwitchToPC(); break; }
                    if (ControlLayout == "PlayStation") { ControlLayout = "Xbox"; InputDeviceManager.SwitchToXbox(); break; }
                    if (ControlLayout == "Auto") { ControlLayout = "PlayStation"; InputDeviceManager.SwitchToPS(); break; }
                }
                else
                {
                    if (ControlLayout == "PlayStation") { ControlLayout = "Auto"; InputDeviceManager.SwitchToAuto(); break; }
                    if (ControlLayout == "Xbox") { ControlLayout = "PlayStation"; InputDeviceManager.SwitchToPS(); break; }
                    if (ControlLayout == "Keyboard") { ControlLayout = "Xbox"; InputDeviceManager.SwitchToXbox(); break; }
                    if (ControlLayout == "Auto") { ControlLayout = "Keyboard"; InputDeviceManager.SwitchToPC(); break; }
                }
                break;

            case 4:
                if (Left) { AudioMasterNum--; } else { AudioMasterNum++; };
                break;
            case 5:
                if (Left) { AudioEffectsNum--; } else { AudioEffectsNum++; };
                break;
            case 6:
                if (Left) { AudioAmbienceNum--; } else { AudioAmbienceNum++; };
                break;
            case 7:
                if (Left) { AudioMusicNum--; } else { AudioMusicNum++; };
                break;
            case 8:
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
        ControlsText.text = ControlLayout;
        AudioMasterText.text = AudioMasterNum.ToString();
        AudioEffectsText.text = AudioEffectsNum.ToString();
        AudioAmbienceText.text = AudioAmbienceNum.ToString();
        AudioMusicText.text = AudioMusicNum.ToString();
        AudioDialogText.text = AudioDialogNum.ToString();
    }



    public void LoadQuit()
    {

       
        float timer = PM.TimePlayedSeconds;
        float seconds = timer % 60;
        float minutes = Mathf.Floor(timer / 60);
        float hours = Mathf.Floor(minutes / 60);
        TimePlayedText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

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
                StartCoroutine(Quiting());
                break;

            case 2:
                LoadQuit();
                break;

        }
        CanInput = true;
    }

    IEnumerator Quiting()
    {
        if (PM.DemonArena.inBossFight==false && PM.PursuerArena.inBossFight==false) // or pursuer active
        {
        WorldSaveGameManager.Instance.Player = PM;
        WorldSaveGameManager.Instance.SaveGame();
        }
        else
        {
            PM.DemonArena.StopMusic();
            PM.PursuerArena.StopMusic();
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }



    public void PlayAudioPressSelect()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PressSelect);
    }
    public void PlayAudioPressMove()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PressMove);
    }
    public void PlayAudioPressOk()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PressOk);
    }
    public void PlayAudioPressCancel()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PressCancel);
    }
}
