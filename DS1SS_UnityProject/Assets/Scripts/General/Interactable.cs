using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    /*needs a UnityEvent to invoke when the player presses a button while in this trigger collider
     * In this case, when the player (PlayerController.cs) enters this collider, tell them that their targetInteractable is this script
     * When they exit this collider, tell them their targetInteractable is null
     * When they push interact, they'll call this scripts Interact() function
     * 
    */

    private PlayerController player;
    [SerializeField] private UnityEvent interactionEvent;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Interact()
    {
        interactionEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.targetInteractable = this;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(player.targetInteractable == this)
        {
            player.targetInteractable = null;
        }
    }
}
