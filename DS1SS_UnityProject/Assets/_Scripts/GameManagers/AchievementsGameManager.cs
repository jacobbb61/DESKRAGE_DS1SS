using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsGameManager : MonoBehaviour
{
    public static AchievementsGameManager Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    // call the below function when an achievement is unlocked, for example on the asylum door to section 2 write "AchievementsGameManager.Instance.UnlockedAchievement(1);"
    public void UnlockedAchievement(int num)
    {
        switch (num)
        {
            case 1:
                GameSaveGameManager.Instance.GameSaveData.Achievement_1 = true;
                break;
            case 2:
                GameSaveGameManager.Instance.GameSaveData.Achievement_2 = true;
                break;
            case 3:
                GameSaveGameManager.Instance.GameSaveData.Achievement_3 = true;
                break;
            case 4:
                GameSaveGameManager.Instance.GameSaveData.Achievement_4 = true;
                break;
            case 5:
                GameSaveGameManager.Instance.GameSaveData.Achievement_5 = true;
                break;
            case 6:
                GameSaveGameManager.Instance.GameSaveData.Achievement_6 = true;
                break;
            case 7:
                GameSaveGameManager.Instance.GameSaveData.Achievement_7 = true;
                break;
            case 8:
                GameSaveGameManager.Instance.GameSaveData.Achievement_8 = true;
                break;
            default:              
                break;
        }
        GameSaveGameManager.Instance.SaveGameData();
    }







}
