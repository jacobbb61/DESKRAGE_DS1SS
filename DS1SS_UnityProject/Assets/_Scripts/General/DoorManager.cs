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

    public AsylumDemonArena DemonArena;
    public PursuerArena PursuerArena;
    public bool isPursuerDoor;



    public string CurrentDoorState_This;

     private CanvasManager CanvasManager;
    [SerializeField] private Collider2D doorCollider; 
     private GameObject doorPrompt;
     private GameObject fogDoorPrompt;
     private GameObject doorUI;
     public GameObject FogAssets;
     private TextMeshProUGUI doorUIText;

    public DoorSaveManager doorSaveManager;

    public Animator Anim;

    private PlayerManager playerManager;
    private PlayerControllerV2 PC;

    public ParticleSystem ParticleSystem;

    private void Start()
    {
        ParticleSystem.Stop();
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
                    doorCollider.enabled = false;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorOpenIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "Closed":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "Locked":
                    doorCollider.enabled = true;
                    Anim.Play("PerpendicularDoorClosedIdle"); 
                    ParticleSystem.Stop();
                    break;
                case "OneSided":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "Fog":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Play();
                    break;
                case "FogEnter":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Play();
                    break;
            }
        }
    }

    private void OnEnable()
    {
        ManualStart();
    }

    public void ManualStart()
    {

        if (doorType == 1)
        {
            switch (CurrentDoorState_This)
            {
                case "Open":
                    doorCollider.enabled = false;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorOpenIdle"); }
                   // ParticleSystem.Stop();
                    ParticleSystem.Stop();
                    if (doorSaveManager.DoorTag_This == "N") { GetComponent<DoorOcludingSection>().Open(); }
                    if (doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().Open(); }
                    if (doorSaveManager.DoorTag_This == "U") { GetComponent<DoorOcludingSection>().Open(); }
                    break;
                case "Closed":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "Locked":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "OneSided":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Stop();
                    break;
                case "Fog":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    //ParticleSystem.Play();
                    ParticleSystem.Play();
                    break;
                case "FogEnter":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { Anim.Play("PerpendicularDoorClosedIdle"); }
                    ParticleSystem.Play();
                    break;
            }
        }
        else
        {
            switch (CurrentDoorState_This)
            {

                case "Fog":
                    ParticleSystem.Play();
                    break;
                case "FogEnter":
                    ParticleSystem.Play();
                    break;
                default:
                    ParticleSystem.Stop();
                    break;
            }
        }

         if(doorSaveManager.DoorTag_This == "N") { GetComponent<DoorOcludingSection>().ManualStart(); }
         if(doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().ManualStart(); }
         if(doorSaveManager.DoorTag_This == "U") { GetComponent<DoorOcludingSection>().ManualStart(); }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            doorPrompt = CanvasManager.DoorPrompt;
            fogDoorPrompt = CanvasManager.FogDoorPrompt;
            doorUI = CanvasManager.DoorUI;
            doorUIText = CanvasManager.DoorDescription;

            switch (CurrentDoorState_This)
            {
                
                case "Open":
                   
                    if (doorType == 0) { doorPrompt.SetActive(true); }
                    else { doorPrompt.SetActive(false); }
                    
                    break;
                case "FogEnter":
                    
                    if (doorType == 0) { fogDoorPrompt.SetActive(true);  }
                    else { fogDoorPrompt.SetActive(true);doorCollider.enabled = true; }

                    break;

                case "Fog":

                    doorPrompt.SetActive(false);
                    fogDoorPrompt.SetActive(false);

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
            fogDoorPrompt.SetActive(false);
            doorUI.SetActive(false);
            doorUIText.text = "Should not see this";
            collision.GetComponent<PlayerControllerV2>().Interactable = null;
        }
    }

    public void UseDoor()
    {
        Debug.Log("A");
        doorPrompt.SetActive(false);
        if (doorType == 1) //perpendicular 
        {
            Debug.Log("B");
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
                                        Anim.Play("PerpendicularDoorOpenIdle");
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
                                        Anim.Play("PerpendicularDoorOpenIdle");
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
                                        Anim.Play("PerpendicularDoorOpenIdle");
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
                case "FogEnter":
                    {
                        //move player in
                        //trigger boss fight
                        if (doorSaveManager.DoorTag_This == "E") 
                        {
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            DemonArena.EnterArena(); playerManager.gameObject.transform.position = DemonArena.transform.position; 
                        }
                        if (doorSaveManager.DoorTag_This == "T")
                        {
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            PursuerArena.EnterArena(); playerManager.gameObject.transform.position = PursuerArena.transform.position;
                            Anim.Play("PerpendicularDoorClosedIdle");
                        }
                        doorCollider.enabled = false;
                        break;
                    }
                case "Closed":
                    {
                        Debug.Log("C");
                        doorCollider.enabled = false;
                        CurrentDoorState_This = "Open";
                        Anim.Play("PerpendicularDoorOpenIdle"); 

                        if (doorSaveManager.DoorTag_This == "E" && DemonArena.currentState == "FirstTime") 
                        {
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            DemonArena.EnterArena(); playerManager.gameObject.transform.position = DemonArena.transform.position;
                            StartCoroutine(Wait(1));
                            CurrentDoorState_This = "Fog";
                        }

                        if (doorSaveManager.DoorTag_This == "T" && PursuerArena.currentState == "FirstTime")
                        {
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            PursuerArena.EnterArena(); playerManager.gameObject.transform.position = PursuerArena.transform.position;
                            StartCoroutine(Wait(1));
                            CurrentDoorState_This = "Fog";
                        }

                        if (doorSaveManager.DoorTag_This == "N")
                        {
                            AchievementsGameManager.Instance.UnlockedAchievement(1);
                        }

                        if (doorSaveManager.DoorTag_This == "N") { GetComponent<DoorOcludingSection>().RevealArea(); }
                        if (doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().RevealArea(); }
                        if (doorSaveManager.DoorTag_This == "U") { GetComponent<DoorOcludingSection>().RevealArea(); }

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
                        PC.PlayerFinishInteraction();
                        break;
                    }

                case "FogEnter":
                    {
                        layerManager.ChangeLayer(targetLayer);
                        WorldSaveGameManager.Instance.Player = playerManager;
                        WorldSaveGameManager.Instance.SaveGame();
                        if (doorSaveManager.DoorTag_This == "M1")
                        {
                            DemonArena.EnterArena();
                        }
                            break;
                    }


                case "Fog":
                    {
                        PC.PlayerFinishInteraction();
                        break;
                    }
                case "Open":
                    {
                        layerManager.ChangeLayer(targetLayer);

                        if (doorSaveManager.DoorTag_This == "F1")
                        {
                            if (DemonArena.currentState == "Active") { DemonArena.currentState = "Idle"; DemonArena.SwitchState("Idle"); } // DemonArena.SwitchState("Idle"); }
                        }

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
        Anim.Play("PerpendicularDoorClosedIdle");
    }

    public void SwitchDoorState(string state) //Used by boss arena manager
    {
        CurrentDoorState_This = state;
    }
}
