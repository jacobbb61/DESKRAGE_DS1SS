using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungeAssistant : MonoBehaviour
{
    public PlayerControllerV2 player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.CanPlunge = true;
        }
    }
}
