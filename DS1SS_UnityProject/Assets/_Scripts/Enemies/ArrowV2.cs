using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ArrowV2 : MonoBehaviour
{
    public Vector3 Target;
    public Rigidbody2D RB;
    public float speed;
    public float angle;
    public bool IsArrow;
    public bool IsDown;
    public Animator Anim;
    public EventReference ArrowBreakAudioRef;

    private void Start()
    {
        if (IsArrow) 
        {
            angle = Mathf.Atan2(Target.y - transform.position.y, Target.x - transform.position.x) * Mathf.Rad2Deg; 
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = targetRotation;
            RB.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }
        else if (IsDown)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            transform.rotation = targetRotation;
            int ran = Random.Range(1, 3);
            if (ran == 1) { transform.localScale = new Vector3(1, 1, 1); } else { transform.localScale = new Vector3(1, -1, 1); }
            Anim.Play("CursedShockWaveVFX");
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
            Anim.Play("CursedShockWaveVFX");
            RB.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }


        StartCoroutine(SafetyDestroy());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerControllerV2>().IsRolling==false) 
            {             
            collision.GetComponent<PlayerControllerV2>().PlayerTakeDamage(15, false, 0);
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
        if (IsArrow) { 
            Destroy(gameObject);
            RuntimeManager.PlayOneShot(ArrowBreakAudioRef, transform.position);
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Anim.Play("CursedShockWaveVFX_Impact");
            RB.velocity = Vector2.zero;
            Destroy(gameObject,0.25f);

        }
    }

    IEnumerator SafetyDestroy()
    {
        yield return new WaitForSeconds(15);
        DestroyArrow();
    }

    private void OnDisable()
    {
        DestroyArrow();
    }
}
