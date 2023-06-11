using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsGameManager : MonoBehaviour
{
    public static SettingsGameManager Instance;

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

    public void UpdateSettings()
    {


        GameSaveGameManager.Instance.SaveGameData();
    }
}
