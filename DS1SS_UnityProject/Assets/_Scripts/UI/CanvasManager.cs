using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public Slider PlayerStaminaSlider;
    public Slider PlayerHealthSlider;

    public GameObject DoorPrompt;
    public GameObject FogDoorPrompt;
    public GameObject DoorUI;
    public GameObject AchievementObj;
    public GameObject ItemPrompt;
    public GameObject TutorialPrompt;
    public GameObject TutorialMessage;

    public TextMeshProUGUI DoorDescription;
    public TextMeshProUGUI ItemProptDescription;
    public TextMeshProUGUI EstusCountText;
    public TextMeshProUGUI TutorialText;

    public Animator BonfireAnim;
    public Animator YouDiedAnim;
    public Animator SceneTransitionAnim;

}
