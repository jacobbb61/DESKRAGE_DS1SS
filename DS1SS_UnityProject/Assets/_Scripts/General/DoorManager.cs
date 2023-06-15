using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public LayerManagerV2 layerManager;
    public bool isLocked = true;
    [Tooltip("ID number for differentiating between door type. 0 = regular, 1 = layer changing door, 2 = one-way, 3 = fog")][SerializeField] private int doorType; // For differentiating between different kinds of doors
    public int targetLayer; // For doors that switch the player's layer
    private bool bossKilled;

    [SerializeField] private CanvasManager CanvasManager;
    [SerializeField] private Collider2D doorCollider; 
    [SerializeField] private GameObject doorPrompt;
    [SerializeField] private GameObject doorUI;
    [SerializeField] private TextMeshProUGUI doorUIText;

    /*
    private void Awake()
    {
        Collider2D[] overlaps = Physics2D.OverlapPointAll(transform.position);
        DoorManager otherDoor;
        foreach (Collider2D c in overlaps)
        {
            if (c.gameObject.TryGetComponent<DoorManager>(out otherDoor) && otherDoor != this)
            {
                targetLayer = FindObjectOfType<LayerManager>().GetLayerFromObject(c.gameObject);
            }
        }
    }*/

    void Start()
    {
        layerManager = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();

        CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        doorPrompt = CanvasManager.DoorPrompt;
        doorUI = CanvasManager.DoorUI;
        doorUIText = CanvasManager.DoorDescription;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorPrompt.SetActive(true);
            collision.GetComponent<PlayerControllerV2>().Interactable = GetComponent<InteractableV2>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorPrompt.SetActive(false);
            doorUI.SetActive(false);
            collision.GetComponent<PlayerControllerV2>().Interactable = null;
        }
    }

    public void UseDoor()
    {
        if (isLocked)
        {
            doorPrompt.SetActive(false);
            doorUIText.text = "This door is locked.";
            doorUI.SetActive(true);
        }
        else
        {
            switch(doorType)
            {
                case (0): // Regular doors
                    {
                        doorCollider.enabled = false;
                        Debug.Log("Door Case 0");
                        break;
                    }

                case (1): // Layer changing doors
                    {
                        // Play animation, freeze player input
                        // Prevent damage
                        layerManager.ChangeLayer(targetLayer);
                        Debug.Log("Door Case 1");
                        break;
                    }

                case (2): // One-way doors
                    {
                        
                        Debug.Log("Door Case 2");
                        break;
                    }

                case (3): // Fog doors
                    {
                        if (bossKilled)
                        {
                            doorCollider.enabled = false;
                        }
                        Debug.Log("Door Case 3");
                        break;
                    }
                default:
                    {
                        Debug.LogError("Error: door ID (" + doorType + ") is invalid. Reassign this value in the inspector");
                        break;
                    }
            }
        }
    }
}
