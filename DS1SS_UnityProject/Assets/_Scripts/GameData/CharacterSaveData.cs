using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
//since we want to reference this data for every save file, this script is not a monobehaviour and is serializable instead
public class CharacterSaveData 
{// only save basic variables, no objects or vectors

    [Header("CharacterName")]
    public string CharacterName = "Character from save data";

    
    [Header("Player World Position")]
    public float xPos;
    public float yPos;

    [Header("Player stats")]
    public float TimePlayed;
    public int CurrentEstus;
    public int MaxEstus;
    public float HP;
    public float Stamina;


    [Header("Doors unlocked/locked")]
    public bool DoorF;
    public bool DoorH1;
    public bool DoorH2;
    public bool DoorD2;
    public bool DoorI;
    //public bool DoorI; section 2 underground door
    //public bool DoorI; section 2 Boss entrance
    //public bool DoorI; section 2 Boss exit
    public bool DoorWallToOscar;
    public bool DoorFloorToUnderground;
    public bool DoorFloorToBossArena;


    [Header("Oscar")]
    public string OscarInteraction = "A";
    public bool IsOscarAlive;


}
