using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(InteractableV2))]
public class DoorManager : MonoBehaviour
{
    public LayerManagerV2 layerManager;
    [Tooltip("ID number for differentiating between door type. 0 = regular, 1 = layer changing door")][SerializeField] private int doorType; // For differentiating between different kinds of doors
    [Tooltip("Only used for layer changing doors (ID 1)")]public int targetLayer; // For doors that switch the player's layer
    private bool inBossFight;



    public string CurrentDoorState_This;

     private CanvasManager CanvasManager;
    [SerializeField] private Collider2D doorCollider; 
     private GameObject doorPrompt;
     private GameObject doorUI;
     private TextMeshProUGUI doorUIText;

    private DoorSaveManager doorSaveManager;






    void Start()
    {
        layerManager = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
        CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        doorSaveManager = GetComponent<DoorSaveManager>();
        doorPrompt = CanvasManager.DoorPrompt;
        doorUI = CanvasManager.DoorUI;
        doorUIText = CanvasManager.DoorDescription;

        if (doorType == 1)
        {
            switch (CurrentDoorState_This)
            {


                case "Open":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = false;
                    break;
                case "Closed":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    break;
                case "Locked":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    break;
                case "OneSided":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    break;
                case "Fog":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    break;
            }
        }

         
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            

            switch (CurrentDoorState_This)
            {
                
                case "Open":
                   
                    if (doorType == 1) { doorPrompt.SetActive(true); }
                    else { doorPrompt.SetActive(false); }
                    
                    break;

                default:
                    doorPrompt.SetActive(true);
                    break;
            }


            collision.GetComponent<PlayerControllerV2>().Interactable = GetComponent<InteractableV2>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorPrompt.SetActive(false);
            doorUI.SetActive(false);
            doorUIText.text = "Should not see this";
            collision.GetComponent<PlayerControllerV2>().Interactable = null;
        }
    }

    public void UseDoor()
    {

        doorPrompt.SetActive(false);
        if (doorType == 1) //perpendicular 
        {
            switch (CurrentDoorState_This)
            {

                case "Locked":
                    doorUI.SetActive(true);
                    doorUIText.text = "Locked";
                    break;
                case "Fog":
                    doorUI.SetActive(true);
                    doorCollider.enabled = false;
                    break;
                case "Closed":
                    doorCollider.enabled = false; 
                    //play anim
                    break;
            }
        }
        else
        {
            switch (CurrentDoorState_This) //layer
            {
                case "Locked":
                    doorUI.SetActive(true);
                    doorUIText.text = "Locked";
                    break;
                case "OneSided":
                    doorUI.SetActive(true);
                    doorUIText.text = "Does not open from this side";
                    break;
                case "Fog":
                    
                    layerManager.ChangeLayer(targetLayer);
                    break;
                case "Open":

                    layerManager.ChangeLayer(targetLayer);

                    if (doorSaveManager.DoorTag_This == "J1")
                    {
                        GetComponent<UnlockDoor>().UnlockOtherDoor();
                    }

                    break;
            }
        }                       
        
    }

    private IEnumerator Wait(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
    }
}
