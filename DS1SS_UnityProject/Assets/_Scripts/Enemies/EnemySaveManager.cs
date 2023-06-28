using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class EnemySaveManager : MonoBehaviour
{

    public UnityEvent RespawnEvent;
    public UnityEvent TakeLightDamageEvent;
    public UnityEvent TakeHeavyDamageEvent;

    [Header("Enemy Identity")]
    public int EnemyTag_This;
    public string EnemyVariant;

    [Header("Enemy Data")]
    public string EnemyBehaviour_This;
    public float EnemyHealth_This;
    public float EnemyPosX_This;
    public float EnemyPosY_This;
    public int EnemyDirection_This;

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        Debug.Log("Saved Enemy " + EnemyTag_This);
        

        EnemyPosX_This = transform.localPosition.x;
        EnemyPosY_This = transform.localPosition.y;

        switch (EnemyVariant)
        {
            case "Dummy":
                EnemyHealth_This = GetComponent<UndeadDummy>().Health;
                break;
            case "A":
                EnemyBehaviour_This = GetComponent<Undead_A>().Behaviour;
                EnemyHealth_This = GetComponent<Undead_A>().Health;
                EnemyDirection_This = GetComponent<Undead_A>().LookDirection;
                break;
            case "B":
                EnemyBehaviour_This = GetComponent<Undead_B>().Behaviour;
                EnemyHealth_This = GetComponent<Undead_B>().Health;
                EnemyDirection_This = GetComponent<Undead_B>().LookDirection;
                break;
            case "C":
                EnemyBehaviour_This = GetComponent<Undead_C>().Behaviour;
                EnemyHealth_This = GetComponent<Undead_C>().Health;
                EnemyDirection_This = GetComponent<Undead_C>().LookDirection;
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
                 CurrentCharacterData.EnemyDirection_1 = EnemyDirection_This;
                break;
            case 2:
                CurrentCharacterData.EnemyBehaviour_2 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_2 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_2 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_2 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_2 = EnemyDirection_This;
                break;
            case 3:
                CurrentCharacterData.EnemyBehaviour_3 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_3 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_3 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_3 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_3 = EnemyDirection_This;
                break;
            case 4:
                CurrentCharacterData.EnemyBehaviour_4 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_4 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_4 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_4 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_4 = EnemyDirection_This;
                break;
            case 5:
                CurrentCharacterData.EnemyBehaviour_5 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_5 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_5 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_5 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_5 = EnemyDirection_This;
                break;
            case 6:
                CurrentCharacterData.EnemyBehaviour_6 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_6 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_6 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_6 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_6 = EnemyDirection_This;
                break;
            case 7:
                CurrentCharacterData.EnemyBehaviour_7 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_7 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_7 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_7 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_7 = EnemyDirection_This;
                break;
            case 8:
                CurrentCharacterData.EnemyBehaviour_8 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_8 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_8 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_8 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_8 = EnemyDirection_This;
                break;
            case 9:
                CurrentCharacterData.EnemyBehaviour_9 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_9 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_9 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_9 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_9 = EnemyDirection_This;
                break;
            case 10:
                CurrentCharacterData.EnemyBehaviour_10 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_10 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_10 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_10 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_10 = EnemyDirection_This;
                break;
            case 11:
                CurrentCharacterData.EnemyBehaviour_11 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_11 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_11 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_11 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_11 = EnemyDirection_This;
                break;
            case 12:
                CurrentCharacterData.EnemyBehaviour_12 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_12 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_12 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_12 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_12 = EnemyDirection_This;
                break;
            case 13:
                CurrentCharacterData.EnemyBehaviour_13 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_13 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_13 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_13 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_13 = EnemyDirection_This;
                break;
            case 14:
                CurrentCharacterData.EnemyBehaviour_14 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_14 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_14 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_14 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_14 = EnemyDirection_This;
                break;
            case 15:
                CurrentCharacterData.EnemyBehaviour_15 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_15 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_15 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_15 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_15 = EnemyDirection_This;
                break;
            case 16:
                CurrentCharacterData.EnemyBehaviour_16 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_16 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_16 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_16 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_16 = EnemyDirection_This;
                break;
            case 17:
                CurrentCharacterData.EnemyBehaviour_17 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_17 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_17 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_17 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_17 = EnemyDirection_This;
                break;
            case 18:
                CurrentCharacterData.EnemyBehaviour_18 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_18 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_18 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_18 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_18 = EnemyDirection_This;
                break;
            case 19:
                CurrentCharacterData.EnemyBehaviour_19 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_19 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_19 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_19 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_19 = EnemyDirection_This;
                break;
            case 20:
                CurrentCharacterData.EnemyBehaviour_20 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_20 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_20 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_20 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_20 = EnemyDirection_This;
                break;
            case 21:
                CurrentCharacterData.EnemyBehaviour_21 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_21 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_21 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_21 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_21 = EnemyDirection_This;
                break;
            case 22:
                CurrentCharacterData.EnemyBehaviour_22 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_22 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_22 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_22 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_22 = EnemyDirection_This;
                break;
            case 23:
                CurrentCharacterData.EnemyBehaviour_23 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_23 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_23 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_23 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_23 = EnemyDirection_This;
                break;
            case 24:
                CurrentCharacterData.EnemyBehaviour_24 = EnemyBehaviour_This;
                CurrentCharacterData.EnemyHealth_24 = EnemyHealth_This;
                CurrentCharacterData.EnemyPosX_24 = EnemyPosX_This;
                CurrentCharacterData.EnemyPosY_24 = EnemyPosY_This;
                CurrentCharacterData.EnemyDirection_24 = EnemyDirection_This;
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
        Debug.Log("Loaded Enemy " + EnemyTag_This);

        switch (EnemyTag_This)
        {
            case 1:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_1;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_1;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_1;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_1;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_1;
                break;
            case 2:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_2;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_2;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_2;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_2;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_1;
                break;
            case 3:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_3;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_3;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_3;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_3;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_1;
                break;
            case 4:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_4;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_4;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_4;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_4;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_4;
                break;
            case 5:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_5;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_5;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_5;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_5;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_5;
                break;
            case 6:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_6;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_6;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_6;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_6;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_6;
                break;
            case 7:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_7;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_7;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_7;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_7;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_7;
                break;
            case 8:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_8;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_8;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_8;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_8;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_8;
                break;
            case 9:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_9;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_9;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_9;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_9;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_9;
                break; 
            case 10:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_10;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_10;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_10;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_10;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_10;
                break;
            case 11:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_11;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_11;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_11;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_11;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_11;
                break;
            case 12:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_12;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_12;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_12;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_12;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_12;
                break;
            case 13:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_13;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_13;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_13;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_13;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_13;
                break;
            case 14:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_14;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_14;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_14;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_14;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_14;
                break;
            case 15:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_15;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_15;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_15;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_15;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_15;
                break;
            case 16:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_16;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_16;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_16;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_16;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_16;
                break;
            case 17:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_17;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_17;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_17;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_17;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_17;
                break;
            case 18:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_18;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_18;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_18;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_18;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_18;
                break;
            case 19:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_19;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_19;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_19;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_19;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_19;
                break;
            case 20:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_20;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_20;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_20;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_20;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_20;
                break;
            case 21:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_21;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_21;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_21;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_21;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_21;
                break;
            case 22:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_22;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_22;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_22;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_22;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_22;
                break;
            case 23:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_23;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_23;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_23;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_23;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_23;
                break;
            case 24:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_24;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_24;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_24;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_24;
                EnemyDirection_This = CurrentCharacterData.EnemyDirection_24;
                break;
            case 100:
                EnemyBehaviour_This = CurrentCharacterData.EnemyBehaviour_100;
                EnemyHealth_This = CurrentCharacterData.EnemyHealth_100;
                EnemyPosX_This = CurrentCharacterData.EnemyPosX_100;
                EnemyPosY_This = CurrentCharacterData.EnemyPosY_100;
                break;
        }

        transform.localPosition = new Vector2(EnemyPosX_This, EnemyPosY_This);

        switch (EnemyVariant)
        {
            case "Dummy":
               
                GetComponent<UndeadDummy>().Health = EnemyHealth_This;
                break;
            case "A":
                GetComponent<Undead_A>().Behaviour = EnemyBehaviour_This;
                GetComponent<Undead_A>().Health = EnemyHealth_This;
                GetComponent<Undead_A>().LookDirection = EnemyDirection_This;
                if (EnemyHealth_This <= 0) { GetComponent<Undead_A>().Dead(); }
                break;
            case "B":
                GetComponent<Undead_B>().Behaviour = EnemyBehaviour_This;
                GetComponent<Undead_B>().Health = EnemyHealth_This;
                GetComponent<Undead_B>().LookDirection = EnemyDirection_This;
                if(EnemyHealth_This <= 0) { GetComponent<Undead_B>().Dead(); }
                break;
            case "C":
                GetComponent<Undead_C>().Behaviour = EnemyBehaviour_This;
                GetComponent<Undead_C>().Health = EnemyHealth_This;
                GetComponent<Undead_C>().LookDirection = EnemyDirection_This;
                if (EnemyHealth_This <= 0) { GetComponent<Undead_C>().Dead(); }
                break;
            case "D":

                break;
        }
    }
}
