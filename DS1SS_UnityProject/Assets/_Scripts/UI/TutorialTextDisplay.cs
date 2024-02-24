using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableV2))]
public class TutorialTextDisplay : MonoBehaviour
{
    private CanvasManager canvasManager;
    private PlayerControllerV2 PC;
    public GameObject Message;


    private void Start()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
        PC = FindObjectOfType<PlayerControllerV2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasManager.TutorialPrompt.SetActive(true);
            collision.GetComponent<PlayerControllerV2>().Interactable = GetComponent<InteractableV2>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasManager.TutorialPrompt.SetActive(false);
            collision.GetComponent<PlayerControllerV2>().Interactable = null;
            canvasManager.TutorialMessage.SetActive(false);
            Message.SetActive(false);
        }
    }

    public void TutorialInteract()
    {
        Debug.Log("Tutorial interacted");
        canvasManager.TutorialPrompt.SetActive(false);
        canvasManager.TutorialMessage.SetActive(true);
        Message.SetActive(true);
        PC.PlayerFinishInteraction();
    }
}
