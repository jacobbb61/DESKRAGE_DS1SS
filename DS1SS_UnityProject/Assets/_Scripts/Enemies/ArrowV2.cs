using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowV2 : MonoBehaviour
{
    public Vector3 Target;
    public Rigidbody2D RB;
    public float speed;
    public float angle;
    public bool IsArrow;

    private void Start()
    {
        if (IsArrow) 
        {
            angle = Mathf.Atan2(Target.y - transform.position.y, Target.x - transform.position.x) * Mathf.Rad2Deg; 
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = targetRotation;
            RB.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }
        else
        {
           if (transform.position.x < Target.x)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.rotation = targetRotation;
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                transform.rotation = targetRotation;
            }
           
            RB.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }
 




    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerControllerV2>().IsRolling==false) 
            {             
            collision.GetComponent<PlayerControllerV2>().PlayerTakeDamage(20, false, 0);
            DestroyArrow(); 
            }
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
