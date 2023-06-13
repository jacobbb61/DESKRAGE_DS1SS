using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int CurrentEstus;
    public float CurrentHP;
    public float TimePlayedSeconds;
    public string Name = "";
    private void Start()
    {
        WorldSaveGameManager.Instance.Player = this;

    }
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build")) { TimePlayedSeconds += Time.deltaTime; }
    }


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.CharacterName = "character from player manager";
        CurrentCharacterData.yPos = transform.position.y;
        CurrentCharacterData.xPos = transform.position.x;

        CurrentCharacterData.Estus = CurrentEstus;
        CurrentCharacterData.HP = CurrentHP;
        CurrentCharacterData.TimePlayed = TimePlayedSeconds;
    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        //Name = CurrentCharacterData.CharacterName;
        Vector2 MyPos = new Vector2 (CurrentCharacterData.xPos, CurrentCharacterData.yPos);
        transform.position = MyPos;
        CurrentHP = CurrentCharacterData.HP;
        CurrentEstus = CurrentCharacterData.Estus;
        TimePlayedSeconds = CurrentCharacterData.TimePlayed;
        Debug.Log("my pos");
    }
}
