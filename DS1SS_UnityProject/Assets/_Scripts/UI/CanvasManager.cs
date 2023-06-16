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
    public GameObject DoorUI;
    public GameObject AchievementObj;
    public GameObject ItemPrompt;

    public TextMeshProUGUI DoorDescription;
    public TextMeshProUGUI ItemProptDescription;

    public Animator BonfireAnim;
    public Animator YouDiedAnim;

}
