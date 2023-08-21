using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
public class Undead_A : MonoBehaviour
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

    [Header("Bools")]
    public bool SeePlayer;
    public bool IsAttacking;
    public bool IsAttackStepping;
    public bool IsDead;
    public bool IsDying;
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
    public GameObject ParryIndicator;

    public Transform GroundCheckPosA;
    public Transform GroundCheckPosB;

    public Collider2D HitPos;
    public Slider HealthSlider;
    public Vector3 OriginPosition;
    public TextMeshProUGUI DamagerNumber;
    public int DamageTakenInTime;

    [Header("Audio")]
    public EventReference GruntAudio;
    public EventReference HitAudio;
    public EventReference DeathAudio;

    public EnemySaveManager EnemySaveManager;

    private float RandomGruntTime;
    private float RandomGruntTarget;

    private void Start()
    {
        RandomGruntTarget = Random.Range(3, 10);
        RandomGruntTime = 0;
        ManualStart();
    }

    public void ManualStart()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }

        IsAttacking = false;
        IsDying = false;
        HealthSlider.maxValue = MaxHealth;

        if (Health > 0) { IsDead = false; IsDying = false; EnemySaveManager.IsLockOnAble = true; } else { Dead(); Behaviour = "Dead"; }
    }

    public void Respawn()
    {
        EnemySaveManager.IsLockOnAble = true;
        DamagerNumber.gameObject.SetActive(false);
        SeePlayer = false;
        IsDead = false;
        Assets.SetActive(true);
        gameObject.SetActive(true);
        Health = MaxHealth;
        HealthSlider.value = Health;
        transform.localPosition = OriginPosition;
        Behaviour = "Idle";
        SeePlayer = false;
    }

    void Update()
    {   
        UpdateUI();
        IsByOriginPosition();
        GroundCheck();
        if (Health > 0)
        {
            IsDead = false; IsDying = false;
            switch (Behaviour)
            {
                case "Idle":
                    LookForPlayer();
                    ParryIndicator.SetActive(false);
                    Anim.Play("UndeadAnim_A_Idle");
                    if (SeePlayer) { Behaviour = "Hostile"; }
                    if (!IsAtOrigin) { Behaviour = "Returning"; } else { RB.velocity = Vector2.zero; }

                    if (RandomGruntTime >= RandomGruntTarget) 
                    { 
                        RandomGruntTime = 0; 
                        RandomGruntTarget = Random.Range(2, 8);
                        RuntimeManager.PlayOneShot(GruntAudio, transform.position);

                    } 
                    else
                    {
                        RandomGruntTime += Time.deltaTime;
                    }




                    break;
                case "Hostile":
                    CombatTime += Time.deltaTime;
                    LookForPlayer();
                    ParryIndicator.SetActive(false);
                    if (SeePlayer && IsInAttackRange()) { Behaviour = "Attacking"; }
                    else if (SeePlayer)
                    {
                        FacePlayer();
                        Walk();
                    }
                    else 
                    {
                        Anim.Play("UndeadAnim_A_Idle");
                        RB.velocity = Vector2.zero;
                    }
                    if (!SeePlayer && !IsAtOrigin && CombatTime>4) {Behaviour = "Returning"; CombatTime = 0; }
                    break;
                case "Attacking":
                    if (!IsAttacking) { AttackingCoroutine = StartCoroutine(Attack()); }
                    if (!IsAttackStepping)
                    {
                        RB.velocity = new Vector2(0, -VerticalSpeed);
                    }
                    break;
                case "Staggered":
                    ParryIndicator.SetActive(false);
                    if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
                    StartCoroutine(Staggered());
                    RB.velocity = Vector2.zero;
                    break;
                case "Parried":
                    ParryIndicator.SetActive(false);
                    break;
                case "BackStep":
                    break;
                case "Returning":
                    ParryIndicator.SetActive(false);
                    LookForPlayer();
                    if (SeePlayer) { Behaviour = "Hostile"; }
                    if (!IsAtOrigin) { FaceOrigin(); WalkToOrigin(); GroundCheck(); }
                    else { Behaviour = "Idle"; }
                    break;
                case "Dying":
                    ParryIndicator.SetActive(false);
                    RB.velocity = Vector2.zero;
                    break;
                case "Dead":
                    ParryIndicator.SetActive(false);
                    Dead();
                    RB.velocity = Vector2.zero;
                    break;
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
    }


    void Death()
    {
        
            IsDying = true;
            DamagerNumber.gameObject.SetActive(false);
            EnemySaveManager.IsLockOnAble = false;

            EnemySaveManager.IsLockOnAble = false;
            RB.velocity = Vector2.zero;
            IsAttacking = false;
            EnemySaveManager.CanBeParry = false;

            Behaviour = "Dying";
            StopAllCoroutines();
            HealthSlider.value = 0;
            Anim.Play("UndeadAnim_A_Death");
            StartCoroutine(DeathWait());
      
    }
    IEnumerator DeathWait()
    {

        yield return new WaitForSeconds(3);

        Dead();

    }
    private void OnDisable()
    {
        if (Behaviour == "Dying") { Dead(); }
        DamagerNumber.gameObject.SetActive(false);
        DamageTakenInTime = 0;
        DamagerNumber.text = DamageTakenInTime.ToString();
    }

    public void Dead()
    {
        Behaviour = "Dead";
        Health = 0;
        IsDead = true;
        Assets.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TakeLightDamage()
    {
        if (!IsDying)
        {
            Health -= 5;
            AddDamage(5);
            RuntimeManager.PlayOneShot(HitAudio, transform.position);
            if (Health <= 0) { Death(); RuntimeManager.PlayOneShot(DeathAudio, transform.position); }
        }
    }
    public void TakeHeavyDamage()
    {
        if (!IsDying)
        {
            Health -= 10;
            AddDamage(10);
            RuntimeManager.PlayOneShot(HitAudio, transform.position);
            if (Health <= 0) { Death(); RuntimeManager.PlayOneShot(DeathAudio, transform.position); return; }
            else { Behaviour = "Staggered"; }
        }
    }
    void AddDamage(int DMG)
    {
        Anim.Play("UndeadAnim_A_HitRegister_Hit");
        if (DamagerNumber.gameObject.activeInHierarchy) { DamageTakenInTime += DMG; DamagerNumber.text = DamageTakenInTime.ToString(); }
        else
        {
            DamageTakenInTime += DMG;
            DamagerNumber.text = DamageTakenInTime.ToString();
            StartCoroutine(ShowDamageNumbers());
        }
    }

    IEnumerator ShowDamageNumbers()
    {
        DamagerNumber.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        DamagerNumber.gameObject.SetActive(false);
        DamageTakenInTime = 0;
    }
    public void ToggleParry()
    {
        EnemySaveManager.CanBeParry = !EnemySaveManager.CanBeParry;
    }
    public void TriggerStagger()
    {
        ParryIndicator.SetActive(false);
        Behaviour = "Parried";
        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
        StartCoroutine(Staggered());
        RB.velocity = Vector2.zero;
    }
    IEnumerator Staggered()
    {  
        Anim.Play("UndeadAnim_A_GettingHit");
        IsAttacking = false;
        ParryIndicator.SetActive(false);
        EnemySaveManager.CanBeParry = false;
        yield return new WaitForSeconds(StaggerTime);
        IsAttacking = false;
        Behaviour = "Hostile";
    }

    void UpdateUI()
    {
        HealthSlider.value = Health;
    }

    void LookForPlayer()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < 4)
        {
            SeePlayer = true;
            return;
        }

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
                //Debug.Log("Undead A hit" + hit.transform.name);
                if (hit.transform.CompareTag("Player"))
                {
                    SeePlayer = true;
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
        else
        {
            if (hitA.collider != null)
            {
                if (hitA.transform.CompareTag("Ground") || hitA.transform.CompareTag("Slope"))
                {
                    GetComponentInChildren<EnemyAnimationEvents>().GroundType = hitA.collider.GetComponent<ObjectType_AudioRef>().ObjectType;
                }
            }
            else if (hitB.collider != null)
            {
                if (hitB.transform.CompareTag("Ground") || hitB.transform.CompareTag("Slope"))
                {
                    GetComponentInChildren<EnemyAnimationEvents>().GroundType = hitB.collider.GetComponent<ObjectType_AudioRef>().ObjectType;
                }
            }

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
            Anim.Play("UndeadAnim_A_Walk");
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
        Anim.Play("UndeadAnim_A_Walk");
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
        if (HitPos.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<PlayerControllerV2>().PlayerTakeDamage(AttackDamage, false, 0);
        }
    }


}
