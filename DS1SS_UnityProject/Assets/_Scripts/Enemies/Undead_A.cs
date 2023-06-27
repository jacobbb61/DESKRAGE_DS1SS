using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Undead_A : MonoBehaviour
{

    [Header("Stats")]
    public float Health;
    public float MaxHealth;
    public string Behaviour;
    public int LookDirection;

    [Header("Move and Detect Data")]
    public float Speed;
    public float ForwardDetectionRange;
    public float BackwardsDetectionRange;
    public float AttackTriggerRange;

    [Header("Combat Data")]
    public float TimeBeforeAttack;
    public float AttackAnimationTime;
    public float AttackCoolDownTime;
    public float AttackRange;
    public float AttackDamage;

    [Header("Bools")]
    public bool SeePlayer;
    public bool IsAttacking;
    public bool IsDead;


    private Animator Anim;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;

    public GameObject Assets;
    public GameObject Eyes;
    public Transform HitStartPos;
    public Slider HealthSlider;
    public Vector3 OriginPosition;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }

        IsAttacking = false;

        HealthSlider.maxValue = MaxHealth;
    }

    public void Respawn()
    {
        IsDead = false;
        Assets.SetActive(true);
        gameObject.SetActive(true);
        Health = MaxHealth;
        HealthSlider.value = Health;
        transform.localPosition = OriginPosition;
        Behaviour = "Idle";
    }

    void Update()
    {   
        UpdateUI();
        if (Health <= 0) { StartCoroutine(Death()); }
        else
        {
            switch (Behaviour)
            {
                case "Idle":
                    LookForPlayer();
                    Anim.Play("UndeadAnim_A_Idle");
                    if (SeePlayer) { Behaviour = "Hostile"; }
                    break;
                case "Hostile":
                    if (IsInAttackRange()) { Behaviour = "Attacking"; }
                    else
                    {
                        FacePlayer();
                        Walk();
                    }
                    break;
                case "Attacking":        
                    if (!IsAttacking) { AttackingCoroutine =  StartCoroutine(Attack()); } 
                    break;
                case "Staggered":
                    break;
                case "Parried":
                    break;
                case "Returning":
                    break;
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
    }

    IEnumerator Death()
    {
        StopCoroutine(AttackingCoroutine);
        HealthSlider.value = 0;
        Anim.Play("UndeadAnim_A_Death");
        yield return new WaitForSeconds(3);

        Dead();

    }

    public void Dead()
    {
        IsDead = true;
        Assets.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TakeLightDamage()
    {
        Health -= 6;
    }
    public void TakeHeavyDamage()
    {
        Health -= 9;
    }

    void UpdateUI()
    {
        HealthSlider.value = Health;
    }

    void LookForPlayer()
    {

        float Range = 0;

        if (transform.position.x > Player.transform.position.x && LookDirection == 1) //looking towards player
        {
            Range = ForwardDetectionRange;
        }
        if (transform.position.x < Player.transform.position.x && LookDirection == -1) //looking towards player
        {
            Range = ForwardDetectionRange;
        }
        if (transform.position.x > Player.transform.position.x && LookDirection == -1) //looking away from player
        {
            Range = BackwardsDetectionRange;
        }
        if (transform.position.x < Player.transform.position.x && LookDirection == 1) //looking away from player
        {
            Range = BackwardsDetectionRange;
        }


        
        
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < Range)
        {
            RaycastHit2D hit = Physics2D.Raycast(Eyes.transform.position, (Player.transform.position - Eyes.transform.position), Range);
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Player"))
                {
                    SeePlayer = true;
                }
              
            } 
            else
                {
                    SeePlayer = false;
                }
        }
        else
        {
            SeePlayer = false;
        }
    }
    void FacePlayer()
    {
        if(transform.position.x > Player.transform.position.x)
        {
            LookDirection = 1;
        }
        else
        {
            LookDirection = -1;
        }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }
    }

    void Walk()
    {
        RB.velocity = new Vector2(-Speed * LookDirection, -5);      
        Anim.Play("UndeadAnim_A_Walk");
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) > ForwardDetectionRange)
        {
            Behaviour = "Idle";
        }

    }

    bool IsInAttackRange()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < AttackTriggerRange)
        {
            Anim.Play("UndeadAnim_A_Idle");
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        yield return new WaitForSeconds(TimeBeforeAttack);
        FacePlayer();
        Anim.Play("UndeadAnim_A_SwingAttack");
        yield return new WaitForSeconds(AttackAnimationTime);
        Anim.Play("UndeadAnim_A_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime);
        IsAttacking = false;
        Behaviour = "Hostile";
    }

    public void AttackStep()
    {
        RB.velocity = new Vector2((-Speed -1) * LookDirection, -5);
    }

    public void AttackRegister()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(-LookDirection, 0), AttackRange);


        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PlayerControllerV2>().PlayerTakeDamage(AttackDamage, false, 0);
            }
        }
    }
}
