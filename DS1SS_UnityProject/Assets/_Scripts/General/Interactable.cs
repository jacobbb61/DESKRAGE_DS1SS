using System;
using UnityEngine;
using UnityEngine.Events;

//maybe do one of these in a future project instead of this system
public interface IInteractable
{
    void Interact();
}


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
        if (Input.GetKeyDown(KeyCode.E) && player.targetInteractable != null)
        {
            this.Interact();
        }
    }

    internal void Interact()
    {
        interactionEvent.Invoke();
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.targetInteractable = this;
            //Debug.Log("Player enters trigger");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(player.targetInteractable == this)
            {
                player.targetInteractable = null;
            }

            //Debug.Log("Player exits trigger");
        }
    }*/

}
