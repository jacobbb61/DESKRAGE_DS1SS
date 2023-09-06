using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Slider PlayerStaminaSlider;
    public Slider PlayerHealthSlider;
    public Slider PlayerStaminaCatchupSlider;
    public Slider PlayerHealthCatchupSlider;

    public GameObject DoorPrompt;
    public GameObject FogDoorPrompt;
    public GameObject DoorUI;
    public GameObject AchievementObj;
    public GameObject ItemPrompt;
    public GameObject InteractPrompt;
    public GameObject TutorialPrompt;
    public GameObject TutorialMessage;

    public TextMeshProUGUI DoorDescription;
    public TextMeshProUGUI ItemProptDescription;
    public TextMeshProUGUI InteractProptDescription;
    public TextMeshProUGUI EstusCountText;
    public TextMeshProUGUI TutorialText;

    public Animator BonfireAnim;
    public Animator YouDiedAnim;
    public Animator SceneTransitionAnim;

    [Header("HUD")]
    public GameObject HUD_1;
    public GameObject HUD_2;
    public GameObject HUD_3;
    public GameObject HUD_4;


    private void Start()
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build"))
        {
            HUD_1.SetActive(true);
            HUD_2.SetActive(true);
            HUD_3.SetActive(true);
            HUD_4.SetActive(true);
            if (GameSaveGameManager.Instance.GameSaveData.HUD == false)
            {
                ToggleHUD();
            }
        }
    }

    public void ToggleHUD()
    {
        HUD_1.SetActive(!HUD_1.activeInHierarchy);
        HUD_2.SetActive(!HUD_2.activeInHierarchy);
        HUD_3.SetActive(!HUD_3.activeInHierarchy);
        HUD_4.SetActive(!HUD_4.activeInHierarchy);
    }

}
