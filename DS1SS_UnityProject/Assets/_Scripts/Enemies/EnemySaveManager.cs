using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class EnemySaveManager : MonoBehaviour
{

    public UnityEvent RespawnEvent;

    [Header("Enemy Identity")]
    public int EnemyTag_This;
    public string EnemyVariant;

    [Header("Enemy Data")]
    public string EnemyBehaviour_This;
    public float EnemyHealth_This;
    public float EnemyPosX_This;
    public float EnemyPosY_This;

    //public int EnemyDirection_This;


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {

        // EnemyBehaviour_This = donnacha code

        EnemyPosX_This = transform.position.x;
        EnemyPosY_This = transform.position.y;

        switch (EnemyVariant)
        {
            case "Dummy":
                EnemyHealth_This = GetComponent<UndeadDummy>().Health;
                break;
            case "A":

                break;
            case "B":

                break;
            case "C":

                break;
            case "D":

                break;
        }

        switch (EnemyTag_This)
        {
            case 1:
            CurrentCharacterData.EnemyBehaviour_1 = EnemyBehaviour_This;
               CurrentCharacterData.EnemyHealth_1 = EnemyHealth_This;
                 CurrentCharacterData.EnemyPosX_1 = EnemyPosX_This;
                 CurrentCharacterData.EnemyPosY_1 = EnemyPosY_This;
                break;
            case 2:
                CurrentCharacterData.EnemyBehaviour_2 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_2 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_2 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_2 = EnemyPosY_This;
                break;
            case 100:
                CurrentCharacterData.EnemyBehaviour_100 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_100 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_100 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_100 = EnemyPosY_This;
                break;
        }


    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        switch (EnemyTag_This)
        {
            case 1:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_1;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_1;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_1;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_1;
                break;
            case 2:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_2;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_2;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_2;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_2;
                break;
            case 100:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_100;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_100;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_100;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_100;
                break;
        }

        transform.position = new Vector2(EnemyPosX_This, EnemyPosY_This);

        switch (EnemyVariant)
        {
            case "Dummy":
                GetComponent<UndeadDummy>().Health = EnemyHealth_This;
                break;
            case "A":

                break;
            case "B":

                break;
            case "C":

                break;
            case "D":

                break;
        }
    }
}
