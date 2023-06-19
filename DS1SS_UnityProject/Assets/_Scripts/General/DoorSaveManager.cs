using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSaveManager : MonoBehaviour
{
    [Header("Door Tag")]
    public string DoorTag_This;


    [Header("Door Data")]
    public string DoorState_This;



    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        DoorState_This = GetComponent<DoorManager>().CurrentDoorState_This;

        switch (DoorTag_This)
        {

            case "A":
                CurrentCharacterData.DoorState_A = DoorState_This;
                break;
            case "C":
                CurrentCharacterData.DoorState_C = DoorState_This;
                break;
            case "E":
                CurrentCharacterData.DoorState_E = DoorState_This;
                break;
            case "F1":
                CurrentCharacterData.DoorState_F1 = DoorState_This;
                break;
            case "H":
                CurrentCharacterData.DoorState_H = DoorState_This;
                break;
            case "J2":
                CurrentCharacterData.DoorState_J2 = DoorState_This;
                break;
            case "K":
                CurrentCharacterData.DoorState_K = DoorState_This;
                break;
            case "M1":
                CurrentCharacterData.DoorState_M1 = DoorState_This;
                break;
            case "M2":
                CurrentCharacterData.DoorState_M2 = DoorState_This;
                break;
            case "N":
                CurrentCharacterData.DoorState_N = DoorState_This;
                break;
            case "O":
                CurrentCharacterData.DoorState_O = DoorState_This;
                break;
            case "P":
                CurrentCharacterData.DoorState_P = DoorState_This;
                break;
            case "S":
                CurrentCharacterData.DoorState_S = DoorState_This;
                break;
            case "T":
                CurrentCharacterData.DoorState_T = DoorState_This;
                break;
            case "U":
                CurrentCharacterData.DoorState_U = DoorState_This;
                break;
        }


    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        switch (DoorTag_This)
        {
            case "A":
                DoorState_This = CurrentCharacterData.DoorState_A;
                break;
            case "C":
                DoorState_This = CurrentCharacterData.DoorState_C;
                break;
            case "E":
                DoorState_This = CurrentCharacterData.DoorState_E;
                break;
            case "F1":
                DoorState_This = CurrentCharacterData.DoorState_F1;
                break;
            case "H":
                DoorState_This = CurrentCharacterData.DoorState_H;
                break;
            case "J2":
                DoorState_This = CurrentCharacterData.DoorState_J2;
                break;
            case "K":
                DoorState_This = CurrentCharacterData.DoorState_K;
                break;
            case "M1":
                DoorState_This = CurrentCharacterData.DoorState_M1;
                break;
            case "M2":
                DoorState_This = CurrentCharacterData.DoorState_M2;
                break;
            case "N":
                DoorState_This = CurrentCharacterData.DoorState_N;
                break;
            case "O":
                DoorState_This = CurrentCharacterData.DoorState_O;
                break;
            case "P":
                DoorState_This = CurrentCharacterData.DoorState_P;
                break;
            case "S":
                DoorState_This = CurrentCharacterData.DoorState_S;
                break;
            case "T":
                DoorState_This = CurrentCharacterData.DoorState_T;
                break;
            case "U":
                DoorState_This = CurrentCharacterData.DoorState_U;
                break;
        }

        GetComponent<DoorManager>().CurrentDoorState_This = DoorState_This;
        GetComponent<DoorManager>().ManualStart();
    }
}
