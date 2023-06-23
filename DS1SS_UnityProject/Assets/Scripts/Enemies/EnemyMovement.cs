using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D enemyBody;
    SpriteRenderer myArt;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float acceleration = 2f;
    private Vector2 velo;
    private int direction = 1;
    [SerializeField] float maxAttackRange, minAttackRange;

    [SerializeField] Vector3 origin;
    [SerializeField] float range = 5;
    [SerializeField] LayerMask ground;
    [SerializeField] float eyeRange = 9;

    [SerializeField] float attackCooldown;

    bool playerSpotted;
    bool attacked;
    bool blocking;

    PlayerController player;
    void Start()
    {

        enemyBody = GetComponent<Rigidbody2D>();
        myArt = GetComponent<SpriteRenderer>();
        if(FindAnyObjectByType<PlayerController>() != null)
            player = FindAnyObjectByType<PlayerController>();

        origin = transform.position;

        StartCoroutine(WalkCycle());
    }

    IEnumerator WalkCycle()
    {
        float walkAmount = Random.Range(1.0f, range);

        if(transform.position.x <= origin.x)
        {
            direction = 1;
            myArt.flipX = true;
        }
        else
        {
            walkAmount *= -1;
            direction = -1;
            myArt.flipX = false;
        }

        Vector3 targetLocation = origin + Vector3.right * walkAmount;

        //print("Target Location: " + targetLocation);

        if (walkAmount > 0)
            yield return new WaitUntil(() => transform.position.x >= targetLocation.x);
        else
            yield return new WaitUntil(() => transform.position.x <= targetLocation.x);

        velo = new Vector3();

        StartCoroutine(WalkCycle());
    }

    IEnumerator FollowPlayer()
    {

        yield return null;

    }

    IEnumerator AttackCycle()
    {
        attacked = true;

        gameObject.SendMessage("Attack", SendMessageOptions.DontRequireReceiver); //each attack will run off of a custom attack script for each enemy

        yield return new WaitForSeconds(attackCooldown);

        attacked = false;
    }

    private void FixedUpdate()
    {

        if (velo.x < moveSpeed)
            velo.x += acceleration * Time.deltaTime * direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5, ground);

        print(hit.point);

        transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);

        enemyBody.velocity = velo;

        if(playerSpotted && !attacked && Vector2.Distance(transform.position, player.transform.position) <= maxAttackRange)
        {

            while (transform.position.x == -direction * minAttackRange + player.transform.position.x)
            {
                velo = new Vector3();
            }
        }

    }

    private void Update()
    {

        RaycastHit2D playerSpot = Physics2D.Raycast(transform.position, new Vector2(direction, 0), eyeRange);

        if(player != null && playerSpot.collider.gameObject == player.gameObject || playerSpotted )
        {
            playerSpotted = true;

            direction = transform.position.x < player.transform.position.x ? 1 : -1;

        }

    }

}
