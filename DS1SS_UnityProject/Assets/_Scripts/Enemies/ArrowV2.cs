using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowV2 : MonoBehaviour
{
    public Vector3 Target;
    public Rigidbody2D RB;
    public float speed;

    private void Start()
    {
        float angle = Mathf.Atan2(Target.y - transform.position.y, Target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = targetRotation;
        RB.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerV2>().PlayerTakeDamage(20, false, 0);
            DestroyArrow();
        }
        if (collision.CompareTag("Ground"))
        {
            DestroyArrow();
        }
        if (collision.CompareTag("Slope"))
        {
            DestroyArrow();
        }
        if (collision.CompareTag("Wall"))
        {
            DestroyArrow();
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
