using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using TMPro;
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


    [Header("Running Slash")]
    public float TimeBeforeAttack_RS;
    public float AttackAnimationTime_RS;
    public float AttackCoolDownTime_RS;

    [Header("Quick Barrage")]
    public float TimeBeforeAttack_QB;
    public float AttackAnimationTime_QB;
    public float AttackCoolDownTime_QB;

    [Header("2H Slash")]
    public float TimeBeforeAttack_2S;
    public float AttackAnimationTime_2S;
    public float AttackCoolDownTime_2S;

    [Header("Stab")]
    public float TimeBeforeAttack_SA;
    public float AttackAnimationTime_SA;
    public float AttackCoolDownTime_SA;

    [Header("Combat Data")]
    public float StaggerTime;
    public float TriggerRange;
    public float CloseAttackRange;
    public float AttackRange;
    public float AttackStepMultiplier;
    public float CombatTime;

    public List<Collider2D> Overlap;

    [Header("Bools")]
    public bool SeePlayer;
    public bool IsAttacking;
    public bool IsHeavyAttacking;
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
    public Rigidbody2D RB;
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

    private float RandomGruntTime;
    private float RandomGruntTarget;



    public EnemySaveManager EnemySaveManager;



    private void Start()
    {
        RandomGruntTarget = Random.Range(3, 10);
        RandomGruntTime = 0;
        ManualStart();
    }

    public void ManualStart()
    {
        Anim = GetComponentInChildren<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }

        IsAttacking = false;

        HealthSlider.maxValue = MaxHealth;

        IsDying = false;

        if (Health > 0) { IsDead = false;  EnemySaveManager.IsLockOnAble = true; } else { Dead(); Behaviour = "Dead"; }
    }

    public void Respawn()
    {

        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }

        RB.constraints = RigidbodyConstraints2D.None;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        EnemySaveManager.IsLockOnAble = true;
        DamagerNumber.gameObject.SetActive(false);

        IsDead = false;
        Assets.SetActive(true);
        gameObject.SetActive(true);
        Health = MaxHealth;
        HealthSlider.value = Health;
        transform.localPosition = OriginPosition;

        Behaviour = "Idle";
        SeePlayer = false;
        ParryIndicator.SetActive(false);
        IsAttackStepping = false;
        IsAttacking = false;

    }

    void Update()
    {
        UpdateUI();
        IsByOriginPosition();
        GroundCheck();

        if (Health > 0)
        {
            RB.constraints = RigidbodyConstraints2D.None;
            RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            IsDead = false; IsDying = false;
            switch (Behaviour)
            {
                case "Idle":
                    ParryIndicator.SetActive(false);
                    LookForPlayer();
                    Anim.Play("UndeadAnim_C_Idle");
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
                    ParryIndicator.SetActive(false);
                    CombatTime += Time.deltaTime;
                    LookForPlayer();                   
                    if (IsInRange()) { Behaviour = "Attacking"; }
                    else
                    {
                        FacePlayer();
                        Walk();
                    }

                    if (!SeePlayer && !IsAtOrigin && CombatTime > 8) { Behaviour = "Returning"; CombatTime = 0; }
                    break;
                case "Attacking":
                    LookForPlayer();
                    if (!IsAttacking) { ChooseAttack(); }
                    else
                    {
                        if (!IsAttackStepping)
                        {
                            FacePlayer();
                            RB.velocity = new Vector2(0, -VerticalSpeed);
                            Debug.Log("testestest");
                        }
                        else
                        {
                            Stepping();
                        }
                    }
                    break;
                case "Staggered":
                    ParryIndicator.SetActive(false);
                    if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
                    //StartCoroutine(Staggered());
                    IsAttacking = false;
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
                case "Dead":
                    ParryIndicator.SetActive(false);
                    Dead();
                    RB.velocity = Vector2.zero;
                    ParryIndicator.SetActive(false);
                    break;
                case "Dying":
                    RB.velocity = Vector2.zero;
                    break;
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
        else
        {

            RB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void ChooseAttack()
    {

            if (IsInCloseRange())
            {
                int num = Random.Range(1, 4);
                Debug.Log(num);
                if (num == 1) { AttackingCoroutine = StartCoroutine(Attack_QuickBarrage()); }
                else if (num == 2) { AttackingCoroutine = StartCoroutine(Attack_2HSlash()); }
                else if (num == 3) { AttackingCoroutine = StartCoroutine(Attack_Stab()); }
                
            }
            else
            {
                AttackingCoroutine = StartCoroutine(Attack_RunningSlash());
            }
        IsAttacking = true;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
    
    
    
    void Death()
    {
       Behaviour = "Dying";

            IsDying = true;
            DamagerNumber.gameObject.SetActive(false);
            SeePlayer = false;
            RB.constraints = RigidbodyConstraints2D.FreezeAll;
            EnemySaveManager.IsLockOnAble = false;
            RB.velocity = Vector2.zero;
            IsAttacking = false;
            IsHeavyAttacking = false;
            EnemySaveManager.CanBeParry = false;
            ParryIndicator.SetActive(false);
            IsAttackStepping = false;

            
            StopAllCoroutines();
            HealthSlider.value = 0;
            Anim.Play("UndeadAnim_C_Death");
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

        RB.constraints = RigidbodyConstraints2D.None;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;

        DamagerNumber.gameObject.SetActive(false);
        DamageTakenInTime = 0;
        DamagerNumber.text = DamageTakenInTime.ToString();
        if (Behaviour == "Attacking")
        {
            CombatTime = 0;
            IsAttacking = false;
            IsHeavyAttacking = false;
            Behaviour = "Hostile";
        }
    }
    private void OnEnable()
    {
        RB.constraints = RigidbodyConstraints2D.None;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (Behaviour == "Attacking")
        {
            Behaviour = "Hostile";
        }
        IsAttacking = false;
        SeePlayer = false;
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
            if (Health <= 0) { Death(); RuntimeManager.PlayOneShot(DeathAudio, transform.position); return; }
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
            if (!IsHeavyAttacking) { Behaviour = "Staggered"; StartCoroutine(Staggered()); }
        }
       
    }
    public void TriggerStagger()
    {
        ParryIndicator.SetActive(false);
        Behaviour = "Parried";
        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
        StartCoroutine(Staggered());
        RB.velocity = Vector2.zero;
    }

    void AddDamage(int DMG)
    {
        Anim.Play("UndeadAnim_C_HItRegister_Hit");
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


    IEnumerator Staggered()
    {
        Anim.Play("UndeadAnim_C_GettingHit");
        IsAttacking = false;
        IsAttacking = false;
        ParryIndicator.SetActive(false);
        EnemySaveManager.CanBeParry = false;
        yield return new WaitForSeconds(StaggerTime);
        IsAttacking = false;
        IsHeavyAttacking = false;
        Behaviour = "Hostile";
    }
    void UpdateUI()
    {
        HealthSlider.value = Health;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    bool IsInRange()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < TriggerRange)
        {
            Anim.Play("UndeadAnim_C_Idle");
            return true;
        }
        else
        {
            return false;
        }
    }
    bool IsInCloseRange()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < CloseAttackRange)
        {
            Anim.Play("UndeadAnim_C_Idle");
            return true;
        }
        else
        {
            return false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator Attack_RunningSlash()
    {
        IsAttacking = true;
        IsHeavyAttacking = true;
        AttackStepMultiplier = 7.5f;
        yield return new WaitForSeconds(TimeBeforeAttack_RS);
        FacePlayer();
        Anim.Play("UndeadAnim_C_RunningSlash");
        yield return new WaitForSeconds(AttackAnimationTime_RS);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime_RS);
        IsAttacking = false;
        IsHeavyAttacking = false;
        Behaviour = "Hostile";
        FacePlayer();
    }
    IEnumerator Attack_QuickBarrage()
    {
        IsAttacking = true;
        IsHeavyAttacking = true;
        AttackStepMultiplier = -1;
        yield return new WaitForSeconds(TimeBeforeAttack_QB);
        FacePlayer();
        Anim.Play("UndeadAnim_C_QuickBarrage");
        yield return new WaitForSeconds(AttackAnimationTime_QB);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime_QB);
        IsAttacking = false;
        IsHeavyAttacking = false;
        Behaviour = "Hostile";
        FacePlayer();
    }
    IEnumerator Attack_2HSlash()
    {
        IsAttacking = true;
        AttackStepMultiplier = -1;
        yield return new WaitForSeconds(TimeBeforeAttack_2S);
        FacePlayer();
        Anim.Play("UndeadAnim_C_2hSwordSlash");
        yield return new WaitForSeconds(AttackAnimationTime_2S);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime_2S);
        IsAttacking = false;
        IsHeavyAttacking = false;
        Behaviour = "Hostile";
        FacePlayer();
    }
    IEnumerator Attack_Stab()
    {
        IsAttacking = true;
        AttackStepMultiplier = 1;
        yield return new WaitForSeconds(TimeBeforeAttack_SA);
        FacePlayer();
        Anim.Play("UndeadAnim_C_StabAttack");
        yield return new WaitForSeconds(AttackAnimationTime_SA);
        Anim.Play("UndeadAnim_C_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime_SA);
        IsAttacking = false;
        IsHeavyAttacking = false;
        Behaviour = "Hostile";
        FacePlayer();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StartAttackStep()
    {
        IsAttackStepping = true;
    }

    public void Stepping()
    {
        if (!HitPos.bounds.Contains(Player.transform.position))
        {
            if (IsGrounded)
            {
                RB.velocity = new Vector2(-(Speed + AttackStepMultiplier) * LookDirection, -VerticalSpeed);
            }
            else
            {
                RB.velocity = new Vector2(-(Speed + AttackStepMultiplier) * LookDirection, -FallSpeed);
            }
        }
        else
        {
            RB.velocity = Vector2.zero;
        }
    }

    public void StopAttackStep()
    {
        RB.velocity = Vector2.zero;
        IsAttackStepping = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AttackRegister_RunningSlash()
    {
        if (HitPos.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<PlayerControllerV2>().PlayerTakeDamage(18, true, 0); //upped from 8
        }

    }
    public void AttackRegister_QuickBarrage()
    {
        if (HitPos.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<PlayerControllerV2>().PlayerTakeDamage(16, true, 0); //upped from 6
        }
    }
    public void AttackRegister_2hSlash()
    {
        if (HitPos.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<PlayerControllerV2>().PlayerTakeDamage(25, false, 0); //upped from 15
        }
    }
    public void AttackRegister_StabAttack()
    {
        if (HitPos.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<PlayerControllerV2>().PlayerTakeDamage(19, false, 0); //upped from 9
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /*
    bool IsOnEnemy()
    {
        
      
    }*/
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ToggleParry()
    {
        EnemySaveManager.CanBeParry = !EnemySaveManager.CanBeParry;
    }

}
