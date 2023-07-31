using System.Collections;
using System.Collections.Generic;
//using UnityEditor.AnimatedValues;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int Direction;
    public float yDirection;
    public float Speed;
    public float VerticalSpeed;
    public float RotationSpeed;
    public string SortingLayer;
    public bool Flying;
    public Rigidbody2D RB;
    public Quaternion from;
    public Quaternion to;
    public float ydis;

    // animate the game object from -1 to +1 and back
    public float minimum;
    public float maximum;

    // starting value for the Lerp
    static float t = 0.0f;
    public Vector3 target;
    private GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);
    }

    public void ManualStart()
    {
        t = 0;
        if (Direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 0, 10);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 190);
        }

        //float vertDis;
        float dis = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        

        if (dis <= 5)
        {
            minimum = 1;
        }
        if (dis <= 10 && dis >5)
        {
            minimum = 2;
        }
        if (dis <= 15 && dis > 10)
        {
            minimum = 2.5f;
        }
        if (dis > 15)
        {
            minimum = 3.5f;
        }
    }

    void Update()
    {
        if (Flying)
        {
            yDirection = 0;
            target = player.transform.position;
            VerticalSpeed = Mathf.Lerp(minimum, maximum, t);
            t += 0.5f * Time.deltaTime;

            ydis = (transform.position.y - player.transform.position.y);

            //float yDisMultiplier = 0;
            //need a yDisMultiplier formula for more accurate results
            
            yDirection = (ydis * -1) + 0.5f;

            RB.AddForce(new Vector3(Direction * Speed, yDirection, VerticalSpeed));
            if (Direction == -1)
            {
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * RotationSpeed));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * -RotationSpeed));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerV2>().PlayerTakeDamage(6, false, 0);
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
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
 }
