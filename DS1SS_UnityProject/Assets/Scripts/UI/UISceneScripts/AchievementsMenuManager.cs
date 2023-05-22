using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AchievementsMenuManager : MonoBehaviour
{
    [Header("Bools")]
    public bool IsUnlockedAchievment1;
    public bool IsUnlockedAchievment2;
    public bool IsUnlockedAchievment3;
    public bool IsUnlockedAchievment4;
    public bool IsUnlockedAchievment5;
    public bool IsUnlockedAchievment6;
    public bool IsUnlockedAchievment7;
    public bool IsUnlockedAchievment8;

    [Header("Unlocked")]
    public GameObject AchievementUnlocked1;
    public GameObject AchievementUnlocked2;
    public GameObject AchievementUnlocked3;
    public GameObject AchievementUnlocked4;
    public GameObject AchievementUnlocked5;
    public GameObject AchievementUnlocked6;
    public GameObject AchievementUnlocked7;
    public GameObject AchievementUnlocked8;

    [Header("Locked")]
    public GameObject AchievementLocked1;
    public GameObject AchievementLocked2;
    public GameObject AchievementLocked3;
    public GameObject AchievementLocked4;
    public GameObject AchievementLocked5;
    public GameObject AchievementLocked6;
    public GameObject AchievementLocked7;
    public GameObject AchievementLocked8;


    void Start()
    {
        AchievementUnlocked1.SetActive(IsUnlockedAchievment1);
        AchievementUnlocked2.SetActive(IsUnlockedAchievment2);
        AchievementUnlocked3.SetActive(IsUnlockedAchievment3);
        AchievementUnlocked4.SetActive(IsUnlockedAchievment4);
        AchievementUnlocked5.SetActive(IsUnlockedAchievment5);
        AchievementUnlocked6.SetActive(IsUnlockedAchievment6);
        AchievementUnlocked7.SetActive(IsUnlockedAchievment7);
        AchievementUnlocked8.SetActive(IsUnlockedAchievment8);

        AchievementLocked1.SetActive(!IsUnlockedAchievment1);
        AchievementLocked2.SetActive(!IsUnlockedAchievment2);
        AchievementLocked3.SetActive(!IsUnlockedAchievment3);
        AchievementLocked4.SetActive(!IsUnlockedAchievment4);
        AchievementLocked5.SetActive(!IsUnlockedAchievment5);
        AchievementLocked6.SetActive(!IsUnlockedAchievment6);
        AchievementLocked7.SetActive(!IsUnlockedAchievment7);
        AchievementLocked8.SetActive(!IsUnlockedAchievment8);
    }


}
