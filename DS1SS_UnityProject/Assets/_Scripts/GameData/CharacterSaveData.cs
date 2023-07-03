using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
//since we want to reference this data for every save file, this script is not a monobehaviour and is serializable instead
public class CharacterSaveData 
{

    public bool FinishedGame;
    public bool StartedGame;

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

    [Header("Items")]
    public bool AKey;
    public bool KKey;
    public bool SKey;
    public bool EstusItem_3;
    public bool EstusItem_6;

    [Header("Bonfires")]
    public int LastBonfireVisited;
    public bool BonfireEverUsed_1;
    public bool BonfireEverUsed_2;
    public bool BonfireEverUsed_3;

    [Header("Door Data")]
    public bool BoulderUsed;
    public string DoorState_A;
    public string DoorState_C;
    public string DoorState_E;
    public string DoorState_F1;
    public string DoorState_H;
    public string DoorState_J2;
    public string DoorState_K;
    public string DoorState_M1;
    public string DoorState_M2;
    public string DoorState_N;
    public string DoorState_O;
    public string DoorState_P;
    public string DoorState_S;
    public string DoorState_T;
    public string DoorState_U;

    [Header("Oscar Data")]
    public string OscarState;
    public bool MoveInteractionOnLoad;
    public bool IsOscarDead;


    [Header("Enemy Data")]
    public string EnemyBehaviour_1;
    public float EnemyHealth_1;
    public float EnemyPosX_1;
    public float EnemyPosY_1;
    public int EnemyDirection_1;

    public string EnemyBehaviour_2;
    public float EnemyHealth_2;
    public float EnemyPosX_2;
    public float EnemyPosY_2;
    public int EnemyDirection_2;

    public string EnemyBehaviour_3;
    public float EnemyHealth_3;
    public float EnemyPosX_3;
    public float EnemyPosY_3;
    public int EnemyDirection_3;

    public string EnemyBehaviour_4;
    public float EnemyHealth_4;
    public float EnemyPosX_4;
    public float EnemyPosY_4;
    public int EnemyDirection_4;

    public string EnemyBehaviour_5;
    public float EnemyHealth_5;
    public float EnemyPosX_5;
    public float EnemyPosY_5;
    public int EnemyDirection_5;

    public string EnemyBehaviour_6;
    public float EnemyHealth_6;
    public float EnemyPosX_6;
    public float EnemyPosY_6;
    public int EnemyDirection_6;

    public string EnemyBehaviour_7;
    public float EnemyHealth_7;
    public float EnemyPosX_7;
    public float EnemyPosY_7;
    public int EnemyDirection_7;

    public string EnemyBehaviour_8;
    public float EnemyHealth_8;
    public float EnemyPosX_8;
    public float EnemyPosY_8;
    public int EnemyDirection_8;

    public string EnemyBehaviour_9;
    public float EnemyHealth_9;
    public float EnemyPosX_9;
    public float EnemyPosY_9;
    public int EnemyDirection_9;

    public string EnemyBehaviour_10;
    public float EnemyHealth_10;
    public float EnemyPosX_10;
    public float EnemyPosY_10;
    public int EnemyDirection_10;

    public string EnemyBehaviour_11;
    public float EnemyHealth_11;
    public float EnemyPosX_11;
    public float EnemyPosY_11;
    public int EnemyDirection_11;

    public string EnemyBehaviour_12;
    public float EnemyHealth_12;
    public float EnemyPosX_12;
    public float EnemyPosY_12;
    public int EnemyDirection_12;

    public string EnemyBehaviour_13;
    public float EnemyHealth_13;
    public float EnemyPosX_13;
    public float EnemyPosY_13;
    public int EnemyDirection_13;

    public string EnemyBehaviour_14;
    public float EnemyHealth_14;
    public float EnemyPosX_14;
    public float EnemyPosY_14;
    public int EnemyDirection_14;

    public string EnemyBehaviour_15;
    public float EnemyHealth_15;
    public float EnemyPosX_15;
    public float EnemyPosY_15;
    public int EnemyDirection_15;

    public string EnemyBehaviour_16;
    public float EnemyHealth_16;
    public float EnemyPosX_16;
    public float EnemyPosY_16;
    public int EnemyDirection_16;

    public string EnemyBehaviour_17;
    public float EnemyHealth_17;
    public float EnemyPosX_17;
    public float EnemyPosY_17;
    public int EnemyDirection_17;

    public string EnemyBehaviour_18;
    public float EnemyHealth_18;
    public float EnemyPosX_18;
    public float EnemyPosY_18;
    public int EnemyDirection_18;

    public string EnemyBehaviour_19;
    public float EnemyHealth_19;
    public float EnemyPosX_19;
    public float EnemyPosY_19;
    public int EnemyDirection_19;

    public string EnemyBehaviour_20;
    public float EnemyHealth_20;
    public float EnemyPosX_20;
    public float EnemyPosY_20;
    public int EnemyDirection_20;

    public string EnemyBehaviour_21;
    public float EnemyHealth_21;
    public float EnemyPosX_21;
    public float EnemyPosY_21;
    public int EnemyDirection_21;

    public string EnemyBehaviour_22;
    public float EnemyHealth_22;
    public float EnemyPosX_22;
    public float EnemyPosY_22;
    public int EnemyDirection_22;

    public string EnemyBehaviour_23;
    public float EnemyHealth_23;
    public float EnemyPosX_23;
    public float EnemyPosY_23;
    public int EnemyDirection_23;

    public string EnemyBehaviour_24;
    public float EnemyHealth_24;
    public float EnemyPosX_24;
    public float EnemyPosY_24;
    public int EnemyDirection_24;

    public string EnemyBehaviour_100;
    public float EnemyHealth_100;
    public float EnemyPosX_100;
    public float EnemyPosY_100;


}
