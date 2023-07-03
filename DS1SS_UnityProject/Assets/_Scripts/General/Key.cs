using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] private GameObject correspondingDoor;
    //private DoorManager door;
    public string itemName;
    //private PlayerItems playerItems;
    private PlayerManager playerManager;
    
    [Tooltip("Current door tags: Cell, A, K, S")]public string targetDoorTag;

    // Start is called before the first frame update
    void Start()
    {
        //door = correspondingDoor.GetComponent<DoorManager>();
        itemName = gameObject.name;
        playerManager = FindObjectOfType<PlayerManager>();

    }

    public void PickUpKey()
    {
        // door.isLocked = false;
        switch (targetDoorTag)
        {
            case "A":
                {
                    playerManager.AKey = true;
                    break;
                }
            case "K":
                {
                    playerManager.KKey = true;
                    break;
                }
            case "S":
                {
                    playerManager.SKey = true;
                    break;
                }
            default:
                {
                    Debug.LogError("Error: Invalid target door tag on key " + this.gameObject.name);
                    break;
                }
        }

        Debug.Log(this.name + " picked up. " + correspondingDoor.gameObject + " unlocked");
        gameObject.SetActive(false);
    }
}
