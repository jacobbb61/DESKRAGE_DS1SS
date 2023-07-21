using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Key : MonoBehaviour
{
    [SerializeField] private GameObject correspondingDoor;
    //private DoorManager door;
    public string itemName;
    //private PlayerItems playerItems;
    private PlayerManager playerManager;
    private PlayerControllerV2 PC;
    private CanvasManager canvasManager;
    
    [Tooltip("Current door tags: Cell, A, K, S")]public string targetDoorTag;



    [Header("Item UI")]
    public GameObject ItemPopUp;
    public RawImage ItemSymbol;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemQuantity;

    public Texture KeySymbol;
    public string AKeyName;
    public string SKeyName;
    public string KKeyName;

    private void Start()
    {
                //door = correspondingDoor.GetComponent<DoorManager>();
        itemName = gameObject.name;
        playerManager = FindObjectOfType<PlayerManager>();
        PC = FindObjectOfType<PlayerControllerV2>();
        canvasManager = FindAnyObjectByType<CanvasManager>();
    }
    // Start is called before the first frame update
    public void ManualStart()
    {


        switch (targetDoorTag)
        {
            case "A":
                {
                    if (playerManager.AKey == true)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    break;
                }
            case "S":
                {
                    if (playerManager.SKey == true)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    break;
                }
            default:
                {
                    // PANIC
                    break;
                }
        }
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



    public void PickUpKey()
    {
        canvasManager.ItemPrompt.SetActive(false);
        PC.PlayerFinishInteraction();
        switch (targetDoorTag)
        {
            case "A":
                {
                    ItemName.text = AKeyName;
                    playerManager.AKey = true;
                    break;
                }
            case "S":
                {
                    ItemName.text = SKeyName;
                    playerManager.SKey = true;
                    break;
                }
            case "K":
                {
                    ItemName.text = KKeyName;
                    playerManager.KKey = true;
                    break;
                }
            default:
                {
                    Debug.LogError("Error: Invalid target door tag on key " + this.gameObject.name);
                    break;
                }
        }
        ItemSymbol.texture = KeySymbol;
        
        ItemQuantity.text = "1";
        StartCoroutine(PopUp());
        Debug.Log(this.name + " picked up. " + correspondingDoor.gameObject + " unlocked");
        GetComponent<SpriteRenderer>().enabled = false;
    } 
    
    IEnumerator PopUp()
    {

        ItemPopUp.SetActive(true);
        yield return new WaitForSeconds(3f);
        ItemPopUp.SetActive(false);
        gameObject.SetActive(false);

    }
}
