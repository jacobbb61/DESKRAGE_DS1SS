using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject correspondingDoor;
    private DoorManager door;

    // Start is called before the first frame update
    void Start()
    {
        door = correspondingDoor.GetComponent<DoorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.isLocked = false;
        Debug.Log(correspondingDoor + " unlocked");
        gameObject.SetActive(false);
    }
}
