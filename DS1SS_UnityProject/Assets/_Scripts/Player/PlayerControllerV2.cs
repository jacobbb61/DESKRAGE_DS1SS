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
    public string State;
    public float Health;
    public float MaxHealth;
    public float Stamina;
    public float MaxStamina;
    public int CurrentEstus;
    public int MaxEstus;

    [Header("stuff for jacob")]
    public float MovementInputDirection;
    public float PlayerDirection;
    private float JumpDirection;
    public bool IsUiOpen = false;

    [Header("movement states")]
    public bool IsGrounded;
    private bool IsStaminaRegen;
    public bool CanMove;
    public bool IsRolling;
    private bool IsBackStepping;
    public bool IsRunning;
    public bool IsJumping;
    private bool ShouldCheckGrounded;
    private bool IsLanding;
    public bool IsMovingInput;
    private bool IsWaitingToRun;
    private bool DontDondgeOnThisRelease;
    private bool IsIdleAnim;
    public float VerticalSpeed;
    public bool FootAOnSlope;
    public bool FootBOnSlope;

    [Header("Movement Data to edit")]
    public float WalkSpeed;
    public float RunSpeed;

    public float RollSpeed;
    public float RollUpSlopeSpeed;
    public float RollDownSlopeSpeed;
    private float OriginalRollSpeed;
    public float RollTime;

    public float BackstepSpeed;
    public float BackstepUpSlopeSpeed;
    public float BackstepDownSlopeSpeed;
    private float OriginalBackstepSpeed;
    public float BackstepTime;

    public float JumpVerticalSpeed;
    public float JumpHorizontalSpeed;
    public float JumpRunHorizontalSpeed;
    public float JumpTime;
    public float FallSpeed;
    public float AirborneControl;
    public float TimeFalling;


    [Header("Combat states")]
    public bool IsImmune;
    public bool IsLockedOn;
    public bool IsBlocking;
    private bool IsHealing;
    private bool IsPlunging;
    public bool CanPlunge;
    public bool CanUseSecondEstus;
    public bool CanAttack;
    public bool CanFollowUp;
    public bool CanFollowUpAgain;

    public bool CanRollOut; //important


    [Header("Combat Data to edit")]
    public float EstusHealAmount;
    public float LightAttackTime;
    public float LightFollowUpAttackTime;
    public float HeavyAttackTime;
    public float HeavyFollowUpAttackTime;
    public float StepDistance;

    [Header("DAMAGE Data to edit")]
    public float LightAttackDamage;
    public float HeavyAttackDamage;
    public float LightAttackRange;
    public float LightAttackFollowUpRange;
    public float HeavyAttackRange;
    public float HeavyAttackFollowUpRange;

    [Header("Audio stuff")]
    public string WallHitType;
    public string GroundType;

    [Header("variables")]
    public GameObject LayerManager;
    public GameObject Assets;
    public Animator Anim;
    public Transform GroundCheckPosA;
    public Transform GroundCheckPosB;

    private Coroutine WaitToRunCoroutine;

    private Coroutine JumpingCoroutine;
    private Coroutine RollingCoroutine;
    private Coroutine BacksteppingCoroutine;
    private Coroutine StaggerCoroutine;
    private Coroutine EstusUseCoroutine;

    private Coroutine LightAttackCoroutine;
    private Coroutine LightAttackFollowUpCoroutine;
    private Coroutine HeavyAttackCoroutine;
    private Coroutine HeavyAttackFollowUpCoroutine;

    private Coroutine CancelThisCoroutine;

    public Rigidbody2D MyRb;
    private Collider2D MyCol;
    private PlayerManager PM;
    private EnemyLock EnemyLock;

    public InteractableV2 Interactable;

    public OscarManager Oscar;

    //UI
    private CanvasManager CM;
    private Slider StaminaSlider;
    private Slider HealthSlider;
    private TextMeshProUGUI EstusCountText;
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        PM = GetComponent<PlayerManager>();
        MyRb = GetComponent<Rigidbody2D>();
        MyCol = GetComponent<CapsuleCollider2D>();
        EnemyLock = GetComponent<EnemyLock>();
        VerticalSpeed = FallSpeed;
        OriginalBackstepSpeed = BackstepSpeed;
        OriginalRollSpeed = RollSpeed;
        ShouldCheckGrounded = true;
        if (SceneManager.GetActiveScene().name == "Build")
        {
            CM = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            StaminaSlider = CM.PlayerStaminaSlider;
            HealthSlider = CM.PlayerHealthSlider;
            EstusCountText = CM.EstusCountText;
            UpdateUI();
        }
        StartCoroutine(StaminaRegenPause());
        
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator PlayerDead()
    {
        bool ahh = false;

        if (PM.HasDied == false) { PM.HasDied = true; }

        State = "Dead";
        CanMove = false;
        CanAttack = false;
        IsMovingInput = false;
        MyRb.velocity = Vector2.zero;
        CM.YouDiedAnim.Play("YouDied");
        Anim.Play("PlayerAnim_Death");
        yield return new WaitForSeconds(4.5f);

        if(PM.DemonArena.currentState == "Active") //player died to demon
        {
            PM.DemonArena.SwitchState("Idle");
            Oscar.DiedToDemon();
        }

        if(PM.PursuerArena.currentState == "Active") //player died to pursuer
        {
            PM.PursuerArena.SwitchState("Idle");
        }



        if (ahh == false)
        {
            switch (PM.LastBonfireVisited)
            {
                case 1:
                    //transform.position = PM.Bonfire_1.transform.position;
                    PM.Bonfire_1.BonfireRest();
                    break;
                case 2:
                    //transform.position = PM.Bonfire_1.transform.position;
                    PM.Bonfire_2.BonfireRest();
                    break;
                case 3:
                    //transform.position = PM.Bonfire_1.transform.position;
                    PM.Bonfire_3.BonfireRest();
                    break;
                default:
                    //transform.position = new Vector2(-90,-18);
                    PM.Bonfire_1.BonfireRest();
                    break;
            }
            ahh = true;
        }
      
        yield return new WaitForSeconds(1f); 

        CanMove = true;
        CanAttack = true;
        IsMovingInput = true;
        Anim.Play("PlayerAnim_Idle");
    }

    public void PlayerTakeDamage(float Damage, bool Knockdown, int KnockDownDirection) // -1 to left, 1 to right, 0 is the direction the player is on relative to the enemy
    {
        if (!IsRolling && !IsImmune)
        {
            if (!Knockdown)
            {
                if (IsBlocking)
                {
                    Stamina -= Health;
                }
                else
                {
                    Health -= Damage;
                    GetComponentInChildren<AnimationAudio>().PlayerDamageAudio();
                    if (PM.HasBeenHit == false) { PM.HasBeenHit = true; }
                }
            }
            else
            {
                Health -= Damage;
                if (PM.HasBeenHit == false) { PM.HasBeenHit = true; }
                if (StaggerCoroutine != null) { StopCoroutine(StaggerCoroutine); }
                StaggerCoroutine = StartCoroutine(Stagger());
            }
        }
        if (Health <= 0) { StartCoroutine(PlayerDead()); }
    }

    public void PlayerFinishInteraction()
    {
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        IsImmune = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    public void A(InputAction.CallbackContext context)
    {
        if (!IsJumping && IsGrounded)
        {
            switch (State)
            {
                case "Idle":
                    ProcessInput_A(context);
                    break;
                case "Walking":
                    ProcessInput_A(context);
                    break;
                case "Running":
                    ProcessInput_A(context);
                    break;
                case "Blocking":
                    ProcessInput_A(context); IsBlocking = false;
                    break;
            }
        }
    }
    void ProcessInput_A(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {

            if (JumpingCoroutine != null) { StopCoroutine(JumpingCoroutine); }


            if (MovementInputDirection > 0.4f) { PlayerDirection = 1; JumpDirection = 1; }
            else if (MovementInputDirection < -0.4f) { PlayerDirection = -1; JumpDirection = -1; }
            else  { JumpDirection = 0; }
            FaceTowardsInput();

            State = "Jumping";
         
            JumpingCoroutine = StartCoroutine(Jump());
            
            Stamina -= 10f;
        }
    }
    //////////////////////////////////////////////////////////////
    public void B(InputAction.CallbackContext context)
    {
        if (CanRollOut && CancelThisCoroutine!=null) 
        {
            ProcessInput_B_Cancel();
            StopCoroutine(CancelThisCoroutine);   
        }
        else
        {
            switch (State)
            {
                case "Idle":
                    ProcessInput_B(context);
                    break;
                case "Walking":
                    ProcessInput_B(context);
                    break;
                case "Running":
                    ProcessInput_B(context);
                    break;
                case "Blocking":
                    ProcessInput_B(context); IsBlocking = false;
                    break;
                default:
                    if (context.action.WasPerformedThisFrame())
                    {
                        DontDondgeOnThisRelease = true;
                    }
                    break;
            }
        }
    }
    void ProcessInput_B(InputAction.CallbackContext context)
    {
   
        if (context.action.WasPerformedThisFrame())
        {
            WaitToRunCoroutine = StartCoroutine(WaitToRun());

        }

        if (context.action.WasReleasedThisFrame())
        {    
            if (IsWaitingToRun) { StopCoroutine(WaitToRunCoroutine); IsWaitingToRun = false; }

            if (State == "Running")
            {                
                StartCoroutine(StaminaRegenPause());
                if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
            }
            else if (State != "Running" && DontDondgeOnThisRelease == false)
            {
                if (MovementInputDirection == 0) // trigger backstep
                {
                    StartBackStep();
                }
                else // trigger roll
                {
                    StartRoll();
                }
                
            }
         DontDondgeOnThisRelease = false;

        }

    }
    void ProcessInput_B_Cancel()
    {
        if (MovementInputDirection == 0) { StartBackStep(); }
        else { StartRoll(); }
        CanFollowUp = false;
        CanRollOut = false;
    }
    void StartRoll()
    {
        if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }

        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }

        RollingCoroutine =  StartCoroutine(Roll());

        Stamina -= 30f;
        State = "Rolling";
    }
    void StartBackStep()
    {
       BacksteppingCoroutine = StartCoroutine(Backstep()); 

        Stamina -= 15f;
        State = "BackStepping";
    }
    //////////////////////////////////////////////////////////////
    public void X(InputAction.CallbackContext context)
    {
        if (CanUseSecondEstus && CurrentEstus>0)
        {
            State = "Estus";
            EstusUseCoroutine = StartCoroutine(UseEstus());
        }
        else
        {
            switch (State)
            {
                case "Idle":
                    ProcessInput_X(context);
                    break;
                case "Walking":
                    ProcessInput_X(context);
                    break;
                case "Blocking":
                    ProcessInput_X(context); IsBlocking = false;
                    break;
                default:
                    break;
            }
        }
    }
    void ProcessInput_X(InputAction.CallbackContext context)
    {
            if (context.action.triggered && CurrentEstus>0)
            {
            State = "Estus";
            EstusUseCoroutine = StartCoroutine(UseEstus());
            } 
            if(context.action.triggered && CurrentEstus == 0)
            {
            State = "Estus";
            EstusUseCoroutine = StartCoroutine(UseEmptyEstus());
            }
    }
    //////////////////////////////////////////////////////////////
    public void Y(InputAction.CallbackContext context)
    {
        switch (State)
        {
            case "Idle":
                ProcessInput_Y(context);
                break;
            case "Walking":
                ProcessInput_Y(context);
                break;
            case "Running":
                ProcessInput_Y(context);
                break;
            case "Blocking":
                ProcessInput_Y(context);
                break;
            default:
                break;
        }
    }
    void ProcessInput_Y(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
           
            if (Interactable != null)
            { 
                State = "Interacting";
                Interactable.InvokeEvent(); 
            }

        }
    }
    //////////////////////////////////////////////////////////////
    public void Move(InputAction.CallbackContext context)
    {

            Vector2 MovementInput;
            MovementInput = context.ReadValue<Vector2>();
            if (MovementInput.x > 0.4f)
            {
                MovementInputDirection = 1;
                IsMovingInput = true;
            }
            if (MovementInput.x < -0.4f)
            {
                MovementInputDirection = -1;
                IsMovingInput = true;
            }
        

        if (context.canceled)
        {
            MovementInputDirection = 0;
            IsMovingInput = false;
        }

        switch (State)
        {
            case "Idle":
                if (IsGrounded && IsMovingInput) State = "Walking";
                break;
        }
    }
    //////////////////////////////////////////////////////////////
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
    //////////////////////////////////////////////////////////////
    public void RT(InputAction.CallbackContext context)
    {
        if (Stamina >= 0)
        {
            if (CanFollowUp)
            {
                ProcessInput_RT_Cancel();
            }
            else if (CanFollowUpAgain)
            {
                ProcessInput_RT_CancelAgain();
            }
            else
            {
                switch (State)
                {
                    case "Idle":
                        ProcessInput_RT(context);
                        break;
                    case "Walking":
                        ProcessInput_RT(context);
                        break;
                    case "Running":
                        ProcessInput_RT(context);
                        break;
                    case "Blocking":
                        ProcessInput_RT(context); IsBlocking = false;
                        break;                    
                    case "Falling":
                        ProcessInput_RT_Plunge(context);
                        break;
                    default:
                        break;
                }
            }
        }     

    }
    void ProcessInput_RT(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (!IsLockedOn)
            {
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                FaceTowardsInput();
            }
            else
            {
                FaceTowardsEnemy();
            }
            HeavyAttackCoroutine = StartCoroutine(HeavyAttack());
            Stamina -= 35f;
        }
    }
    void ProcessInput_RT_Cancel()
    {
        if (HeavyAttackCoroutine != null) { StopCoroutine(HeavyAttackCoroutine); }

        if (!IsLockedOn)
        {
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
            FaceTowardsInput();
        }
        else
        {
            FaceTowardsEnemy();
        }
        HeavyAttackFollowUpCoroutine = StartCoroutine(HeavyAttackFollowUp());
        Stamina -= 30f;
        CanFollowUp = false;
    }
    void ProcessInput_RT_CancelAgain()
    {
        if (HeavyAttackFollowUpCoroutine != null) { StopCoroutine(HeavyAttackFollowUpCoroutine); }

        if (!IsLockedOn)
        {
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
            FaceTowardsInput();
        }
        else
        {
            FaceTowardsEnemy();
        }
        HeavyAttackCoroutine = StartCoroutine(HeavyAttack());
        Stamina -= 30f;
        CanFollowUpAgain = false;
    }
    void ProcessInput_RT_Plunge(InputAction.CallbackContext context) 
    {
        if (context.action.triggered && IsPlunging==false && CanPlunge)
        {
            IsPlunging = true;
            Stamina -= 35f;
            CanPlunge = false;
        }
    }
    //////////////////////////////////////////////////////////////
    public void RB(InputAction.CallbackContext context)
    {
        if (Stamina >= 0)
        {
            if (CanFollowUp)
            {
                ProcessInput_RB_Cancel();
            }
            else if (CanFollowUpAgain)
            {
                ProcessInput_RB_CancelAgain();
            }
            else
            {
                switch (State)
                {
                    case "Idle":
                        ProcessInput_RB(context);
                        break;
                    case "Walking":
                        ProcessInput_RB(context);
                        break;
                    case "Running":
                        ProcessInput_RB(context);
                        break;
                    case "Blocking":
                        ProcessInput_RB(context); IsBlocking = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    void ProcessInput_RB(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (!IsLockedOn)
            {
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                FaceTowardsInput();
            }
            else
            {
                FaceTowardsEnemy();
            }
            LightAttackCoroutine = StartCoroutine(LightAttack());
            Stamina -= 25f;
        }
    }
    void ProcessInput_RB_Cancel()
    {
        if (LightAttackCoroutine != null) { StopCoroutine(LightAttackCoroutine); }
        if (!IsLockedOn)
        {
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
            FaceTowardsInput();
        }
        else
        {
            FaceTowardsEnemy();
        }
        
        LightAttackFollowUpCoroutine = StartCoroutine(LightAttackFollowUp());
        Stamina -= 20f;
        CanFollowUp = false;
    }
    void ProcessInput_RB_CancelAgain()
    {
        if (LightAttackFollowUpCoroutine != null) { StopCoroutine(LightAttackFollowUpCoroutine); }
        if (!IsLockedOn)
        {
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
            FaceTowardsInput();
        }
        else
        {
            FaceTowardsEnemy();
        }

        LightAttackCoroutine = StartCoroutine(LightAttack());
        Stamina -= 20f;
        CanFollowUpAgain = false;
    }
    //////////////////////////////////////////////////////////////
    public void LT(InputAction.CallbackContext context)
    {

        if (Stamina >= 0)
        {
            switch (State)
            {
                case "Idle":
                    ProcessInput_LT(context);
                    break;
                case "Walking":
                    ProcessInput_LT(context);
                    break;
                case "Running":
                    ProcessInput_LT(context);
                    break;
                case "Blocking":
                    ProcessInput_LT(context); IsBlocking = false;
                    break;
                default:
                    break;
            }
        }

    }
    void ProcessInput_LT(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (!IsLockedOn)
            {
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                FaceTowardsInput();
            }
            else
            {
                FaceTowardsEnemy();
            }
            StartCoroutine(Parry());
            Stamina -= 15f;
        }
    }
    //////////////////////////////////////////////////////////////
    public void LB(InputAction.CallbackContext context)
    {
        if (Stamina >= 0)
        {
            switch (State)
            {
                case "Idle":
                    ProcessInput_LB(context);
                    break;
                case "Walking":
                    ProcessInput_LB(context);
                    break;
                case "Running":
                    ProcessInput_LB(context);
                    break;
                case "Blocking":
                    ProcessInput_LB(context);
                    break;
                default:
                    break;
            }
        }
    }  
    void ProcessInput_LB(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            if (!IsLockedOn)
            {
                if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
                if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
                FaceTowardsInput();
            }
            else
            {
                FaceTowardsEnemy();
            }
            StartCoroutine(EnterBlock());
        }
        if (context.action.WasReleasedThisFrame())
        {
            StartCoroutine(ExitBlock());
        }
    }
    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Update()
    {   
        if (Health <= 0) { StartCoroutine(PlayerDead()); }
        GroundCheck();
        UpdateUI();
        if (IsStaminaRegen) { StaminaRegen(); }

        switch (State) 
        {
            case "Idle": 
                Idle();
                break;
            case "Walking": 
                Walking();          
                break;
            case "Running": 
                Running();
                break;
            case "Rolling":
                Rolling();
                break;
            case "BackStepping":
                BackStepping();
                break;
            case "Jumping":
                Jumping();
                break;
            case "Falling":
                Falling();
                break;
            case "Attacking":
                Attacking();
                break;
            case "Blocking":
                Blocking();
                break;
            case "Estus":

                break;
            case "Interacting":
                IsImmune = true;
                break;
            case "Stagger":
                IsImmune = true;
                break;
            case "Dead":
                IsImmune = true;
                break;
            case "Saving":
                IsImmune = true;
                break;
            case "MenuOpen":
                MenuOpen();
                break;
            default:
                Debug.LogError("Player state unknown, default to idle");
                State = "Idle";
                break;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Idle()
    {
        Anim.Play("PlayerAnim_Idle");
        if (IsLockedOn) { FaceTowardsEnemy(); } else { FaceTowardsInput(); }
    }

    void Walking()
    {
        if (MovementInputDirection > 0.2f && !IsLockedOn) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.2f && !IsLockedOn) { PlayerDirection = -1; }
        if (IsLockedOn) { FaceTowardsEnemy(); } else { FaceTowardsInput(); }    

        

        MyRb.velocity = new Vector2(MovementInputDirection * WalkSpeed, -VerticalSpeed);

        if (!IsLockedOn) { Anim.Play("PlayerAnim_WalkForward"); }
        else 
        {
            if (GetComponent<EnemyLock>().enemyToRight)
            {
                if (MovementInputDirection < -0.2f)
                {
                    Anim.Play("PlayerAnim_WalkForward");
                }
                else
                {
                    Anim.Play("PlayerAnim_WalkBackward");
                }
            }
            else
            {
                if (MovementInputDirection > 0.2f)
                {
                    Anim.Play("PlayerAnim_WalkForward");
                }
                else
                {
                    Anim.Play("PlayerAnim_WalkBackward");
                }
            }
            

        
        
        }
        
        
        if (MovementInputDirection == 0) { State = "Idle"; }
    }

    void Running()
    {      
        if (Stamina <= 0) //ran out of stamina, break run
        {
            StartCoroutine(StaminaRegenPause());
            if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        }
        else
        {
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; Anim.Play("PlayerAnim_Run"); Stamina -= Time.deltaTime * 8.5f; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; Anim.Play("PlayerAnim_Run"); Stamina -= Time.deltaTime * 8.5f; }
            if (MovementInputDirection > -0.2f && MovementInputDirection < 0.2f) { Anim.Play("PlayerAnim_Idle"); DontDondgeOnThisRelease = true;  }

            FaceTowardsInput(); 

           
            IsStaminaRegen = false;

            MyRb.velocity = new Vector2(MovementInputDirection * RunSpeed, -VerticalSpeed);
        }
    }

    void Rolling()
    {

        if (IsGrounded)
        {
            MyRb.velocity = new Vector2(PlayerDirection * RollSpeed, -VerticalSpeed);
        }
        else
        {
            MyRb.velocity = new Vector2(PlayerDirection * RollSpeed, -5);
        }

    }
    void BackStepping()
    {
        if (IsGrounded) { MyRb.velocity = new Vector2(-PlayerDirection * BackstepSpeed, -VerticalSpeed); }
        else { MyRb.velocity = new Vector2(-PlayerDirection * BackstepSpeed, -5); }
        
    }

    void Jumping()
    {
        IsJumping = true;
        IsGrounded = false;
        FootAOnSlope = false;
        FootBOnSlope = false;
        IsStaminaRegen = false;
    }
    void Falling()
    {

        if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
        FaceTowardsInput();

        MyRb.velocity = new Vector2(MyRb.velocity.x, MyRb.velocity.y);
        if (!IsRunning) 
        { 
            MyRb.velocity = new Vector2(MovementInputDirection * AirborneControl, MyRb.velocity.y);
        }
        else 
        { 
            MyRb.velocity = new Vector2(MovementInputDirection * (AirborneControl + 1), MyRb.velocity.y); 
        }


        TimeFalling += Time.deltaTime;
        VerticalSpeed = 5 + TimeFalling;


        if (IsGrounded && !IsJumping && !IsLanding)
        {
            StartCoroutine(Land());
            IsLanding = true;
            IsPlunging = false;
            TimeFalling = 0;
        }
        if (!IsGrounded && !IsLanding)
        {
            if (IsPlunging)
            {
                Anim.Play("PlayerAnim_Plunge");
                PlungeAttackRegester();
            }
            else
            {
                Anim.Play("PlayerAnim_JumpIdle");
            }
        }


    }
    void Attacking()
    {

    }

    void Blocking()
    {
        if (Stamina <= 0) //ran out of stamina, break blocking and stagger
        {
           StartCoroutine(Stagger());
        }
    }
    void MenuOpen()
    {
        Anim.Play("PlayerAnim_Idle");
        if (IsUiOpen == false) { if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; } }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void FaceTowardsEnemy()
    {
            if (EnemyLock.enemyToRight)
            {
                PlayerDirection = -1;
            }
            else
            {
                PlayerDirection = 1;
            }
        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
    }
    void FaceTowardsInput()
    {
        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ground check
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void GroundCheck()
    {
        if (!IsJumping && ShouldCheckGrounded)
        {
            
            int layerMask = ~(LayerMask.GetMask("Enemy"));
            RaycastHit2D hitA = Physics2D.Raycast(GroundCheckPosA.position, Vector2.down, .65f, layerMask);
            RaycastHit2D hitB = Physics2D.Raycast(GroundCheckPosB.position, Vector2.down, .65f, layerMask);

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
                if (hitA.collider == null)
                {
                    if (hitA.transform.CompareTag("Wall") || hitA.transform.CompareTag("Slope"))
                    {
                        GroundType = hitA.collider.GetComponent<ObjectType_AudioRef>().ObjectType;
                    }
                }
            }

            if (!IsGrounded && State != "Rolling" && State != "BackStepping")
            {
                State = "Falling";
                Anim.Play("PlayerAnim_JumpIdle");
                VerticalSpeed = 4f;
            }
            else { VerticalSpeed = FallSpeed; }


            if (FootAOnSlope && FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
            else if (FootAOnSlope && !FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
            else if (!FootAOnSlope && FootBOnSlope && hitB.transform != null) { OnSlope(hitB.transform); }
            else if (!FootAOnSlope && !FootBOnSlope)
            {
                //not on slope reset speeds
                WalkSpeed = 2f;
                RunSpeed = 5f;
            }

        }
        
    }
    void OnSlope(Transform Slope)
    {
        if (!IsJumping)
        {
            float SlopeAngle = Slope.transform.transform.eulerAngles.z;
            if (SlopeAngle <= 35 && SlopeAngle < 60)
            {
                // right  a 35 or less slope
                if (MovementInputDirection == 1)
                { //going against slope

                    WalkSpeed = 3f;
                    RunSpeed = 5.5f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 0f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                }
                else
                {//going with slope

                    WalkSpeed = 2f;
                    RunSpeed = 4f;
                    if (IsMovingInput) { VerticalSpeed = 3f; } else { MyRb.velocity = Vector2.zero; }
                    if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                    if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                }

            }
            else if (SlopeAngle > 35 && SlopeAngle < 60)
            {
                // right  a 35 or MORE slope
                if (MovementInputDirection == 1)
                { //going against slope
                    WalkSpeed = 3.5f;
                    RunSpeed = 7f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 0f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                }
                else
                {//going with slope
                    WalkSpeed = 1.5f;
                    RunSpeed = 3f;
                    if (IsMovingInput) { VerticalSpeed = 3f; } else { MyRb.velocity = Vector2.zero; }
                    if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput && ! IsJumping) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                    if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput && !IsJumping) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                }
            }
            if (SlopeAngle >= 325 && SlopeAngle > 300)
            {
                // left a 35 or less slope
                if (MovementInputDirection == 1)
                { //going against slope
                    WalkSpeed = 2f;
                    RunSpeed = 4f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 3f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                    if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                    if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                }
                else
                {//going with slope
                    WalkSpeed = 3f;
                    RunSpeed = 5.5f;
                    if (IsMovingInput) { VerticalSpeed = 0f; } else { MyRb.velocity = Vector2.zero; }
                }
            }
            else if (SlopeAngle < 325 && SlopeAngle > 300)
            {
                // left a 35 or MORE slope
                if (MovementInputDirection == 1)
                { //going with slope
                    WalkSpeed = 1.5f;
                    RunSpeed = 3f;
                    if (IsMovingInput) { VerticalSpeed = 3f; } else { MyRb.velocity = Vector2.zero; }
                    if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                    if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                }
                else
                {//going against slope
                    WalkSpeed = 3.5f;
                    RunSpeed = 7f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 0f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                }
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void UpdateUI()
    {
        if (StaminaSlider != null) { StaminaSlider.value = Stamina; }
        if (HealthSlider != null) { HealthSlider.value = Health; }
        if (EstusCountText != null) { EstusCountText.text = CurrentEstus.ToString(); }
    }
    void StaminaRegen()
    {
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        if (Stamina < 100) { Stamina += Time.deltaTime * 50; } else { Stamina = 100; IsStaminaRegen = false; }

    }
    IEnumerator StaminaRegenPause()
    {
        IsStaminaRegen = false;
        yield return new WaitForSeconds(.5f);
        IsStaminaRegen = true;
    }



    IEnumerator WaitToRun()
    {
        IsWaitingToRun = true;

        yield return new WaitForSeconds(.25f);

        if (State != "Rolling") { State = "Running";}

        IsWaitingToRun = false;
    }
    IEnumerator Roll()
    {
        IsStaminaRegen = false;
        Anim.Play("PlayerAnim_Roll");

        yield return new WaitForSeconds(RollTime);

        MyRb.velocity = Vector2.zero;  

        if (IsMovingInput) 
        {
            State = "Walking"; 
        } 
        else
        {
            State = "Idle";
        }

        StartCoroutine(StaminaRegenPause()); 
   
    }
    IEnumerator Backstep()
    {
 
        IsStaminaRegen = false;
        Anim.Play("PlayerAnim_BackStep");     

        yield return new WaitForSeconds(BackstepTime);

        MyRb.velocity = Vector2.zero;

       

        if (IsMovingInput)
        {
            State = "Walking";
        }
        else
        {
            State = "Idle";
        }

        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator Jump()
    {
        ShouldCheckGrounded = false;
        Anim.Play("PlayerAnim_JumpEnter");

        if (MovementInputDirection == 0)
        {
            MyRb.velocity = new Vector2(0, JumpVerticalSpeed +1);
        }
        else
        {
            MyRb.velocity = new Vector2(JumpDirection * JumpRunHorizontalSpeed, JumpVerticalSpeed);
        }

        yield return new WaitForSeconds(.25f);

        IsJumping = false;
    
        State = "Falling";

        yield return new WaitForSeconds(.25f);
        ShouldCheckGrounded = true;
    }
    IEnumerator Land()
    {
        if (VerticalSpeed > 7) 
        {
            PlayerTakeDamage(30, true, 0);
            if (Health <= 0) { AchievementsGameManager.Instance.UnlockedAchievement(3);  }//deid to fall damage
        }


        StartCoroutine(StaminaRegenPause());

        if (JumpingCoroutine != null) { StopCoroutine(JumpingCoroutine); }

        Anim.Play("PlayerAnim_JumpLand");

        yield return new WaitForSeconds(.5f);

        IsLanding = false;
        CanPlunge = false;
        VerticalSpeed = FallSpeed;
        if (IsMovingInput) { State = "Walking"; } else { State = "Idle"; }     
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator LightAttack()
    {
        State = "Attacking";

        IsStaminaRegen = false;
        CanFollowUp = false;

        Anim.Play("PlayerAnim_LightSwing");

        yield return new WaitForSeconds(LightAttackTime-.25f);
   
        CanRollOut = true; CancelThisCoroutine = LightAttackCoroutine;
        CanFollowUp = true;  

        yield return new WaitForSeconds(.25f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUp = false;

        StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
    IEnumerator LightAttackFollowUp()
    {
        State = "Attacking";

        IsStaminaRegen = false;
        CanFollowUp = false;
        CanRollOut = false;

        Anim.Play("PlayerAnim_LightSwingFollowUpAttack");

        yield return new WaitForSeconds(LightFollowUpAttackTime-.25f);

        CanRollOut = true; CancelThisCoroutine = LightAttackFollowUpCoroutine;
        CanFollowUpAgain = true;

        yield return new WaitForSeconds(.25f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain = false;

        StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    IEnumerator HeavyAttack()
    {
        State = "Attacking";

        IsStaminaRegen = false;
        CanFollowUp = false;

        Anim.Play("PlayerAnim_HeavySwing");

        yield return new WaitForSeconds(HeavyAttackTime - .5f);
        
        CanRollOut = true; CancelThisCoroutine = HeavyAttackCoroutine;
        CanFollowUp = true;

        yield return new WaitForSeconds(.5f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUp = false; 

        StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
    IEnumerator HeavyAttackFollowUp()
    {
        State = "Attacking";

        IsStaminaRegen = false;
        CanFollowUp = false;
        CanRollOut = false;

        Anim.Play("PlayerAnim_HeavySwingFollowUpAttack");

        yield return new WaitForSeconds(HeavyFollowUpAttackTime - .2f);

        CanRollOut = true; CancelThisCoroutine = HeavyAttackFollowUpCoroutine;
        CanFollowUpAgain = true;

        yield return new WaitForSeconds(.2f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain = false;

        StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    public void AttackStep()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), .5f, layerMask);

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                MyRb.velocity = Vector2.zero;
            }
            else if (hit.transform.CompareTag("Demon"))
            {
              MyRb.velocity = Vector2.zero;                  
            }
            else
            {
                MyRb.velocity = new Vector2(PlayerDirection * StepDistance, 0);
            }
            
        }
        else
        {
            MyRb.velocity = new Vector2(PlayerDirection * StepDistance, 0);
        }
    }

    IEnumerator Stagger()
    {
        if (EstusUseCoroutine != null) { StopCoroutine(EstusUseCoroutine); }
        Anim.Play("PlayerAnim_StaggerGettingHit");
        State = "Stagger";

        yield return new WaitForSeconds(0.75f);

        CanRollOut = true; CancelThisCoroutine = StaggerCoroutine;

        yield return new WaitForSeconds(0.25f);

        CanRollOut = false; CancelThisCoroutine = null;

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        IsImmune = false;
    }

    IEnumerator Parry()
    {
        Anim.Play("PlayerAnim_Parry");
        State = "Attacking";

        yield return new WaitForSeconds(1f);

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }


    void HoldBlock()
    {
        IsBlocking = true;

        Anim.Play("PlayerAnim_ShieldBlockHold");
    }
    IEnumerator EnterBlock()
    {
        State = "Blocking";

        Anim.Play("PlayerAnim_ShieldBlockEnter");

        yield return new WaitForSeconds(0.5f);

        HoldBlock();
    }
    IEnumerator ExitBlock()
    {
        Anim.Play("PlayerAnim_ShieldBlockExit");

        yield return new WaitForSeconds(0.25f);

        IsBlocking = false;

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    IEnumerator UseEstus()
    {
        MyRb.velocity = Vector2.zero;
        Anim.Play("PlayerAnim_EstusUse");
        yield return new WaitForSeconds(1);

        Health += EstusHealAmount;
        Health = Mathf.Clamp(Health, 0, 100);
        CurrentEstus--;
        CanRollOut = true;
        CancelThisCoroutine = EstusUseCoroutine;
        CanUseSecondEstus = true;

        yield return new WaitForSeconds(.5f);

        CancelThisCoroutine = null;
        CanRollOut = false;
        CanUseSecondEstus = false;

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
    IEnumerator UseEmptyEstus()
    {
        MyRb.velocity = Vector2.zero;
        Anim.Play("PlayerAnim_EstusUseEmpty");
        yield return new WaitForSeconds(1f);
        CanRollOut = true;
        CancelThisCoroutine = EstusUseCoroutine;
        yield return new WaitForSeconds(.5f);
        CancelThisCoroutine = null;
        CanRollOut = false;
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    void PlungeAttackRegester()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), HeavyAttackFollowUpRange, layerMask);
        

        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Demon"))
            {
                hit.transform.GetComponent<AsylumDemon>().TakePlungeDamage();
                IsPlunging = false;
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakePlungeDamage();
                IsPlunging = false;
            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void GiveEstus(int num)
    {
        if (num == 6)
        {
            MaxEstus = 6;
            CurrentEstus = 6;
        }
        if (num == 3)
        {
           if (MaxEstus == 3) { MaxEstus = 6; CurrentEstus = 6; }
           else if (MaxEstus == 0) { MaxEstus = 3; CurrentEstus = 3; }
        }
       // Debug.Log("Max was " + MaxEstus + "is now " + num); 
    }

    public void HitWall(string WallHit)
    {
        WallHitType = WallHit;
        StartCoroutine(WallStagger());
    }

    IEnumerator WallStagger()
    {
        Anim.Play("PlayerAnim_StaggerHitObject");
        State = "Stagger";
        yield return new WaitForSeconds(.5f);
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
}
