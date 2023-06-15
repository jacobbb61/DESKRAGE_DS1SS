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
    public string PlayerLayer;

    [Header("Stuff")]
    public int Estus;
    public int MaxEstus;
    public float HP;
    public float TimePlayed;

    [Header("Bonfires")]
    public int LastBonfireVisited;
    public bool BonfireEverUsed_1;
    public bool BonfireEverUsed_2;
    public bool BonfireEverUsed_3;


    [Header("Enemy Data")]
    public string EnemyBehaviour_1;
    public float EnemyHealth_1;
    public float EnemyPosX_1;
    public float EnemyPosY_1;

    public string EnemyBehaviour_2;
    public float EnemyHealth_2;
    public float EnemyPosX_2;
    public float EnemyPosY_2;

    public string EnemyBehaviour_3;
    public float EnemyHealth_3;
    public float EnemyPosX_3;
    public float EnemyPosY_3;

    public string EnemyBehaviour_100;
    public float EnemyHealth_100;
    public float EnemyPosX_100;
    public float EnemyPosY_100;

    [Header("Door Data")]
    public string DoorState_A1;
    public string DoorState_A2;
}
