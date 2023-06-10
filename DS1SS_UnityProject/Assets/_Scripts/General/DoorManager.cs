using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerControls;

public class DoorManager : MonoBehaviour
{
    private LayerManager layerManager;
    public bool isLocked = true;
    [Tooltip("ID number for differentiating between door type. 0 = regular, 1 = layer changing door, 2 = one-way, 3 = fog")][SerializeField] private int doorID = 0; // For differentiating between different kinds of doors
    public int targetLayer; // For doors that switch the player's layer
    [SerializeField] private Collider2D doorCollider;
    private bool bossKilled;

    // Start is called before the first frame update
    void Start()
    {
        layerManager = FindObjectOfType<LayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseDoor()
    {
        if (isLocked)
        {
            // Tell player the door is locked
        }
        else
        {
            switch(this.doorID)
            {
                case (0): // Regular doors
                    {
                        doorCollider.enabled = false;
                        Debug.Log("Case 0");
                        break;
                    }

                case (1): // Layer changing doors
                    {
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
    }
}
