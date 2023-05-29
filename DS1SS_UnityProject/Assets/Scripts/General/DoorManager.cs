using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerControls;

public class DoorManager : MonoBehaviour
{
    private LayerManager layerManager;
    public bool isLocked = true;
    [Tooltip("ID number for differentiating between door type.")]public int doorID = 0; // For differentiating between different kinds of doors
    public int targetLayer; // For doors that switch the player's layer
    private Collider2D doorCollider;
    private bool bossKilled;

    // Start is called before the first frame update
    void Start()
    {
        layerManager = FindObjectOfType<LayerManager>();
        doorCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLayer()
    {
        if (isLocked)
        {
            // Tell player the door is locked
        }
        else
        {
            switch(doorID)
            {
                case 0: // Regular doors
                    {
                        doorCollider.enabled = false;
                        break;
                    }

                case 1: // Layer changing doors
                    {
                        layerManager.ChangeLayer(targetLayer);
                        break;
                    }

                case 2: // One-way doors
                    {
                        break;
                    }

                case 3: // Fog doors
                    {
                        if (bossKilled)
                        {
                            doorCollider.enabled = false;
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogError("Error: door ID (" + doorID + ") is invalid. Reassign this value in the inspector");
                        break;
                    }
            }
        }
    }
}
