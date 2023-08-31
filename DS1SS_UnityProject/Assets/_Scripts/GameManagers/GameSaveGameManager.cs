using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public struct GameSaveData
{
    public bool Achievement_1;
    public bool Achievement_2;
    public bool Achievement_3;
    public bool Achievement_4;
    public bool Achievement_5;
    public bool Achievement_6;

    public int LastSlotUsed;

    public bool HUD;
    public bool Subtitles;
    public string Controls;

    public float Master;
    public float Effects;
    public float Ambience;
    public float Music;
    public float Dialog;
}
public class GameSaveGameManager : MonoBehaviour
{

    public static GameSaveGameManager Instance;

    public GameSaveData GameSaveData;
    string filePath;
    const string FILE_NAME = "SaveStatus.json";



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
        filePath = Application.persistentDataPath;
        GameSaveData = new GameSaveData();
        LoadGameSaveData();
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);

    }


    public void LoadGameSaveData()
    {
        //always check the file exists
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            //load the file content as string
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            //deserialise the loaded string into a GameSaveData struct
            GameSaveData = JsonUtility.FromJson<GameSaveData>(loadedJson);
            Debug.Log("GameData File loaded successfully");
        }
        else
        {
            Debug.Log("GameData File not found, created a new one");
            GameSaveData.Achievement_1 = false;
            GameSaveData.Achievement_2 = false;
            GameSaveData.Achievement_3 = false;
            GameSaveData.Achievement_4 = false;
            GameSaveData.Achievement_5 = false;
            GameSaveData.Achievement_6 = false;

            GameSaveData.HUD = true;
            GameSaveData.Subtitles = true;
            GameSaveData.Controls = "Keyboard";

            GameSaveData.LastSlotUsed = 0;

            GameSaveData.Master = 10;
            GameSaveData.Effects = 10;
            GameSaveData.Ambience = 10;
            GameSaveData.Music = 10;
            GameSaveData.Dialog = 10;

            SaveGameData();         
        }
    }

    public void SaveGameData()
    {
        


        //serialise the GameStatus struct into a Json string
        string gameStatusJson = JsonUtility.ToJson(GameSaveData);
        //write a text file containing the string value as simple text
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
        Debug.Log("File created and saved");
        Debug.Log(GameSaveData.LastSlotUsed);
    }

    public void SaveSettings(bool HUD, bool Subtitles,string Controls, float Master, float Effects, float Ambience, float Music, float Dialog)
    {
        GameSaveData.HUD = HUD;
        GameSaveData.Subtitles = Subtitles;
        GameSaveData.Controls = Controls;

        GameSaveData.Master = Master;
        GameSaveData.Effects = Effects;
        GameSaveData.Ambience = Ambience;
        GameSaveData.Music = Music;
        GameSaveData.Dialog = Dialog;

        SaveGameData();
    }
}
