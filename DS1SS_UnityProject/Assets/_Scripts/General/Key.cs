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

        switch (targetDoorTag)
        {
            case "A":
                {
                    if (playerManager.AKey == true)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    break;
                }
            case "K":
                {
                    if (playerManager.KKey == true)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    break;
                }
            case "S":
                {
                    if (playerManager.SKey == true)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    break;
                }
            default:
                {
                    // PANIC
                    break;
                }
        }
    }

    public void PickUpKey()
    {
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
