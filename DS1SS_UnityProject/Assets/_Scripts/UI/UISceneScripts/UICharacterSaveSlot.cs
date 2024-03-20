using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UICharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWriter SaveFileWriter;

    [Header("Game Slot")]
    public int characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI TimePlayed;
    private float seconds;
    private float minutes;
    private float hours;
    public GameObject CompletedGame;

    public GameObject nightmare01;
    public GameObject nightmare02;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        SaveFileWriter = new SaveFileDataWriter();
        SaveFileWriter.SaveDataDirectoryPath = Application.persistentDataPath;

        // save slot 01
        if (characterSlot == 1)
        {
           
            SaveFileWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            Debug.Log(WorldSaveGameManager.Instance.DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01));
            // if file exists, get info from it
            if (SaveFileWriter.CheckToSeeIfFileExists())
            {
                float timer = WorldSaveGameManager.Instance.CharacterSlot01.TimePlayed;
                float mili = TimeSpan.FromSeconds(timer).Milliseconds;
                float seconds = TimeSpan.FromSeconds(timer).Seconds;
                float minutes = TimeSpan.FromSeconds(timer).Minutes;
                float hours = TimeSpan.FromSeconds(timer).Hours;
                TimePlayed.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");

                if (WorldSaveGameManager.Instance.CharacterSlot01.FinishedGame)
                {
                    CompletedGame.SetActive(true);
                }
                if (WorldSaveGameManager.Instance.CharacterSlot01.Nightmare)
                {
                    nightmare01.SetActive(true);
                    nightmare02.SetActive(true);
                }

            }
            // doesnt exits, disable
            else
            {
                gameObject.SetActive(false);
            }

        } else if (characterSlot == 2)
        {


            SaveFileWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            // if file exists, get info from it
            if (SaveFileWriter.CheckToSeeIfFileExists())
            {
                float timer = WorldSaveGameManager.Instance.CharacterSlot02.TimePlayed;
                float mili = TimeSpan.FromSeconds(timer).Milliseconds;
                float seconds = TimeSpan.FromSeconds(timer).Seconds;
                float minutes = TimeSpan.FromSeconds(timer).Minutes;
                float hours = TimeSpan.FromSeconds(timer).Hours;
                TimePlayed.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");




                if (WorldSaveGameManager.Instance.CharacterSlot02.FinishedGame)
                {
                    CompletedGame.SetActive(true);
                }
                if (WorldSaveGameManager.Instance.CharacterSlot02.Nightmare)
                {
                    nightmare01.SetActive(true);
                    nightmare02.SetActive(true);
                }
            }
            // doesnt exits, disable
            else
            {
                gameObject.SetActive(false);
            }

        }
        else if (characterSlot == 3)
        { 

             SaveFileWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            // if file exists, get info from it
            if (SaveFileWriter.CheckToSeeIfFileExists())
            {
                float timer = WorldSaveGameManager.Instance.CharacterSlot03.TimePlayed;
                float mili = TimeSpan.FromSeconds(timer).Milliseconds;
                float seconds = TimeSpan.FromSeconds(timer).Seconds;
                float minutes = TimeSpan.FromSeconds(timer).Minutes;
                float hours = TimeSpan.FromSeconds(timer).Hours;
                TimePlayed.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");

                if (WorldSaveGameManager.Instance.CharacterSlot03.FinishedGame)
                {
                    CompletedGame.SetActive(true);
                }
                if (WorldSaveGameManager.Instance.CharacterSlot03.Nightmare)
                {
                    nightmare01.SetActive(true);
                    nightmare02.SetActive(true);
                }

            }
            // doesnt exits, disable
            else
            {
                gameObject.SetActive(false);
            }

        }
        
    }



    public void LoadGameFromCharacterSlot()
    {
        if (characterSlot==1) { WorldSaveGameManager.Instance.CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01; }
        if (characterSlot==2) { WorldSaveGameManager.Instance.CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02; }
        if (characterSlot==3) { WorldSaveGameManager.Instance.CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03; }
        WorldSaveGameManager.Instance.LoadGame();
    } 



}
