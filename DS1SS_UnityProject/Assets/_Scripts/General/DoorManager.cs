using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FMODUnity;

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
    public int DoorMaterial;



    public EventReference FogEnterAudioRef;
    public EventReference DoorClosedAudioRef;

    public EventReference DoorClosed2AudioRef;
    public EventReference OpenAudioRef;
    public EventReference NightmareRef;





    public string CurrentDoorState_This;

     private CanvasManager CanvasManager;
    [SerializeField] private Collider2D doorCollider;
    public GameObject doorPrompt;
     private GameObject fogDoorPrompt;
     private GameObject doorUI;
     public GameObject FogAssets;
     private TextMeshProUGUI doorUIText;

    public DoorSaveManager doorSaveManager;

    public Animator Anim;
    public GameObject NightmareObj;
    public Animator NightmareAnim;

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
                    if (Anim.gameObject.activeInHierarchy) {PlayOpen(); }
                    ParticleSystem.Stop();
                    break;
                case "Closed":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Stop();
                    break;
                case "Locked":
                    doorCollider.enabled = true;
                    PlayClosed(); 
                    ParticleSystem.Stop();
                    break;
                case "OneSided":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Stop();
                    break;
                case "Fog":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Play();
                    break;
                case "FogEnter":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Play();
                    break;
            }
        }
    }
    public void PlayClosed()
    {
        switch (DoorMaterial)
        {
            case 0:
                Anim.Play("PerpendicularDoorClosedIdle");
                break;
            case 1:
                Anim.Play("PerpendicularDoorClosedIdle_Wood");
                break;
            case 2:
                Anim.Play("PerpendicularDoorClosedIdle_Wood");
                break;
        }
    }
    public void PlayOpen()
    {
        switch (DoorMaterial)
        {
            case 0:
                Anim.Play("PerpendicularDoorOpenIdle");
                break;
            case 1:
                Anim.Play("PerpendicularDoorOpenIdle_Wood");
                break;
            case 2:
                Anim.Play("PerpendicularDoorOpenIdle_Iron");
                break;
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
                    if (Anim.gameObject.activeInHierarchy) {PlayOpen(); }
                   // ParticleSystem.Stop();
                    ParticleSystem.Stop();
                    if (doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().Open(); }
                    if (doorSaveManager.DoorTag_This == "U") { GetComponent<DoorOcludingSection>().Open(); }
                    break;
                case "Closed":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Stop();
                    break;
                case "Locked":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Stop();
                    break;
                case "OneSided":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    ParticleSystem.Stop();
                    break;
                case "Fog":
                    Debug.Log("Close Door");
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
                    //ParticleSystem.Play();
                    ParticleSystem.Play();
                    break;
                case "FogEnter":
                    doorCollider.enabled = true;
                    if (Anim.gameObject.activeInHierarchy) { PlayClosed(); }
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

         if(doorSaveManager.DoorTag_This == "H") { GetComponent<DoorOcludingSection>().ManualStart(); }
         if(doorSaveManager.DoorTag_This == "U") { GetComponent<DoorOcludingSection>().ManualStart(); }



         if (inBossFight) { doorCollider.enabled = true; }
        
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
        if (PC == null)
        {
            PC = FindObjectOfType<PlayerControllerV2>();
        }


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
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        PlayOpen();
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
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
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        PlayOpen();
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
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
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        PlayOpen();
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }
                            case "N":
                                {
                                    if (playerManager.NKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        PlayOpen();
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";

                                        playerManager.GetComponent<PlayerControllerV2>().IsImmune = true;
                                        RuntimeManager.PlayOneShot(NightmareRef, transform.position);
                                        NightmareObj.SetActive(true);
                                        NightmareAnim.SetTrigger("Nightmare");
                                        StartCoroutine(Nightmare(2.5f));

                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                        doorUIText.text = "Locked";
                                    }
                                    break;
                                }

                            default:
                                {
                                    doorUI.SetActive(true);
                                    doorUIText.text = "Locked";
                                    RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                                    RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                    break;
                                }
                        }
                        break;
                    }
                case "FogEnter":
                    {
                        //move player in
                        //trigger boss fight

                        doorCollider.enabled = false;
                        RuntimeManager.PlayOneShot(FogEnterAudioRef, transform.position);

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
                            PlayClosed();
                        }
                        doorCollider.enabled = true;
                        break;
                    }
                case "Closed":
                    {
                        Debug.Log("C");
                        doorCollider.enabled = false;
                        CurrentDoorState_This = "Open";
                       PlayOpen(); 

                        if (doorSaveManager.DoorTag_This == "E" && DemonArena.currentState == "FirstTime")
                        {
                            RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            DemonArena.EnterArena(); playerManager.gameObject.transform.position = DemonArena.transform.position;
                            StartCoroutine(Wait(1));
                            CurrentDoorState_This = "Fog";
                        }

                        if (doorSaveManager.DoorTag_This == "T" && PursuerArena.currentState == "FirstTime")
                        {
                            RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                            WorldSaveGameManager.Instance.Player = playerManager;
                            WorldSaveGameManager.Instance.SaveGame();
                            PursuerArena.EnterArena(); playerManager.gameObject.transform.position = PursuerArena.transform.position;
                            StartCoroutine(Wait(1));
                            CurrentDoorState_This = "Fog";
                        }

                        if (doorSaveManager.DoorTag_This == "N")
                        {
                            RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                            AchievementsGameManager.Instance.UnlockedAchievement(1);
                        }

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
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                    }
                                    break;
                                }

                            case "K":
                                {
                                    if (playerManager.KKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                    }
                                    break;
                                }

                            case "S":
                                {
                                    if (playerManager.SKey == true)
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Door unlocked";
                                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);
                                        //Anim.Play("");
                                        //Wait();
                                        doorCollider.enabled = false;
                                        CurrentDoorState_This = "Open";
                                    }
                                    else
                                    {
                                        doorUI.SetActive(true);
                                        doorUIText.text = "Locked";
                                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                    }
                                    break;
                                }

                            default:
                                {
                                    doorUI.SetActive(true);
                                    doorUIText.text = "Locked";
                                    RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                                    break;
                                }
                        }
                        break;
                    }

                case "OneSided":
                    {
                        doorUI.SetActive(true);
                        doorUIText.text = "Does not open from this side";
                        RuntimeManager.PlayOneShot(DoorClosedAudioRef, transform.position);
                        RuntimeManager.PlayOneShot(DoorClosed2AudioRef, transform.position);
                        PC.PlayerFinishInteraction();
                        break;
                    }

                case "FogEnter":
                    {
                        layerManager.ChangeLayer(targetLayer);
                        WorldSaveGameManager.Instance.Player = playerManager;
                        WorldSaveGameManager.Instance.SaveGame();

                        RuntimeManager.PlayOneShot(FogEnterAudioRef, transform.position);

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
                        RuntimeManager.PlayOneShot(OpenAudioRef, transform.position);

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
        PlayClosed();
    }
    private IEnumerator Nightmare(float waitSeconds)
    {


        yield return new WaitForSeconds(waitSeconds);

        WorldSaveGameManager.Instance.LoadNightmare();
    }
    public void SwitchDoorState(string state) //Used by boss arena manager
    {
        CurrentDoorState_This = state;
    }
}
