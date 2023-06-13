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


        switch (DoorTag_This)
        {
            case "A1":
                CurrentCharacterData.DoorState_A1 = DoorState_This;
                break;
            case "A2":
                CurrentCharacterData.DoorState_A2 = DoorState_This;
                break;
        }


    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        switch (DoorTag_This)
        {
            case "A1":
                DoorState_This = CurrentCharacterData.DoorState_A1;
                break;
            case "A2":
                DoorState_This = CurrentCharacterData.DoorState_A2;
                break;
        }
    }
}
