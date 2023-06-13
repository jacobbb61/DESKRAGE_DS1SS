using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySaveManager : MonoBehaviour
{
    [Header("Enemy Tag")]
    public int EnemyTag_This;
    //public EnemyScript EnemyScript_This

    [Header("Enemy Data")]
    public string EnemyBehaviour_This;
    public float EnemyHealth_This;
    public float EnemyPosX_This;
    public float EnemyPosY_This;

    //public int EnemyDirection_This;


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        // EnemyBehaviour_This = donnacha code
        // EnemyHealth_This = donnacha code
        EnemyPosX_This = transform.position.x;
        EnemyPosY_This = transform.position.y;

        switch (EnemyTag_This)
        {
            case 1:
            CurrentCharacterData.EnemyBehaviour_01 = EnemyBehaviour_This;
               CurrentCharacterData.EnemyHealth_01 = EnemyHealth_This;
                 CurrentCharacterData.EnemyPosX_01 = EnemyPosX_This;
                 CurrentCharacterData.EnemyPosY_01 = EnemyPosY_This;
                break;
            case 2:
                CurrentCharacterData.EnemyBehaviour_02 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_02 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_02 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_02 = EnemyPosY_This;
                break;
        }


    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        switch (EnemyTag_This)
        {
            case 1:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_01;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_01;
                Vector2 MyPos_1 = new Vector2(CurrentCharacterData.EnemyPosX_01, CurrentCharacterData.EnemyPosY_01);
                transform.position = MyPos_1;
                break;
            case 2:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_02;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_02;
                Vector2 MyPos_2 = new Vector2(CurrentCharacterData.EnemyPosX_02, CurrentCharacterData.EnemyPosY_02);
                transform.position = MyPos_2;
                break;
        }
    }
}
