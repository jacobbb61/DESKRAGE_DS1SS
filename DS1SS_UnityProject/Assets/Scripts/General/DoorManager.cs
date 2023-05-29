using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerControls;

public class DoorManager : MonoBehaviour
{
    private LayerManager layerManager;
    public bool isLocked = true;
    [Tooltip("ID number for differentiating between door type")]public int doorID = 0; // For differentiating between different kinds of doors
    public int targetLayer; // For doors that switch the player's layer

    // Start is called before the first frame update
    void Start()
    {
        layerManager = FindObjectOfType<LayerManager>();
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
            layerManager.ChangeLayer(targetLayer);
        }
    }
}
