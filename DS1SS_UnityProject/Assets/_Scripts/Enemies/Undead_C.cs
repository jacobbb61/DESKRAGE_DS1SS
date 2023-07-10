using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Undead_C : MonoBehaviour
{

    [Header("Stats")]
    public float Health;
    public float MaxHealth;
    public string Behaviour;
    public int LookDirection;

    [Header("Move and Detect Data")]
    public float Speed;
    public float BaseSpeed;
    public float ForwardDetectionRange;
    public float BackwardsDetectionRange;
    public float AttackTriggerRange;
    public float VerticalSpeed;
    public float FallSpeed;
    public string SlopeDir;


    [Header("Combat Data")]
    public float TimeBeforeAttack;
    public float AttackAnimationTime;
    public float AttackCoolDownTime;
    public float StaggerTime;
    public float AttackRange;
    public float AttackDamage;

    public float CombatTime;

    public List<Collider2D> Overlap;

    [Header("Bools")]
    public bool SeePlayer;
    public bool IsAttacking;
    public bool IsAttackStepping;
    public bool IsDead;
    public bool IsAtOrigin;
    public bool IsGrounded;
    public bool IsOnSlope;
    public bool FootAOnSlope;
    public bool FootBOnSlope;

    private Animator Anim;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;

    public GameObject Assets;
    public GameObject Eyes;

    public Transform GroundCheckPosA;
    public Transform GroundCheckPosB;

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

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }
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
        IsByOriginPosition();
        GroundCheck();
        if (Health <= 0) { StartCoroutine(Death()); }
        else
        {
            IsDead = false;
            switch (Behaviour)
            {
                case "Idle":
                    LookForPlayer();
                    Anim.Play("UndeadAnim_C_Idle");
                    if (SeePlayer) { Behaviour = "Hostile"; }
                    if (!IsAtOrigin) { Behaviour = "Returning"; } else { RB.velocity = Vector2.zero; }
                    break;
                case "Hostile":
                    CombatTime += Time.deltaTime;
                    LookForPlayer();
                    if (SeePlayer && IsInAttackRange()) { Behaviour = "Attacking"; }
                    else if (SeePlayer)
                    {
                        FacePlayer();
                        Walk();
                    }
                    else
                    {
                        Anim.Play("UndeadAnim_C_Idle");
                        RB.velocity = Vector2.zero;
                    }
                    
                    if (!SeePlayer && !IsAtOrigin && CombatTime>8) {Behaviour = "Returning"; CombatTime = 0; }
                    break;
                case "Attacking":
                    if (!IsAttacking) { AttackingCoroutine =  StartCoroutine(Attack()); }
                    if (!IsAttackStepping)
                    {
                        RB.velocity = new Vector2(0, -VerticalSpeed);
                    }
                    break;
                case "Staggered":
                    StartCoroutine(Staggered());
                    RB.velocity = Vector2.zero;
                    break;
                case "Parried":
                    break;
                case "BackStep":
                    break;
                case "Returning":
                    LookForPlayer();
                    if (SeePlayer) { Behaviour = "Hostile"; }                  
                    if (!IsAtOrigin) { FaceOrigin(); WalkToOrigin(); GroundCheck(); }
                    else { Behaviour = "Idle"; }
                    break;
                case "Dead":
                    Dead();
                    RB.velocity = Vector2.zero;
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
        Anim.Play("UndeadAnim_C_Death");
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
        Behaviour = "Staggered";
    }
    IEnumerator Staggered()
    {  
        Anim.Play("UndeadAnim_C_GettingHit");
        yield return new WaitForSeconds(StaggerTime);
        Behaviour = "Hostile";
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
            int layerMask = ~(LayerMask.GetMask("Ignore Raycast"));
            RaycastHit2D hit = Physics2D.Raycast(Eyes.transform.position, (Player.transform.position - Eyes.transform.position), Range, layerMask);
            
            if (hit.transform != null)
            {
                
                
                if (hit.transform.CompareTag("Player"))
                {
                    SeePlayer = true;
                }
                else
                {
                    //Debug.Log("Undead C sees" + hit.transform.name);
                    SeePlayer = false;
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


    void GroundCheck()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hitA = Physics2D.Raycast(GroundCheckPosA.position, Vector2.down, .45f, layerMask);
            RaycastHit2D hitB = Physics2D.Raycast(GroundCheckPosB.position, Vector2.down, .45f, layerMask);

            if (hitA.collider != null)
            {
                if (hitA.transform.CompareTag("Ground"))
                {
                    IsGrounded = true;
                    FootAOnSlope = false;
                }
                else if (hitA.transform.CompareTag("Slope"))
                {
                    IsGrounded = true;
                    FootAOnSlope = true;
                }
                else
                {
                    IsGrounded = false;
                    FootAOnSlope = false;
                }
            }
            if (hitB.collider != null)
            {
                if (hitB.transform.CompareTag("Ground"))
                {
                    IsGrounded = true;
                    FootBOnSlope = false;
                }
                else if (hitB.transform.CompareTag("Slope"))
                {
                    IsGrounded = true;
                    FootBOnSlope = true;
                }
                else
                {
                    IsGrounded = false;
                    FootBOnSlope = false;
                }
            }
            if (hitA.collider == null && hitB.collider == null)
            {
                IsGrounded = false;
            }


            
                if (FootAOnSlope && FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
                else if (FootAOnSlope && !FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
                else if (!FootAOnSlope && FootBOnSlope && hitB.transform != null) { OnSlope(hitB.transform); }
                else if (!FootAOnSlope && !FootBOnSlope)
                {
                    //not on slope reset speeds
                    Speed = BaseSpeed;
                   if (IsGrounded) { VerticalSpeed = 0.5f; } else { VerticalSpeed = FallSpeed; }
                }
                 
    }

    void OnSlope(Transform Slope)
    {

        float SlopeAngle = Slope.transform.transform.eulerAngles.z;
        if (SlopeAngle <= 35 && SlopeAngle < 60)
        {
            // right  a 35 or less slope
            if (LookDirection == 1)
            { //going against slope
                Speed = 2f; SlopeDir = "a";
                if (FootAOnSlope && !FootBOnSlope) { VerticalSpeed = 0f; }
                else if (!FootAOnSlope && FootBOnSlope) { VerticalSpeed = 0f; }
                else { VerticalSpeed = 5f; }
            }
            else
            {//going with slope

                Speed = 4.5f; SlopeDir = "b";
                VerticalSpeed = 0f; 
            }

        }
        else if (SlopeAngle > 35 && SlopeAngle < 60)
        {
            // right  a 35 or MORE slope
            if (LookDirection == 1)
            { //going against slope
                Speed = 5f; SlopeDir = "c";
            }
            else
            {//going with slope
                Speed = 1f; SlopeDir = "d";
                if (FootAOnSlope && !FootBOnSlope) { Speed = 2.5f; VerticalSpeed = 0f; }
                if (!FootAOnSlope && FootBOnSlope) { Speed = 2.5f; VerticalSpeed = 0f; }
            }
        }
        if (SlopeAngle >= 325 && SlopeAngle > 300)
        {
            // left a 35 or less slope
            if (LookDirection == 1)
            { //going against slope
                Speed = 1f; SlopeDir = "e";
                if (FootAOnSlope && !FootBOnSlope) { Speed = 2.5f; VerticalSpeed = 0f; }
                if (!FootAOnSlope && FootBOnSlope) { Speed = 2.5f; VerticalSpeed = 0f; }
            }
            else
            {//going with slope
                Speed = 4f; SlopeDir = "f";
            }
        }
        else if (SlopeAngle < 325 && SlopeAngle > 300)
        {
            // left a 35 or MORE slope
            if (LookDirection == 1)
            { //going against slope
                Speed = 3f; SlopeDir = "g";
                 VerticalSpeed = 0f; 
            }
            else
            {//going with slope
                Speed = 4.5f; SlopeDir = "h";
            }
        }
    }

    void Walk()
    {
        if (IsGrounded)
        {
            RB.velocity = new Vector2(-Speed * LookDirection, -VerticalSpeed);
            Anim.Play("UndeadAnim_C_Walk");
        }
    }

    void IsByOriginPosition()
    {
        if (Vector3.Distance(transform.localPosition, OriginPosition) < 2)
        {
            IsAtOrigin = true;
        }
        else
        {
            IsAtOrigin = false;
        }
    }


    void FaceOrigin()
    {
        if (transform.localPosition.x > OriginPosition.x)
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
    void WalkToOrigin()
    {
        FaceOrigin();
        Speed = BaseSpeed;
        RB.velocity = new Vector2(-Speed * LookDirection, -VerticalSpeed);
        Anim.Play("UndeadAnim_C_Walk");
    }



    bool IsInAttackRange()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < AttackTriggerRange)
        {
            Anim.Play("UndeadAnim_C_Idle");
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
        Anim.Play("UndeadAnim_C_SwingAttack");
        yield return new WaitForSeconds(AttackAnimationTime);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime);
        IsAttacking = false;
        Behaviour = "Hostile";
        FacePlayer();
    }

    public void StartAttackStep()
    {

        RB.velocity = new Vector2(-Speed * LookDirection, -VerticalSpeed);
        IsAttackStepping = true;

    }
    public void StopAttackStep()
    {
        RB.velocity = Vector2.zero;
        IsAttackStepping = false;
    }

    public void AttackRegister()
    {
        int layerMask = ~(LayerMask.GetMask("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(-LookDirection, 0), AttackRange, layerMask);


        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PlayerControllerV2>().PlayerTakeDamage(AttackDamage, false, 0);
            }
        }
    }

    bool IsOnEnemy()
    {

        Collider2D coll = GetComponent<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (Physics2D.OverlapCollider(coll, filter, Overlap) > 0)
        {
            Debug.Log(Overlap);
        }


        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, Vector2.down, 0.1f);
  
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    IEnumerator StepBack()
    {
        Behaviour = "BackStep";
        RB.velocity = new Vector2(0,0);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(1f);
        Behaviour = "Hostile";
    }
    bool IsByWall()
    {

        int layerMask = ~(LayerMask.GetMask("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, new Vector2(LookDirection, 0), 1.5f, layerMask);


        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Wall"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
