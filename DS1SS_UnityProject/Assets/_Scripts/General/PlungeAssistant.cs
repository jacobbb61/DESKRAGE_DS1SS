using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungeAssistant : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerV2>().CanPlunge = true;
        }
    }
}
