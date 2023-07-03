using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableV2))]
public class TutorialTextDisplay : MonoBehaviour
{
    private CanvasManager canvasManager;
    public string displayText;
    private PlayerControllerV2 PC;


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
            canvasManager.TutorialText.text = "You should not be reading this, maidenless one";
        }
    }

    public void TutorialInteract()
    {
        Debug.Log("Tutorial interacted");
        canvasManager.TutorialPrompt.SetActive(false);
        canvasManager.TutorialText.text = displayText;
        canvasManager.TutorialMessage.SetActive(true);
        PC.PlayerFinishInteraction();
    }
}
