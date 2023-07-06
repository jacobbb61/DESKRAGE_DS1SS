using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsylumDemon : MonoBehaviour
{
    [Header("Stats")]
    public float Health;
    public float MaxHealth;
    public string Behaviour;

    public int LookDirection;
    public int CurrnetLookDirection;
    public bool IsFacingPlayer;
    public bool IsTurning;

    [Header("Bools")]
    public bool IsActive;
    public bool IsAttacking;
    public bool IsDead;

    [Header("Move and Detect Data")]
    public float Speed;
    public float AttackTriggerRange;

    [Header("Combat Data")]
    public float TimeToTurn;
    public float TimeBeforeAttack;
    public float AttackAnimationTime;
    public float AttackCoolDownTime;
    public float StaggerTime;
    public float AttackRange;
    public float AttackDamage;


    private Animator Anim;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;

    public GameObject Assets;
    public GameObject UIAssets;
    public Transform HitStartPos;
    public Slider HealthSlider;
    public Vector3 OriginPosition;


    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;


        IsAttacking = false;

        HealthSlider.maxValue = MaxHealth;

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }

        if (IsActive) { UIAssets.SetActive(true); }else { UIAssets.SetActive(false); }
    }

    public void Respawn()
    {
        if (IsDead == false)
        {
            Assets.SetActive(true);
            gameObject.SetActive(true);
            Health = MaxHealth;
            HealthSlider.value = Health;
            transform.localPosition = OriginPosition;
            Behaviour = "Idle";
        }
    }

    void Update()
    {
        UpdateUI();
        if (Health <= 0) { StartCoroutine(Death()); }
        else
        {
            IsDead = false;
            switch (Behaviour)
            {
                case "Idle":
                    FacePlayer();
                    if (IsActive) { Behaviour = "Hostile"; UIAssets.SetActive(true); }
                    Anim.Play("AsylumDemonAnim_Idle");
                    break;
                case "Hostile":
                    
                        FacePlayer();
                    if (IsFacingPlayer) { Walk(); }
                    
                    break;
                case "Attacking":
                    if (!IsAttacking) { AttackingCoroutine = StartCoroutine(Attack()); }
                    break;
                case "Parried":
                    break;
                case "Dead":
                    Dead();
                    break;
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
    }

    IEnumerator Death()
    {
        Behaviour = "Dead";
        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }
        HealthSlider.value = 0;
        //Anim.Play("UndeadAnim_A_Death");
        yield return new WaitForSeconds(3);

        Dead();

    }

    public void Dead()
    {
        Health = 0;
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

    void FacePlayer()
    {
        if (IsTurning == false)
        {
            if (transform.position.x > Player.transform.position.x)
            {
                LookDirection = 1;
            }
            else
            {
                LookDirection = -1;
            }

            if (CurrnetLookDirection != LookDirection)
            {
                IsFacingPlayer = false;
                StartCoroutine(Turn());
            }
        }
    }


    IEnumerator Turn()
    {
        IsTurning = true;
        Anim.Play("AsylumDemonAnim_Idle");
        yield return new WaitForSeconds(TimeToTurn);
        IsTurning = false;
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;
        IsFacingPlayer = true;
    }

    void Walk()
    {
        RB.velocity = new Vector2(-Speed * LookDirection, -5);
        Anim.Play("AsylumDemonAnim_Walk");
    }

    bool IsInAttackRange()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < AttackTriggerRange)
        {
            Anim.Play("AsylumDemonAnim_Idle");
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
        //Anim.Play("UndeadAnim_A_SwingAttack");
        yield return new WaitForSeconds(AttackAnimationTime);
        //Anim.Play("UndeadAnim_A_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime);
        IsAttacking = false;
        Behaviour = "Hostile";
    }

    public void AttackStep()
    {
        RB.velocity = new Vector2((-Speed - 1) * LookDirection, -5);
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
