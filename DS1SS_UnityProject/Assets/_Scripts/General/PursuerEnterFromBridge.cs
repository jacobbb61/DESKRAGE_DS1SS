using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuerEnterFromBridge : MonoBehaviour
{
    public PursuerArena PursuerArena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PursuerArena.EnterArena();
        }
    }
}
