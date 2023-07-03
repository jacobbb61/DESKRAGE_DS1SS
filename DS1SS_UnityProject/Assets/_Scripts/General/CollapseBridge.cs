using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    private Rigidbody2D rb;
    public string currentState;
    private int floorHealth = 3;
    private Rigidbody2D playerRB;
    private Collider2D bridgeCollider;
    [Tooltip("True = this gameobject is a bridge. False = this gameobject is a collapsable floor.")]public bool isBridge;

    // The bridge referenced in this script must have a collider and kinematic rigidbody

    void Start()
    {
        rb = bridge.GetComponent<Rigidbody2D>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        bridgeCollider = GetComponent<Collider2D>();
        if (!isBridge)
        {
            currentState = "Unbroken";
        }
        else
        {
            currentState = "Closed";
        }
    }

    private void Update()
    {
        if (floorHealth <= 0 && !isBridge)
        {
            rb.isKinematic = false;
            gameObject.SetActive(false);
            currentState = "Broken";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBridge /* && playerRB.velocity.y < 0*/)
        {
            floorHealth--;
            if (floorHealth == 2)
            {
                currentState = "Breaking1";
            }
            else if (floorHealth == 1)
            {
                currentState = "Breaking 2";
            }
            else
            {
                // PANIC
            }
        }
        else if (collision.CompareTag("Player") && isBridge)
        {
            // Play animation
            bridgeCollider.enabled = false;
            // Wait();
            // this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != ("Player"))
        {
            this.gameObject.SetActive(false);
            currentState = "Broken";
        }
    }

    IEnumerator Wait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
    }
}
