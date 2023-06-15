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
    public Texture AchievementSymbol_7;
    public Texture AchievementSymbol_8;

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
                GameSaveGameManager.Instance.GameSaveData.Achievement_1 = true;
                AchievementTitle.text = "Out of the frying pan";
                AchievementSymbol.texture = AchievementSymbol_1;
                break;
            case 2:
                GameSaveGameManager.Instance.GameSaveData.Achievement_2 = true;
                AchievementTitle.text = "A new Knight in town";
                AchievementSymbol.texture = AchievementSymbol_2;
                break;
            case 3:
                GameSaveGameManager.Instance.GameSaveData.Achievement_3 = true;
                AchievementTitle.text = "Break a leg";
                AchievementSymbol.texture = AchievementSymbol_3;
                break;
            case 4:
                GameSaveGameManager.Instance.GameSaveData.Achievement_4 = true;
                AchievementTitle.text = "Humpty Dumpty";
                AchievementSymbol.texture = AchievementSymbol_4;
                break;
            case 5:
                GameSaveGameManager.Instance.GameSaveData.Achievement_5 = true;
                AchievementTitle.text = "Flawless";
                AchievementSymbol.texture = AchievementSymbol_5;
                break;
            case 6:
                GameSaveGameManager.Instance.GameSaveData.Achievement_6 = true;
                AchievementTitle.text = "Unbroken Spirit";
                AchievementSymbol.texture = AchievementSymbol_6;
                break;
            case 7:
                GameSaveGameManager.Instance.GameSaveData.Achievement_7 = true;
                AchievementTitle.text = "Touch grass";
                AchievementSymbol.texture = AchievementSymbol_7;
                break;
            case 8:
                GameSaveGameManager.Instance.GameSaveData.Achievement_8 = true;
                AchievementTitle.text = "The Chosen Undead";
                AchievementSymbol.texture = AchievementSymbol_8;
                break;
            default:              
                break;
        }

        AchievementObj.GetComponent<Animator>().SetTrigger("Active");

        GameSaveGameManager.Instance.SaveGameData();
    }







}
