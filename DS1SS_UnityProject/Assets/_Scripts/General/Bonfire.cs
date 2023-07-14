using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonfire : MonoBehaviour
{
    private PlayerManager PM;
    private PlayerControllerV2 PC;
    public OscarManager OM;

    public int BonfireTagNum;
    public bool BonfireEverUsed;

    private Animator Anim;

    public GameObject[] EnemySaveManagerList;
    private GameObject BonfirePromptUI;


    void Start()
    {

        PM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerV2>();
       // EnemySaveManagerList = GameObject.FindGameObjectsWithTag("Enemy");

        Anim = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().BonfireAnim;
        BonfirePromptUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ItemPrompt;
    }


    public void InteractBonfire()
    {
        StartCoroutine(UseThisBonfire());
        

    }

    IEnumerator UseThisBonfire()
    {
        PC.MyRb.velocity = Vector2.zero;
        PM.LastBonfireVisited = BonfireTagNum;

        PC.CanMove = false;
        PC.CanAttack = false;
        PC.CanFollowUp = false;
        PC.Anim.Play("PlayerAnim_BonfireInteract");

        yield return new WaitForSeconds(1);

        if (BonfireEverUsed) { Anim.Play("BonfireLitActivate"); }
        else { Anim.Play("BonfireLitFirstTime"); BonfireEverUsed = true; }
        
        yield return new WaitForSeconds(1);
        PC.PlayerFinishInteraction();
        BonfireRest();
        PC.Anim.Play("PlayerAnim_Idle");
        PC.CanMove = true;
        PC.CanAttack = true;
    }


    public void BonfireRest()
    {
        PC.State = "Idle";
        PC.CanMove = true;

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

        if (PM.DemonArena.currentState == "Active") //player died to demon
        {
            PM.DemonArena.SwitchState("Idle");
            PM.DemonArena.ManualStart();
        }

        //upadate oscar
        OM.Reload();
    }







    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BonfirePromptUI.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ItemProptDescription.text = "Y : Light flame";
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
