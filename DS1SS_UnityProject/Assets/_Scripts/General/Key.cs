using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject correspondingDoor;

    //private DoorManager door;
    public string itemName;
    //private PlayerItems playerItems;
    public PlayerManager playerManager;
    public PlayerControllerV2 PC;
    private CanvasManager canvasManager;

    public GameObject ItemAsset;
    public GameObject ItemObject;
    
    [Tooltip("Current door tags: Cell, A, K, S, N")]public string targetDoorTag;

    public EventReference ItemAudioRef;

    [Header("Item UI")]
    public GameObject ItemPopUp;
    public RawImage ItemSymbol;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemQuantity;

    public Texture KeySymbol;
    public string AKeyName;
    public string SKeyName;
    public string KKeyName;
    public string NKeyName;

    public bool ShowPopUp;

    private void Start()
    {
        //door = correspondingDoor.GetComponent<DoorManager>();
        itemName = gameObject.name;
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
                        ItemObject.SetActive(false);
                    }
                    else
                    {
                        ItemObject.SetActive(true);
                    }
                    break;
                }
            case "S":
                {
                    if (playerManager.SKey == true)
                    {
                        ItemObject.SetActive(false);
                    }
                    else
                    {
                        ItemObject.SetActive(true);
                    }
                    break;
                }
            case "K":
                {
                    if (playerManager.KKey == true)
                    {
                        ItemObject.SetActive(false);
                    }
                    else
                    {
                        ItemObject.SetActive(true);
                    }
                    break;
                }
            case "N":
                {
                    if (playerManager.KKey == true)
                    {
                        ItemObject.SetActive(false);
                    }
                    else
                    {
                        ItemObject.SetActive(true);
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
        if (ItemObject.activeInHierarchy)
        {
            if (targetDoorTag == "N")
            {
                if (ShowPopUp)
                {
                    canvasManager.ItemPrompt.SetActive(true);
                    PC.Interactable = GetComponent<InteractableV2>();
                }
                else
                {
                    canvasManager.ItemPrompt.SetActive(false);
                    PC.Interactable = null;
                }
            }
            else
            {
            canvasManager.ItemPrompt.SetActive(true);
            PC.Interactable = GetComponent<InteractableV2>();
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvasManager.ItemPrompt.SetActive(false);
        PC.Interactable = null;
    }

    public void ShowPop()
    {
        canvasManager.ItemPrompt.SetActive(true);
        PC.Interactable = GetComponent<InteractableV2>();
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
            case "N":
                {
                    ItemName.text = NKeyName;
                    playerManager.NKey = true;
                    break;
                }
            default:
                {
                    Debug.LogError("Error: Invalid target door tag on key " + this.gameObject.name);
                    break;
                }
        }
        ItemSymbol.texture = KeySymbol;
        ItemAsset.SetActive(false);
        ItemQuantity.text = "1";
        StartCoroutine(PopUp());
        Debug.Log(this.name + " picked up. " + correspondingDoor.gameObject + " unlocked");
        GetComponent<SpriteRenderer>().enabled = false;



        RuntimeManager.PlayOneShot(ItemAudioRef, transform.position);



    } 
    
    IEnumerator PopUp()
    {

        ItemPopUp.SetActive(true);
        yield return new WaitForSeconds(2f);
        ItemPopUp.SetActive(false);
        gameObject.SetActive(false);

    }
}
