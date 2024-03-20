using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class EstusPickUp : MonoBehaviour
{

    public string itemName;
    public int itemID;

    public EstusPickUpManager estusPickUpManager;
    public PlayerManager playerManager;
    public PlayerControllerV2 PC;
    public CanvasManager canvasManager;

    public GameObject ItemAsset;
    public GameObject ItemObject;

    public EventReference ItemAudioRef;

    [Header("Item UI")]
    public GameObject ItemPopUp;
    public RawImage ItemSymbol;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemQuantity;

    public Texture KeySymbol;



    public void Remove()
    {
        ItemObject.SetActive(false);
    }


    public void PickUp()
    {
        canvasManager.ItemPrompt.SetActive(false);
        PC.PlayerFinishInteraction();

        PC.GiveOneEstus();

        ItemName.text = itemName;

        ItemSymbol.texture = KeySymbol;
        ItemAsset.SetActive(false);
        ItemQuantity.text = "1";
        StartCoroutine(PopUp());

        GetComponent<SpriteRenderer>().enabled = false;

        RuntimeManager.PlayOneShot(ItemAudioRef, transform.position);

        switch(itemID)
        {
            case 1:
                estusPickUpManager.EstusPickedUp_1 = true;
                break;
            case 2:
                estusPickUpManager.EstusPickedUp_2 = true;
                break;
            case 3:
                estusPickUpManager.EstusPickedUp_3 = true;
                break;
            case 4:
                estusPickUpManager.EstusPickedUp_4 = true;
                break;
            case 5:
                estusPickUpManager.EstusPickedUp_5 = true;
                break;
            case 6:
                estusPickUpManager.EstusPickedUp_6 = true;
                break;
            case 7:
                PC.MaxHealth = 10000;
                PC.Health = 10000;
                PC.MaxEstus = 10000;
                PC.CurrentEstus = 10000;
                break;
        }


    }
    IEnumerator PopUp()
    {

        ItemPopUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ItemPopUp.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canvasManager.ItemPrompt.SetActive(true);
        PC.Interactable = GetComponent<InteractableV2>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvasManager.ItemPrompt.SetActive(false);
        PC.Interactable = null;
    }
}
