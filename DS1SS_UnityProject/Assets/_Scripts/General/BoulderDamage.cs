using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDamage : MonoBehaviour
{
    public float Damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerV2>().PlayerTakeDamage(Damage, true, 0);
        }
    }
}
