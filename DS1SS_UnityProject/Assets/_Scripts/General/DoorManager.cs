using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PlayerControls;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(Interactable))]
public class DoorManager : MonoBehaviour
{
    public GameObject canvas;

    [SerializeField] private bool CanInput = true;
    [SerializeField] private bool InRange;

    [SerializeField] private LayerManager layerManager;
    public bool isLocked = true;
    [Tooltip("ID number for differentiating between door type. 0 = regular, 1 = layer changing door, 2 = one-way, 3 = fog")][SerializeField] private int doorID = 0; // For differentiating between different kinds of doors
    [SerializeField] private int targetLayer; // For doors that switch the player's layer
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private bool bossKilled;
    [SerializeField] private Transform doorPrompt;
    [SerializeField] private Transform doorUI;
    [SerializeField] private TextMeshProUGUI doorUIText;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        doorPrompt = FindObjectOfType<Canvas>().transform.GetChild(2);
        layerManager = FindObjectOfType<LayerManager>();
        doorUI = FindObjectOfType<Canvas>().transform.GetChild(3);
        doorUIText = doorUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorPrompt.gameObject.SetActive(true); InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorPrompt.gameObject.SetActive(false); InRange = false;
            doorUI.gameObject.SetActive(false);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.action.triggered && InRange == true && CanInput)
        {
            Debug.Log("E Button Pressed");
            CanInput = false;
            UseDoor();
         }
    }
    



    public void UseDoor()
    {
        Debug.Log("try use door");

            if (isLocked)
            {
                doorPrompt.gameObject.SetActive(false);
                doorUIText.text = "This door is locked.";
                doorUI.gameObject.SetActive(true);
            }
            else
            {
                switch (doorID)
                {
                    case (0): // Regular doors
                        {
                            doorCollider.enabled = false;
                            Debug.Log(gameObject.name + "Case 0");
                            break;
                        }

                    case (1): // Layer changing doors
                        {
                            // Play animation, freeze player input
                            // Prevent damage
                            layerManager.ChangeLayer(targetLayer);
                            Debug.Log("Case 1");
                            break;
                        }

                    case (2): // One-way doors
                        {

                            Debug.Log("Case 2");
                            break;
                        }

                    case (3): // Fog doors
                        {
                            if (bossKilled)
                            {
                                doorCollider.enabled = false;
                            }
                            Debug.Log("Case 3");
                            break;
                        }
                    default:
                        {
                            Debug.LogError("Error: door ID (" + doorID + ") is invalid. Reassign this value in the inspector");
                            break;
                        }
                }
            
        }
        CanInput = true;
    }
}
