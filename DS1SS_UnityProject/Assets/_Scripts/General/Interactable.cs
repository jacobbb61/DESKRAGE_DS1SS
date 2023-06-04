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
    [SerializeField] private Transform interactableText; // This will be the text telling the player they can interact with the object

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        interactableText = GameObject.FindGameObjectWithTag("Player").gameObject.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactionEvent != null)
        {
            Interact();
        }
    }

    internal void Interact()
    {
        interactionEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.targetInteractable = this;
            interactableText.gameObject.SetActive(true);
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

            interactableText.gameObject.SetActive(false);
            //Debug.Log("Player exits trigger");
        }
    }

}
