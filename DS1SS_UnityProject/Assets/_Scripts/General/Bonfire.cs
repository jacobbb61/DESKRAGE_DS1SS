using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonfire : MonoBehaviour
{
    private PlayerManager PM;
    private PlayerControllerV2 PC;

    public int BonfireTagNum;
    public bool BonfireEverUsed;

    private Animator Anim;

    public GameObject[] EnemySaveManagerList;
    private GameObject BonfirePromptUI;


    void Start()
    {
        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerV2>();
        EnemySaveManagerList = GameObject.FindGameObjectsWithTag("Enemy");

        Anim = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().BonfireAnim;
        BonfirePromptUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ItemPrompt;
    }



    public void UseBonfire()
    {
        PC.MyRb.velocity = Vector2.zero;
        if (BonfireEverUsed) { Anim.Play("BonfireLitActivate"); }
        else { Anim.Play("BonfireLitFirstTime"); BonfireEverUsed = true; }
        PM.LastBonfireVisited = BonfireTagNum;

        StartCoroutine(WaitForAnim());
    }


    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(1);
        PC.Health = PC.MaxHealth;
        PC.Stamina = PC.MaxStamina;
        // replenish estus
        PC.CurrentEstus = PC.MaxEstus;
        // Reset Enemies
        if (EnemySaveManagerList != null)
        {
            foreach (GameObject Enemy in EnemySaveManagerList)
            {
                Enemy.GetComponent<EnemySaveManager>().RespawnEvent.Invoke();
            }
        }
    }







    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BonfirePromptUI.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ItemProptDescription.text = "Light flame";
            collision.GetComponent<PlayerControllerV2>().Interactable = GetComponent<InteractableV2>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BonfirePromptUI.SetActive(false);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ItemProptDescription.text = "You should not see me";
            collision.GetComponent<PlayerControllerV2>().Interactable = null;
        }
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {

        switch (BonfireTagNum)
        {
            case 1:
                CurrentCharacterData.BonfireEverUsed_1 = BonfireEverUsed;
                break;
            case 2:
                CurrentCharacterData.BonfireEverUsed_2 = BonfireEverUsed;
                break;
            case 3:
                CurrentCharacterData.BonfireEverUsed_3 = BonfireEverUsed;
                break;
        }
    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        switch (BonfireTagNum)
        {
            case 1:
                BonfireEverUsed = CurrentCharacterData.BonfireEverUsed_1;
                break;
            case 2:
                BonfireEverUsed = CurrentCharacterData.BonfireEverUsed_2;
                break;
            case 3:
                BonfireEverUsed = CurrentCharacterData.BonfireEverUsed_3;
                break;
        }
    }

}
