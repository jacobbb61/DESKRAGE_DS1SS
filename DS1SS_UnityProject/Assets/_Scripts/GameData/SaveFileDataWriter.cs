using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{
    public string SaveDataDirectoryPath = "";
    public string SaveFileName = "";


    // before we create a new save file, we must check to see if one of this characetr slot already exists (max 3 character slots)
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(SaveDataDirectoryPath, SaveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // used to delete character save files
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(SaveDataDirectoryPath, SaveFileName));
    }


    // used to create a save file upon starting a new game
    public void CreateNewCharacterSaveFile(CharacterSaveData CharacterData)
    {
        // make a path to the save file
        string SavePath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

        try
        {
            //create the directopry the file will be written to, if it does not already exist
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            Debug.Log("Creating save file, at save path: " + SavePath);

            //serialize the C sharp game data object into json
            string DataToStore = JsonUtility.ToJson(CharacterData, true);

            //write that file to our system
            using (FileStream Stream = new FileStream(SavePath, FileMode.Create))
            {
                using (StreamWriter FileWriter = new StreamWriter(Stream))
                {
                    FileWriter.Write(DataToStore);
                }
            }
        }
        catch(Exception Ex)
        {
            Debug.LogError("Error while trying to save, character data, it no working???" + SavePath + "\n" + Ex);
        }
    }

    //used to load a save file upon loading previous game
     public CharacterSaveData LoadSaveFile()
    {

        CharacterSaveData CharacterData = null;

        // make a path to the LOAD file
        string LoadPath = Path.Combine(SaveDataDirectoryPath, SaveFileName);


        if (File.Exists(LoadPath))
        {

            try
            {
            string DataToLoad = "";

            using (FileStream Stream = new FileStream(LoadPath, FileMode.Open))
            {
                using (StreamReader FileReader = new StreamReader(Stream))
                {
                    DataToLoad = FileReader.ReadToEnd();
                }
            }

            // deserialize the data from json back to a unity file
            CharacterData = JsonUtility.FromJson<CharacterSaveData>(DataToLoad);
            }
        catch(Exception EX)
        {
                Debug.LogError("Failed to load save data");
        }
        }

        return CharacterData;

    }
}
