using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerV2 : MonoBehaviour
{

    [Header("Player Stats")]
    public float Health;
    public float MaxHealth;
    public float Stamina;
    public float MaxStamina;
    public int CurrentEstus;
    public int MaxEstus;

    [Header("stuff for jacob")]
    public float MovementInputDirection;
    public float PlayerDirection;
    public float DodgeDirection;
    public bool IsUiOpen = false;

    [Header("movement states")]
    public bool IsGrounded;
    public bool IsStaminaRegen;
    public bool CanMove;
    public bool IsRolling;
    public bool IsBackStepping;
    public bool IsRunning;
    public bool IsJumping;
    public bool IsMovingInput;
    private bool IsWaitingToRun;
    public bool IsIdleAnim;
    public float VerticalSpeed;
    public bool FootAOnSlope;
    public bool FootBOnSlope;

    [Header("Movement Data to edit")]
    public float WalkSpeed;
    public float RunSpeed;
    public float RollSpeed;
    public float RollTime;
    public float BackstepSpeed;
    public float BackstepTime;
    public float JumpVerticalSpeed;
    public float JumpHorizontalSpeed;
    public float JumpRunHorizontalSpeed;
    public float JumpTime;
    public float FallSpeed;
    public float AirborneControl;


    [Header("Combat states")]
    public bool IsLockedOn;
    public bool IsBlocking;
    public bool IsHealing;
    public bool IsAttackStepping;
    public bool CanAttack;
    public bool CanFollowUp;
    public bool CanRollOut;
    private bool SwapLightAnim;

    [Header("Combat Data to edit")]
    public float EstusHealAmount;
    public float LightAttackTime;
    public float LightFollowUpAttackTime;
    public float HeavyAttackTime;
    public float HeavyFollowUpAttackTime;
    public float LightAttackStep;
    public float HeavyAttackStep;

    [Header("DAMAGE Data to edit")]
    public float LightAttackDamage;
    public float HeavyAttackDamage;
    public float LightAttackRange;
    public float LightAttackFollowUpRange;
    public float HeavyAttackRange;
    public float HeavyAttackFollowUpRange;


    [Header("variables")]
    public GameObject LayerManager;
    public GameObject Assets;
    public Animator Anim;
    public Transform GroundCheckPosA;
    public Transform GroundCheckPosB;

    private Coroutine WaitToRunCoroutine;
    private Coroutine JumpingCoroutine;
    private Coroutine LightAttackCoroutine;
    private Coroutine HeavyAttackCoroutine;

    public Rigidbody2D MyRb;
    private Collider2D MyCol;
    private PlayerManager PM;
    private EnemyLock EnemyLock;

    public InteractableV2 Interactable;

    //UI
    private CanvasManager CM;
    private Slider StaminaSlider;
    private Slider HealthSlider;
    private TextMeshProUGUI EstusCountText;

    private void Start()
    {
        PM = GetComponent<PlayerManager>();
        MyRb = GetComponent<Rigidbody2D>();
        MyCol = GetComponent<CapsuleCollider2D>();
        EnemyLock = GetComponent<EnemyLock>();
        VerticalSpeed = FallSpeed;
        if (SceneManager.GetActiveScene().name == "Build")
        {
            CM = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            StaminaSlider = CM.PlayerStaminaSlider;
            HealthSlider = CM.PlayerHealthSlider;
            EstusCountText = CM.EstusCountText;
        }
        StartCoroutine(StaminaRegenPause());        
    }

    IEnumerator PlayerDead()
    {
        CanMove = false;
        CanAttack = false;
        IsMovingInput = false;
        MyRb.velocity = Vector2.zero;
        CM.YouDiedAnim.Play("YouDied");
        yield return new WaitForSeconds(4.5f);
       
        switch (PM.LastBonfireVisited)
        {
            case 1:
                transform.position = PM.Bonfire_1.transform.position;
                PM.Bonfire_1.BonfireRest();
                break;
            case 2:
                transform.position = PM.Bonfire_1.transform.position;
                PM.Bonfire_2.BonfireRest();
                break;
            case 3:
                transform.position = PM.Bonfire_1.transform.position;
                PM.Bonfire_3.BonfireRest();
                break;
            default:
                transform.position = new Vector2(-90,-18);
                PM.Bonfire_1.BonfireRest();
                break;
        }
        yield return new WaitForSeconds(1f);
        CanMove = true;
        CanAttack = true;
        IsMovingInput = true;
    }

    public void PlayerTakeDamage(float Damage, bool Knockdown, int KnockDownDirection) // -1 to left, 1 to right, 0 is the direction the player is on relative to the enemy
    {
        if (!IsRolling)
        {
            if (!Knockdown)
            {
                if (!IsBlocking)
                {
                    Stamina -= Health;
                }
                else
                {
                    Health -= Damage;
                }
            }
            else
            {
                Health -= Damage;
                //knock down player
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {

            if (context.performed)
            {
                Vector2 MovementInput;
                MovementInput = context.ReadValue<Vector2>();
            if (MovementInput.x > 0.2f)
            {
                MovementInputDirection = 1;
                IsMovingInput = true;
            }
            if (MovementInput.x < -0.2f)
            {
                MovementInputDirection = -1;
                IsMovingInput = true;
            }
        }
        
        if (context.canceled)
        {
            MovementInputDirection = 0;
            IsMovingInput = false;
            MyRb.velocity = Vector2.zero;
        }
    }
    public void B(InputAction.CallbackContext context)
    {
        if (IsBlocking) { CanMove = true; }
        if (CanMove && !IsUiOpen && !IsHealing)
        {

            if (context.action.WasPerformedThisFrame())
            {

                WaitToRunCoroutine = StartCoroutine(WaitToRun()); 
                
            }
            if (context.action.WasReleasedThisFrame())
            {
                if (IsWaitingToRun) { StopCoroutine(WaitToRunCoroutine); IsWaitingToRun = false; }
                if (IsGrounded)
                {
                    if (!IsRunning) // trigger roll
                    {
                        
                        IsRunning = false;
                        CanMove = false;
                        if (MovementInputDirection == 1) { DodgeDirection = 1; }
                        else if (MovementInputDirection == -1) { DodgeDirection = -1; }
                        if (MovementInputDirection == 0) { StartCoroutine(Backstep()); Debug.Log("Backstep");  Stamina -= 15f; }
                        else { StartCoroutine(Roll()); Debug.Log("Roll"); Stamina -= 30f; }
                    }
                    else
                    {
                        IsRunning = false;
                    }
                }
                else
                {
                    IsRunning = false;
                    IsRolling = false;
                }
            }
        }
        if (CanRollOut)
        {        
            if (context.action.WasReleasedThisFrame())
            {
                if (IsWaitingToRun) { StopCoroutine(WaitToRunCoroutine); IsWaitingToRun = false; }
                if (IsGrounded)
                {
                    if (!IsRunning) // trigger roll
                    {

                        IsRunning = false;
                        CanMove = false;
                        if (MovementInputDirection == 1) { DodgeDirection = 1; }
                        else if (MovementInputDirection == -1) { DodgeDirection = -1; }
                        if (MovementInputDirection == 0) { StartCoroutine(Backstep()); Debug.Log("Backstep"); Stamina -= 15f; }
                        else { StartCoroutine(Roll()); Debug.Log("Roll"); Stamina -= 30f; }
                    }
                    else
                    {
                        IsRunning = false;
                    }
                }
                else
                {
                    IsRunning = false;
                    IsRolling = false;
                }
            }
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (IsBlocking) { CanMove = true; }
        if (CanMove && !IsUiOpen && !IsHealing)
        {

            if (context.action.WasPerformedThisFrame())
            {
                if (IsGrounded)
                {
                    DodgeDirection = MovementInputDirection;
                    JumpingCoroutine = StartCoroutine(Jump());
                    Stamina -= 30f;
                    CanMove = false;
                    VerticalSpeed = 0;
                }
            }
        }

    }
    public void Y(InputAction.CallbackContext context)
    {
        Debug.Log("Y");
        if (CanMove && !IsRolling && !IsJumping && !IsRunning && Interactable != null && !IsUiOpen && !IsHealing)
        {
            if (context.action.triggered)
            {
                Interactable.InvokeEvent();
            }
        }

    }

    public void X(InputAction.CallbackContext context)
    {
        Debug.Log("X");
        if (CanMove && !IsRolling && !IsJumping && !IsRunning && !IsUiOpen && !IsHealing)
        {
            if (context.action.triggered && CurrentEstus>0)
            {
                StartCoroutine(UseEstus());
            } 
            if(context.action.triggered && CurrentEstus == 0)
            {
                StartCoroutine(UseEmptyEstus());
            }
        }

    }

    public void LockOn(InputAction.CallbackContext context)
    {
        Debug.Log("LockOn");
        if (!IsUiOpen)
        {
            if (context.action.triggered)
            {
                EnemyLock.ToggleLockOn(); ;
            }    
        }

    }


    public void RT(InputAction.CallbackContext context)
    {
        if (IsBlocking) { CanMove = true; }
        if (CanMove && IsGrounded && !IsUiOpen && !CanFollowUp && CanAttack && Stamina > 0)
        {
            if (context.action.triggered)
            {
                HeavyAttackCoroutine = StartCoroutine(HeavyAttack());
                Stamina -= 35f;
            }
        }
        if (IsGrounded && !IsUiOpen && CanFollowUp && Stamina > 0)
        {
            if (context.action.triggered)
            {
                if (HeavyAttackCoroutine != null) { StopCoroutine(HeavyAttackCoroutine); }
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                StartCoroutine(HeavyAttackFollowUp());
                Stamina -= 30f;
            }
        }

    }
    public void RB(InputAction.CallbackContext context)
    {
        

        if (CanMove && IsGrounded && !IsUiOpen && !CanFollowUp && CanAttack && Stamina > 0)
        {
            if (context.action.triggered)
            {
                LightAttackCoroutine = StartCoroutine(LightAttack());
                Stamina -= 25f;
            }
        }
        if (IsGrounded && !IsUiOpen && CanFollowUp && Stamina > 0)
        {
            if (context.action.triggered)
            {
                if (LightAttackCoroutine != null) { StopCoroutine(LightAttackCoroutine); }
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                StartCoroutine(LightAttackFollowUp());
                Stamina -= 20f;
            }
        }


    }

    public void LT(InputAction.CallbackContext context)
    {

      

    }
    public void LB(InputAction.CallbackContext context)
    {
        if (IsBlocking)
        {
            if (context.action.triggered)
            {
                ToggleBlock();
            }
        }
        if (CanMove && IsGrounded && !IsUiOpen && CanAttack && Stamina > 0 && !IsBlocking)
        {
            if (context.action.triggered)
            {
                ToggleBlock();
            }
        }
    }

    public void Update()
    {
        if (Health <= 0) { StartCoroutine(PlayerDead()); }
        GroundCheck();
        UpdateUI();
        if (IsStaminaRegen) { StaminaRegen(); }
        if (Stamina <= 0 && IsGrounded) 
        { 
            StartCoroutine(StaminaRegenPause()); 
            IsRunning = false;
            if (IsBlocking) { StartCoroutine(Stagger()); } //stagger player
        }

        if (IsLockedOn && !IsRolling && !IsRunning && !IsJumping)
        {
            if (EnemyLock.enemyToRight)
            {
                PlayerDirection = -1;
            }
            else
            {
                PlayerDirection = 1;
            }
        }
            
        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }            
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }

        if (IsGrounded && CanMove && IsIdleAnim && !IsMovingInput)
        {
            Anim.Play("PlayerAnim_Idle");  
        }

        if (IsGrounded && CanMove && IsMovingInput)
        {
            
            if (MovementInputDirection > 0.2f && !IsLockedOn) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f && !IsLockedOn) { PlayerDirection = -1; }
            if (!IsRunning)
            {
                MyRb.velocity = new Vector2(MovementInputDirection * WalkSpeed, -VerticalSpeed);
                IsStaminaRegen = true;
                if (!IsLockedOn) { Anim.Play("PlayerAnim_WalkForward"); }
                else { Anim.Play("PlayerAnim_WalkBackward"); }
            }
            else if (IsRunning && Stamina > 0)
            {
                Stamina -= Time.deltaTime * 8.5f; IsStaminaRegen = false;
                MyRb.velocity = new Vector2(MovementInputDirection * RunSpeed, -VerticalSpeed);
                Anim.Play("PlayerAnim_Run");
            }
        }
       
        if(!IsGrounded && IsJumping && !IsRolling && !IsBackStepping)
        {
            if (!IsRunning) { MyRb.velocity = new Vector2(MovementInputDirection * AirborneControl, MyRb.velocity.y); }
            else { MyRb.velocity = new Vector2(MovementInputDirection * AirborneControl *4, MyRb.velocity.y); }
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
        }
        if(IsGrounded && IsRolling) { MyRb.velocity = new Vector2(DodgeDirection * RollSpeed, MyRb.velocity.y / 10); }
        if(IsGrounded && IsBackStepping) { MyRb.velocity = new Vector2(-PlayerDirection * BackstepSpeed, MyRb.velocity.y / 10); }


    }    
    public void GroundCheck()
    {
        RaycastHit2D hitA = Physics2D.Raycast(GroundCheckPosA.position, Vector2.down, .45f);
        RaycastHit2D hitB = Physics2D.Raycast(GroundCheckPosB.position, Vector2.down, .45f);

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
        if(FootAOnSlope && FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
        else if (FootAOnSlope && !FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
        else if (!FootAOnSlope && FootBOnSlope && hitB.transform != null) { OnSlope( hitB.transform); }
        else if(!FootAOnSlope && !FootBOnSlope)
        {
            //not on slope reset speeds
            WalkSpeed = 2f;
            RunSpeed = 5f;

            if (IsGrounded) { VerticalSpeed = 0.5f; } else { VerticalSpeed = FallSpeed; }
        }



    }
    void OnSlope(Transform Slope)
    {

        float SlopeAngle = Slope.transform.transform.eulerAngles.z;
        if (SlopeAngle <= 35 && SlopeAngle < 60)
        {
            // right  a 35 or less slope
            if (MovementInputDirection == 1)
            { //going against slope
                Debug.Log("Going up 30 right");
                WalkSpeed = 3f;
                RunSpeed = 5.5f;
                if (IsMovingInput) { VerticalSpeed = 0f; } else { MyRb.velocity = Vector2.zero; }
            }
            else
            {//going with slope
                Debug.Log("Going down 30 right");
                WalkSpeed = 1.5f;
                RunSpeed = 4f;
                if (IsMovingInput) { VerticalSpeed = 5f; } else { MyRb.velocity = Vector2.zero; }
                if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; } 
                if(!FootAOnSlope && FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
            }

        }
        else if(SlopeAngle > 35 && SlopeAngle <60)
        {
            // right  a 35 or MORE slope
            if (MovementInputDirection == 1)
            { //going against slope
                WalkSpeed = 3.5f;
                RunSpeed = 7f; 
               if (IsMovingInput) { VerticalSpeed = 0f;} else { MyRb.velocity = Vector2.zero; }
            }
            else
            {//going with slope
                WalkSpeed = 1.5f;
                RunSpeed = 3f;
                if (IsMovingInput) { VerticalSpeed = 5f; } else { MyRb.velocity = Vector2.zero; }
                if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
                if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
            }
        }
        if (SlopeAngle >= 325 && SlopeAngle >300)
        {
            // left a 35 or less slope
            if (MovementInputDirection == 1)
            { //going against slope
                WalkSpeed = 1.5f;
                RunSpeed = 4f;
                if (IsMovingInput) { VerticalSpeed = 5f; } else { MyRb.velocity = Vector2.zero; }
                if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
                if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; } 
            }
            else
            {//going with slope
                WalkSpeed = 3f;
                RunSpeed = 5.5f;
                if (IsMovingInput) { VerticalSpeed = 0f;  } else { MyRb.velocity = Vector2.zero; }
            }
        }
        else if (SlopeAngle < 325 && SlopeAngle >300)
        {
            // left a 35 or MORE slope
            if (MovementInputDirection == 1)
            { //going with slope
                WalkSpeed = 1.5f;
                RunSpeed = 3f;
                if (IsMovingInput) { VerticalSpeed = 5f; } else { MyRb.velocity = Vector2.zero; }
                if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
                if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 1.75f; VerticalSpeed = 1.75f; }
            }
            else
            {//going against slope
                WalkSpeed = 3.5f;
                RunSpeed = 7f;
                if (IsMovingInput) { VerticalSpeed = 0f;  } else { MyRb.velocity = Vector2.zero; }
            }
        }
    }


    public void UpdateUI()
    {
        if (StaminaSlider != null) { StaminaSlider.value = Stamina; }
        if (HealthSlider != null) { HealthSlider.value = Health; }
        if (EstusCountText != null) { EstusCountText.text = CurrentEstus.ToString(); }
    }

    public void StaminaRegen()
    {
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        if (Stamina < 100) { Stamina += Time.deltaTime * 30; } else { Stamina = 100; IsStaminaRegen = false; }

    }

    IEnumerator WaitToRun()
    {
        IsWaitingToRun = true;
        yield return new WaitForSeconds(.25f);
        Debug.Log("Run");
        if (!IsRolling) { IsRunning = true; }
        IsWaitingToRun = false;
    }
    IEnumerator Roll()
    {
        IsRolling = true;
        IsIdleAnim = false;
        CanMove = false;
        IsStaminaRegen = false;
        Anim.Play("PlayerAnim_Roll");

        if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }

        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }


        yield return new WaitForSeconds(RollTime);
        CanMove = true;
        IsRolling = false;
        DodgeDirection = 0;
        MyRb.velocity = Vector2.zero;   
        if (!IsMovingInput) { MovementInputDirection = 0; }
        StartCoroutine(StaminaRegenPause());
        yield return new WaitForSeconds(.4f);
        IsIdleAnim = true;
    }
    IEnumerator Backstep()
    {
        IsBackStepping = true;
        IsIdleAnim = false;
        Anim.Play("PlayerAnim_BackStep");
        IsStaminaRegen = false;

        yield return new WaitForSeconds(BackstepTime);
        CanMove = true;
        IsBackStepping = false;
        MyRb.velocity = Vector2.zero;
        StartCoroutine(StaminaRegenPause());
        yield return new WaitForSeconds(.25f);
        IsIdleAnim = true;
    }
    IEnumerator Jump()
    {
        IsIdleAnim = false;
        Anim.Play("PlayerAnim_JumpEnter");
        IsJumping = true;
        IsStaminaRegen = false;
        if (!IsRunning) { MyRb.velocity = new Vector2(MovementInputDirection * JumpHorizontalSpeed, JumpVerticalSpeed); }
        else { MyRb.velocity = new Vector2(MovementInputDirection * JumpRunHorizontalSpeed, JumpVerticalSpeed); }

        yield return new WaitForSeconds(.25f);
        Anim.Play("PlayerAnim_JumpIdle");
        if (IsGrounded)
        {
            FinishJump();
            StopCoroutine(JumpingCoroutine);
        }

        yield return new WaitForSeconds(JumpTime);
        FinishJump();
    }
    void FinishJump()
    {
        CanMove = true;
        IsRolling = false;
        IsRunning = false;
        IsJumping = false;
        VerticalSpeed = FallSpeed;
        MyRb.velocity = Vector2.zero;
        DodgeDirection = 0;
        IsIdleAnim = true;
        Anim.Play("PlayerAnim_JumpLand");
        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator StaminaRegenPause()
    {
        IsStaminaRegen = false;
        yield return new WaitForSeconds(.65f);
        IsStaminaRegen = true;
    }

    void AttackStart()
    {
        CanMove = false;
        CanAttack = false;
        IsStaminaRegen = false;
        IsAttackStepping = true; 
        CanFollowUp = false;
        IsBlocking = false;
    }

    IEnumerator LightAttack()
    {
        AttackStart();
        Anim.Play("PlayerAnim_LightSwing");
        AttackStepMove(LightAttackStep);
        yield return new WaitForSeconds(LightAttackTime-1.2f);
        CanFollowUp = true;
        CanRollOut = true;
        IsAttackStepping = false; 
        
        yield return new WaitForSeconds(1.2f);
        CanMove = true;
        CanRollOut = false;
        CanAttack = true;
        CanFollowUp = false;
        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator LightAttackFollowUp()
    {
        AttackStart();
        Anim.Play("PlayerAnim_LightSwingFollowUpAttack");
        AttackStepMove(LightAttackStep);
        yield return new WaitForSeconds(LightFollowUpAttackTime-1);
        CanRollOut = true;
        yield return new WaitForSeconds(1);
        CanRollOut = false;
        IsAttackStepping = false; 
        CanMove = true;
        CanAttack = true;
        StartCoroutine(StaminaRegenPause());
    }

    IEnumerator HeavyAttack()
    {
        AttackStart();
        Anim.Play("PlayerAnim_HeavySwing");
        AttackStepMove(HeavyAttackStep);
        CanFollowUp = false;
        yield return new WaitForSeconds(HeavyAttackTime - 1.4f);
        CanFollowUp = true;
        CanRollOut = true;
        IsAttackStepping = false; 
        
        yield return new WaitForSeconds(1.4f);
        CanMove = true;
        CanRollOut = false;
        CanAttack = true;
        CanFollowUp = false;
        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator HeavyAttackFollowUp()
    {
        AttackStart();
        Anim.Play("PlayerAnim_HeavySwingFollowUpAttack");
        AttackStepMove(HeavyAttackStep);
        yield return new WaitForSeconds(HeavyFollowUpAttackTime - 1);
        CanRollOut = true;
        yield return new WaitForSeconds(1);
        CanRollOut = false;
        IsAttackStepping = false; 
        CanMove = true;
        CanAttack = true;
        StartCoroutine(StaminaRegenPause());
    }

    void AttackStepMove(float StepDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);


        if (hit.collider != null)
        {
            if (!hit.transform.CompareTag("Enemy"))
            {
                MyRb.velocity = new Vector2(PlayerDirection * StepDistance, MyRb.velocity.y / 10);
            }
            else
            {
                MyRb.velocity = Vector2.zero;
            }
            
        }
        else
        {
            MyRb.velocity = new Vector2(PlayerDirection * StepDistance, MyRb.velocity.y / 10);
        }
    }

    IEnumerator Stagger()
    {
        Anim.Play("PlayerAnim_StaggerGettingHit");
        CanFollowUp = false;
        CanMove = false;
        CanAttack = false;
        IsBlocking = false;

        yield return new WaitForSeconds(1f);
        CanFollowUp = true;
        CanMove = true;
        CanAttack = true;
    }

    void ToggleBlock()
    {
        if (!IsBlocking)
        {
            StartCoroutine(EnterBlock());
        }
        else
        {
            StartCoroutine(ExitBlock());
        }
    }
    void HoldBlock()
    {
        IsBlocking = true;
        Anim.Play("PlayerAnim_ShieldBlockHold");
    }
    IEnumerator EnterBlock()
    {
        Anim.Play("PlayerAnim_ShieldBlockEnter");
        CanFollowUp = false;
        CanMove = false;
        yield return new WaitForSeconds(0.5f);
        HoldBlock();
    }
    IEnumerator ExitBlock()
    {
        Anim.Play("PlayerAnim_ShieldBlockExit");
        yield return new WaitForSeconds(0.25f);
        CanFollowUp = true;
        CanMove = true;
        IsBlocking = false;
    }

    IEnumerator UseEstus()
    {
        MyRb.velocity = Vector2.zero;
        Anim.Play("PlayerAnim_EstusUse");
        CanMove = false;
        CanAttack = false;
        CanFollowUp = false;
        yield return new WaitForSeconds(1);

        Health += EstusHealAmount;
        Health = Mathf.Clamp(Health, 0, 100);
        CurrentEstus--;
        yield return new WaitForSeconds(.5f);
        CanMove = true;
        CanAttack = true;
        CanFollowUp = true;
    }
    IEnumerator UseEmptyEstus()
    {
        MyRb.velocity = Vector2.zero;
        Anim.Play("PlayerAnim_EstusUseEmpty");
        CanMove = false;
        CanAttack = false;
        CanFollowUp = false;
        yield return new WaitForSeconds(1.5f);
        CanMove = true;
        CanAttack = true;
        CanFollowUp = true;
    }

}
