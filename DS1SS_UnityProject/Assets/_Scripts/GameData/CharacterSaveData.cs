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


    [Header("Enemy Data")]
    public string EnemyBehaviour_01;
    public float EnemyHealth_01;
    public float EnemyPosX_01;
    public float EnemyPosY_01;

    public string EnemyBehaviour_02;
    public float EnemyHealth_02;
    public float EnemyPosX_02;
    public float EnemyPosY_02;

    public string EnemyBehaviour_03;
    public float EnemyHealth_03;
    public float EnemyPosX_03;
    public float EnemyPosY_03;

    [Header("Door Data")]
    public string DoorState_A1;
    public string DoorState_A2;
}
