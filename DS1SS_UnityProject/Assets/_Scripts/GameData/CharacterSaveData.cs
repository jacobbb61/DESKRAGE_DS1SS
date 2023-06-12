using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
//since we want to reference this data for every save file, this script is not a monobehaviour and is serializable instead
public class CharacterSaveData 
{

    [Header("CharacterName")]
    public string CharacterName = "Character from save data";

    // only save basic variables, no objects or vectors
    [Header("World Position")]
    public float xPos;
    public float yPos;

    [Header("Stuff")]
    public int Estus;
    public float HP;
    public float TimePlayed;

}
