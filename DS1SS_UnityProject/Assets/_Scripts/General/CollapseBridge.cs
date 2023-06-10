using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    private Rigidbody2D rb;

    // The bridge referenced in this script must have a collider and kinematic rigidbody

    void Start()
    {
        rb = bridge.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger");
            rb.isKinematic = false;
            gameObject.SetActive(false);
        }
    }
}
