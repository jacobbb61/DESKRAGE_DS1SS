using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] private GameObject correspondingDoor;
    private DoorManager door;
    public string itemName;
    private PlayerItems playerItems;

    // Start is called before the first frame update
    void Start()
    {
        door = correspondingDoor.GetComponent<DoorManager>();
        itemName = gameObject.name;
        playerItems = FindObjectOfType<PlayerItems>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpKey()
    {
        door.isLocked = false;
        Debug.Log(this.name + " picked up. " + correspondingDoor.gameObject + " unlocked");
        playerItems.keys.Add(gameObject);
        gameObject.SetActive(false);

        //player manager. cellkey = true;
    }
}
