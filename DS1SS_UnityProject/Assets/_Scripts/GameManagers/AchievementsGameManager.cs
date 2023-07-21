using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsGameManager : MonoBehaviour
{
    public static AchievementsGameManager Instance;
    public TextMeshProUGUI AchievementTitle;
    public RawImage AchievementSymbol;
    public GameObject AchievementObj;

    public Texture AchievementSymbol_1;
    public Texture AchievementSymbol_2;
    public Texture AchievementSymbol_3;
    public Texture AchievementSymbol_4;
    public Texture AchievementSymbol_5;
    public Texture AchievementSymbol_6;

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
        AchievementObj = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().AchievementObj;
        AchievementSymbol = AchievementObj.GetComponentInChildren<RawImage>();
        AchievementTitle = AchievementObj.GetComponentInChildren<TextMeshProUGUI>();

        switch (num)
        {
            case 1:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_1 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_1 = true;
                    AchievementTitle.text = "Out of the frying pan";
                    AchievementSymbol.texture = AchievementSymbol_1;
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            case 2:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_2 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_2 = true;
                    AchievementTitle.text = "A new Knight in town";
                    AchievementSymbol.texture = AchievementSymbol_2;
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            case 3:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_3 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_3 = true;
                    AchievementTitle.text = "Break a leg";
                    AchievementSymbol.texture = AchievementSymbol_3;
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            case 4:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_4 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_4 = true;
                    AchievementTitle.text = "Humpty Dumpty";
                    AchievementSymbol.texture = AchievementSymbol_4;
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            case 5:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_5 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_5 = true;
                    AchievementTitle.text = "Flawless";
                    AchievementSymbol.texture = AchievementSymbol_5;
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            case 6:
                if (GameSaveGameManager.Instance.GameSaveData.Achievement_6 == false)
                {
                    GameSaveGameManager.Instance.GameSaveData.Achievement_6 = true;
                    AchievementTitle.text = "The Chosen Undead";
                    AchievementSymbol.texture = AchievementSymbol_6; 
                    AchievementObj.GetComponent<Animator>().SetTrigger("Active");
                    GameSaveGameManager.Instance.SaveGameData();
                }
                break;
            default:              
                break;
        }
    }







}
