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
    public bool inBossFight;



    public string CurrentDoorState_This;

     private CanvasManager CanvasManager;
    [SerializeField] private Collider2D doorCollider; 
     private GameObject doorPrompt;
     private GameObject doorUI;
     private TextMeshProUGUI doorUIText;

    public DoorSaveManager doorSaveManager;

    public Animator Anim;

    private PlayerManager playerManager;
    private PlayerControllerV2 PC;


    private void Start()
    {
        
        layerManager = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
        CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        doorSaveManager = GetComponent<DoorSaveManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        PC = FindObjectOfType<PlayerControllerV2>();
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
                    Anim.Play("PerpendicularDoorOpenIdle");
                    
                    break;
                case "Closed":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    Anim.Play("PerpendicularDoorClosedIdle");
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



    public void ManualStart()
    {

        if (doorType == 1)
        {
            switch (CurrentDoorState_This)
            {


                case "Open":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = false;
                    Anim.Play("PerpendicularDoorOpenIdle");
                    
                    break;
                case "Closed":
                    //anim.Play("CellDoorOpenIdle");
                    doorCollider.enabled = true;
                    Anim.Play("PerpendicularDoorClosedIdle");
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

         if(doorSaveManager.DoorTag_This == "N") { GetComponent<DoorOcludingSection>().ManualStart(); }
         if(doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().ManualStart(); }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            doorPrompt = CanvasManager.DoorPrompt;
            doorUI = CanvasManager.DoorUI;
            doorUIText = CanvasManager.DoorDescription;

            switch (CurrentDoorState_This)
            {
                
                case "Open":
                   
                    if (doorType == 0) { doorPrompt.SetActive(true); }
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
            PC.PlayerFinishInteraction();
            switch (CurrentDoorState_This)
            {      
                case "Locked":
                    {
                        switch (doorSaveManager.DoorTag_This) // This gets the tag of the door and checks if it has the corresponding key to unlock
                        {
                            case "A":
                                {
                                    if (playerManager.AKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            case "K":
                                {
                                    if (playerManager.KKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            case "S":
                                {
                                    if (playerManager.SKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            default:
                                {
                                    doorUI.SetActive(true);
                                    doorUIText.text = "Locked";
                                    break;
                                }
                        }
                        break;
                    }
                case "Fog":
                    {
                        doorUI.SetActive(true);
                        doorCollider.enabled = false;
                        break;
                    }
                case "Closed":
                    {
                        doorCollider.enabled = false;
                        CurrentDoorState_This = "Open";
                        Anim.Play("PerpendicularDoorOpenIdle");

                        if (doorSaveManager.DoorTag_This == "N")
                        {
                            AchievementsGameManager.Instance.UnlockedAchievement(1);
                        }

                        if (doorSaveManager.DoorTag_This == "N") { GetComponent<DoorOcludingSection>().RevealArea(); }
                        if (doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().RevealArea(); }

                        break;
                    }
            }
        }
        else
        {
            switch (CurrentDoorState_This) //layer
            {
                case "Locked":
                    {
                        switch (doorSaveManager.DoorTag_This) // This gets the tag of the door and checks if it has the corresponding key to unlock
                        {
                            case "A":
                                {
                                    if (playerManager.AKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            case "K":
                                {
                                    if (playerManager.KKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            case "S":
                                {
                                    if (playerManager.SKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            default:
                                {
                                    doorUI.SetActive(true);
                                    doorUIText.text = "Locked";
                                    break;
                                }
                        }
                        break;
                    }

                case "OneSided":
                    {
                        doorUI.SetActive(true);
                        doorUIText.text = "Does not open from this side";
                        break;
                    }

                case "Fog":
                    {
                        layerManager.ChangeLayer(targetLayer);
                        break;
                    }

                case "Open":
                    {
                        layerManager.ChangeLayer(targetLayer);

                        if (doorSaveManager.DoorTag_This == "J1")
                        {
                            GetComponent<UnlockDoor>().UnlockOtherDoor();
                        }
                        break;
                    }
            }
        }                       
        
    }

    private IEnumerator Wait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
    }

    public void SwitchDoorState(string state) //Used by boss arena manager
    {
        CurrentDoorState_This = state;
        //Visual updates will go here also
    }
}
