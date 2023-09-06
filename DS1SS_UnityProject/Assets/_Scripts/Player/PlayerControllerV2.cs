using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class PlayerControllerV2 : MonoBehaviour
{

    public GameObject BossCam;
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
    public float MovementInputAmount;
    public float PlayerDirection;
    private float JumpDirection;
    public bool IsUiOpen = false;
    public bool FadeOutMusic = false;

    [Header("movement states")]
    public bool IsGrounded;
    public bool IsGroundedOnSlope;
    public bool IsStaminaRegen;
    public bool CanMove;
    public bool IsRolling;
    private bool IsBackStepping;
    public bool IsRunning;
    public bool IsJumping;
    private bool ShouldCheckGrounded;
    private bool IsLanding;
    public bool IsMovingInput;
    private bool IsWaitingToRun;
    public bool DontDondgeOnThisRelease;
    private bool IsIdleAnim;
    public float VerticalSpeed;
    public bool FootAOnSlope;
    public bool FootBOnSlope;

    public bool GoingToRollUpSlope;
    public bool GoingToRollDownSlope;

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
    public bool IsPlunging;
    public bool CanPlunge;
    public bool CanUseSecondEstus;
    public bool CanAttack;
    public bool CanFollowUp;
    public bool CanFollowUpAgain;
    public bool CanFollowUpAgain2;

    public bool CanRollOut; //important


    [Header("Combat Data to edit")]
    public float EstusHealAmount;
    public float LightAttackTime;
    public float LightFollowUpAttackTime;
    public float LightFollowUpAttack2Time;

    public float HeavyAttackTime;
    public float HeavyFollowUpAttackTime;
    public float HeavyFollowUpAttack2Time;

    public float StepDistance;

    public float StaggerTime;

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
    private Coroutine StaminaRegenCoroutine;
    private Coroutine EstusUseCoroutine;

    private Coroutine AttackCoroutine;
    private Coroutine AttackFollowUpCoroutine;
    private Coroutine AttackFollowUpCoroutine2;



    private Coroutine EnterBlockCoroutine;
    private Coroutine ExitBlockCoroutine;

    private Coroutine CancelThisCoroutine;

    public Rigidbody2D MyRb;
    private Collider2D MyCol;
    private PlayerManager PM;
    public PlayerMenuManager PlayerMenuManager;
    private EnemyLock EnemyLock;

    public InteractableV2 Interactable;

    public OscarManager Oscar;

    public GameObject BloodImpact;


    public CatchupSliders CatchupSliders;

 

    public AsylumDemonArena AsylumDemonArena;
    public PursuerArena PursuerArena;
    float saved;
    public bool ahh = false;

    public EventReference Audio_BlockHit_Ref;
    public EventReference Audio_PlungeSwing_Ref;
    public EventReference Audio_Plunge_Ref;

    public GameObject LowHealthObj;


    //UI
    private CanvasManager CM;
    private Slider StaminaSlider;
    private Slider HealthSlider; 
    private Slider healthCatchupSlider;
    private Slider staminaCatchupSlider;
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
            healthCatchupSlider = CM.PlayerHealthCatchupSlider;
            staminaCatchupSlider = CM.PlayerStaminaCatchupSlider;
            EstusCountText = CM.EstusCountText;
            UpdateUI();
        }
         if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine);} StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());
        if(Health<= 20) { LowHealthObj.SetActive(true); } else
        {
            LowHealthObj.SetActive(false);
        }


        if (CurrentEstus > 4) { CurrentEstus = 4; }
        if (MaxEstus > 4) { MaxEstus = 4; }

        BossCam.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator PlayerDead()
    {
        MyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        FadeOutMusic = true;
        if (PM.HasDied == false) { PM.HasDied = true; }

        State = "Dead";
        CanMove = false;
        CanAttack = false;
        MyRb.velocity = Vector2.zero;
        CM.YouDiedAnim.Play("YouDied");
        Anim.Play("PlayerAnim_Death");
        yield return new WaitForSeconds(4.5f);

        if(PM.DemonArena.currentState == "Active") //player died to demon
        {
            PM.DemonArena.SwitchState("Idle");
            PM.DemonArena.DemonDeaths++;
            Oscar.DiedToDemon();
        }

        if(PM.PursuerArena.currentState == "Active" || PM.PursuerArena.inBossFight==true) //player died to pursuer
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
        BossCam.SetActive(false);

        yield return new WaitForSeconds(1f);



        MyRb.constraints = RigidbodyConstraints2D.None;
        MyRb.constraints = RigidbodyConstraints2D.FreezeRotation;


        Health = 100;
        healthCatchupSlider.value = 100;
        CanMove = true;
        CanAttack = true;

        IsRolling = false;
        IsImmune = false;
        IsBlocking = false;
        ahh = false;
       // State = "Idle";

    }

    public void PlayerTakeDamage(float Damage, bool staggger, int KnockDownDirection) // -1 to left, 1 to right, 0 is the direction the player is on relative to the enemy
    {
        if (!IsRolling && !IsImmune)
        {
            if (!staggger)
            {
                if (IsBlocking)
                {
                    Stamina -= Damage*1.5f;
                    StaminaCatchup();
                    Health -= Damage/4;
                    RuntimeManager.PlayOneShot(Audio_BlockHit_Ref, transform.position);
                    if (Stamina <= 0)
                    {
                        StartCoroutine(Stagger());
                        Stamina = 0;
                    }

                    StartCoroutine(CatchupSliders.ManualUpdate(true));
                }
                else
                {
                    Health -= Damage;
                    BloodEffect();
                    GetComponentInChildren<AnimationAudio>().PlayerDamageAudio();
                    if (PM.HasBeenHit == false && KnockDownDirection==0) { PM.HasBeenHit = true; }
                    StartCoroutine(CatchupSliders.ManualUpdate(true));
                }
            }
            else
            {
                if (IsBlocking)
                {
                    RuntimeManager.PlayOneShot(Audio_BlockHit_Ref, transform.position);
                }

                if (State != "Stagger" && KnockDownDirection==0)
                { 
                    if (PM.HasBeenHit == false && KnockDownDirection == 0) { PM.HasBeenHit = true; }               
                    if (StaggerCoroutine != null) { StopCoroutine(StaggerCoroutine); }
                    Health -= Damage; BloodEffect();
                    StaggerCoroutine = StartCoroutine(Stagger());
                    StartCoroutine(CatchupSliders.ManualUpdate(true));
                }
                if (State != "Stagger" && KnockDownDirection == 1)
                {
                    if (PM.HasBeenHit == false && KnockDownDirection == 0) { PM.HasBeenHit = true; }
                    if (StaggerCoroutine != null) { StopCoroutine(StaggerCoroutine); }
                    Health -= Damage; BloodEffect();
                    StaggerCoroutine = StartCoroutine(KnockDown());
                    StartCoroutine(CatchupSliders.ManualUpdate(true));
                }

            }
        }


      //  if (Health <= 0) { StartCoroutine(PlayerDead()); }
    }

    void BloodEffect()
    {
        Anim.Play("PlayerAnim_HitRegister_Hit");
        GameObject NewBlood = Instantiate(BloodImpact);
        NewBlood.transform.position = transform.position;
        Destroy(NewBlood, 0.25f);
    }

    public void PlayerFinishInteraction()
    {
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        IsRolling = false;
        IsImmune = false;
        IsBlocking = false;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    public void A(InputAction.CallbackContext context)
    {
        if (!IsJumping && IsGrounded || IsGroundedOnSlope)
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
            StaminaCatchup();
        }
    }
    //////////////////////////////////////////////////////////////
    public void B(InputAction.CallbackContext context)
    {
            switch (State)
            {
                case "Idle":
                    if (Stamina >= 10) ProcessInput_B(context);
                    break;
                case "Walking":
                    if (Stamina >= 10) ProcessInput_B(context);
                    break;
                case "Running":
                    if (Stamina >= 10) ProcessInput_B(context);
                    break;
                case "Blocking":
                    if (Stamina >= 10) ProcessInput_B(context); IsBlocking = false;
                    break;
            case "Rolling":
                    //if (CanRollOut) { if (Stamina >= 10) { ProcessInput_B(context); } }          
                    break;
                default:
                    break;
            
        }
    }
    void ProcessInput_B(InputAction.CallbackContext context)
    {
   
        if (context.action.WasPerformedThisFrame())
        {

                if (RollingCoroutine != null) { StopCoroutine(RollingCoroutine); }
                if (MovementInputDirection == 0) 
                {// trigger backstep
                    StartBackStep();
                }
                else 
                {// trigger roll
                    StartRoll();
                }
            

        }

    }
    void StartRoll()
    {
        if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }

        if (PlayerDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }

        RollingCoroutine =  StartCoroutine(Roll());

        Stamina -= 25f;
        StaminaCatchup();
        State = "Rolling";
    }
    void StartBackStep()
    {
        BacksteppingCoroutine = StartCoroutine(Backstep()); 

        Stamina -= 15f;
        StaminaCatchup();
        State = "BackStepping";
    }
    //////////////////////////////////////////////////////////////
    public void X(InputAction.CallbackContext context)
    {
        if (CanUseSecondEstus && CurrentEstus>0 && Health<MaxHealth)
        {
            State = "Estus";
            StopCoroutine(EstusUseCoroutine);
            CanUseSecondEstus = false;
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
                case "Running":
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
            if (context.action.triggered && CurrentEstus> 0 && Health < MaxHealth)
            {
            State = "Estus";
            EstusUseCoroutine = StartCoroutine(UseEstus());
            } 
            if(context.action.triggered && CurrentEstus == 0 && Health < MaxHealth)
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


        if (MovementInput.x > 0.1f && MovementInput.x <= 0.75f)
        {
            MovementInputDirection = 1;
            MovementInputAmount = 0.5f;
            IsMovingInput = true;
            //walking
        }
        else if (MovementInput.x < -0.1f && MovementInput.x >= -0.75f)
        {
            MovementInputDirection = -1;
            MovementInputAmount = 0.5f;
            IsMovingInput = true;
            //walking
        }
        else if (MovementInput.x > 0.75f && MovementInput.x <= 1f)
        {
            MovementInputDirection = 1;
            MovementInputAmount = 1;
            IsMovingInput = true;
            //running
        }
        else if (MovementInput.x < -0.75f && MovementInput.x >= -1f)
        {
            MovementInputDirection = -1;
            MovementInputAmount = 1;
            IsMovingInput = true;
            //running
        }


        switch (State)
        {
            case "Idle":
                ProccessMove(context);
                break;
            case "Walking":
                ProccessMove(context);
                break;
            case "Running":
                ProccessMove(context);
                break;
            case "Blocking":
                ProccessMove(context);
                break;
            case "Rolling":
                //if (CanRollOut) { if (Stamina >= 10) { ProcessInput_B(context); } }          
                break;
            default:
                break;

        }

        if (context.canceled)
        {
            MovementInputDirection = 0;
            IsMovingInput = false;
        }
    }
    void ProccessMove(InputAction.CallbackContext context)
    {
        Vector2 MovementInput;
        MovementInput = context.ReadValue<Vector2>();
        if (MovementInput.x > 0.1f && MovementInput.x <= 0.75f)
        {
            if (IsGrounded || IsGroundedOnSlope && IsMovingInput) State = "Walking";
        }
        else if (MovementInput.x < -0.1f && MovementInput.x >= -0.75f)
        {
            if (IsGrounded || IsGroundedOnSlope && IsMovingInput) State = "Walking";
        }
        else if (MovementInput.x > 0.75f && MovementInput.x <= 1f)
        {
            if (IsGrounded || IsGroundedOnSlope && IsMovingInput) State = "Running";
        }
        else if (MovementInput.x < -0.75f && MovementInput.x >= -1f)
        {
            if (IsGrounded || IsGroundedOnSlope && IsMovingInput) State = "Running";
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
        if (Stamina >= 0 && CanAttack)
        {
            if (CanFollowUp)
            {
                if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
                if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
                if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);
                ProcessInput_RT_Cancel();
            }
            else if (CanFollowUpAgain)
            {
                if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
                if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
                if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);
                ProcessInput_RT_CancelAgain();
            }
            else if (CanFollowUpAgain2)
            {
                if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
                if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
                if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);
                ProcessInput_RT_CancelAgain2();
            }
            else
            {
                switch (State)
                {
                    case "Idle":
                        if (context.action.triggered) ProcessInput_RT();
                        break;
                    case "Walking":
                        if (context.action.triggered) ProcessInput_RT();
                        break;
                    case "Running":
                        if (context.action.triggered) ProcessInput_RT();
                        break;
                    case "Blocking":
                        if (context.action.triggered) ProcessInput_RT(); IsBlocking = false;
                        break;                    
                    case "Falling":
                        if (context.action.triggered) ProcessInput_RT_Plunge();
                        break;
                    default:
                        break;
                }
            }
        }     

    }
    void ProcessInput_RT()
    {
        if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
        if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
        if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);

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
            AttackCoroutine = StartCoroutine(HeavyAttack());
            Stamina -= 22f;
            StaminaCatchup();
    }
    void ProcessInput_RT_Cancel()
    {
        if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
        if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
        if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);

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
        AttackFollowUpCoroutine = StartCoroutine(HeavyAttackFollowUp());
        Stamina -= 22f;
        StaminaCatchup();
        CanFollowUp = false;
    }
    void ProcessInput_RT_CancelAgain()
    {
        if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
        if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
        if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);

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
        AttackFollowUpCoroutine2 = StartCoroutine(HeavyAttackFollowUp2());
        Stamina -= 22f;
        StaminaCatchup();
        CanFollowUpAgain = false;
    }
    void ProcessInput_RT_CancelAgain2()
    {
        if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
        if (AttackFollowUpCoroutine != null) StopCoroutine(AttackFollowUpCoroutine);
        if (AttackFollowUpCoroutine2 != null) StopCoroutine(AttackFollowUpCoroutine2);

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
        AttackCoroutine = StartCoroutine(HeavyAttackFollowUp2());
        Stamina -= 22f;
        StaminaCatchup();
        CanFollowUpAgain = false;
    }
    void ProcessInput_RT_Plunge() 
    {
        if (IsPlunging==false && CanPlunge)
        {

            RuntimeManager.PlayOneShot(Audio_PlungeSwing_Ref, transform.position);
            IsPlunging = true;
            Stamina -= 40f;
            StaminaCatchup();
            CanPlunge = false;
        }
    }
    //////////////////////////////////////////////////////////////
    public void RB(InputAction.CallbackContext context)
    {
        if (Stamina >= 0 && CanAttack)
        {
            if (CanFollowUp)
            {
                if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
                if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
                if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
                ProcessInput_RB_Cancel();
            }
            else if (CanFollowUpAgain)
            {
                if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
                if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
                if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
                ProcessInput_RB_CancelAgain();
            }
            else if (CanFollowUpAgain2)
            {
                if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
                if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
                if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
                ProcessInput_RB_CancelAgain2();
            }
            else
            {
                switch (State)
                {
                    case "Idle":
                        if (context.action.triggered) ProcessInput_RB();
                        break;
                    case "Walking":
                        if (context.action.triggered) ProcessInput_RB();
                        break;
                    case "Running":
                        if (context.action.triggered) ProcessInput_RB();
                        break;
                    case "Blocking":
                        if (context.action.triggered) ProcessInput_RB(); IsBlocking = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    void ProcessInput_RB()
    {
        if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
        if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
        if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
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
            AttackCoroutine = StartCoroutine(LightAttack());
            Stamina -= 15f;
            StaminaCatchup();
    }
    void ProcessInput_RB_Cancel()
    {
        if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
        if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
        if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
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
        
        AttackFollowUpCoroutine = StartCoroutine(LightAttackFollowUp());
        Stamina -= 15f;
        StaminaCatchup();
        CanFollowUp = false;
    }
    void ProcessInput_RB_CancelAgain()
    {
        if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
        if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
        if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
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

        AttackFollowUpCoroutine2 = StartCoroutine(LightAttackFollowUp2());
        Stamina -= 15f;
        StaminaCatchup();
        CanFollowUpAgain = false;
    }
    void ProcessInput_RB_CancelAgain2()
    {
        if (AttackFollowUpCoroutine2 != null) { StopCoroutine(AttackFollowUpCoroutine2); }
        if (AttackFollowUpCoroutine != null) { StopCoroutine(AttackFollowUpCoroutine); }
        if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); }
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

        AttackCoroutine = StartCoroutine(LightAttack());
        Stamina -= 15f;
        StaminaCatchup();
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
            StaminaCatchup();
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
        if (context.action.WasPressedThisFrame() && State != "Blocking")
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

                EnterBlock();
        }
        if (context.action.WasReleasedThisFrame())
        {
            ExitBlock();
        }
    }
    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Update()
    {

        //
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //

        if (Health <= 0 && State!="Dead") { StartCoroutine(PlayerDead()); }
        if (State != "Dead")
        {
            if (Health <= 20) { LowHealthObj.SetActive(true); }
            else
            {
                LowHealthObj.SetActive(false);
            }
        }
        else
        {

            LowHealthObj.SetActive(false);
        }
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
                CanFollowUp = false;
                CanFollowUpAgain = false;
                CanFollowUpAgain2 = false;
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

        if (PlayerMenuManager != null) 
        {
          
        if (!FadeOutMusic)
        {
            saved = PlayerMenuManager.AudioMusicNum;
        }
        else
        {
            PlayerMenuManager.AudioMusicNum -= Time.deltaTime * 5;
            PlayerMenuManager.UpdateFMODSettings();
            Debug.Log(PlayerMenuManager.AudioMusicNum);

            if (PlayerMenuManager.AudioMusicNum <= 0)
            {
                AsylumDemonArena.StopMusic();
                PursuerArena.StopMusic();
                PlayerMenuManager.AudioMusicNum = saved;
                PlayerMenuManager.UpdateFMODSettings();
                FadeOutMusic = false;
            }
        }
    }




    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Idle()
    {

        IsBlocking = false;
        IsRolling = false;
        IsImmune = false;
        GoingToRollUpSlope = false;
        GoingToRollDownSlope = false;
        CanAttack = true;


        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }
        if (Stamina!=MaxStamina && IsStaminaRegen==false) {StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }



        Anim.Play("PlayerAnim_Idle");

        if (IsLockedOn) { FaceTowardsEnemy(); } else { FaceTowardsInput(); }
    }

    void Walking()
    {
        IsBlocking = false;
        IsRolling = false;
        IsImmune = false;
        CanAttack = true;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }
        if (Stamina != MaxStamina && IsStaminaRegen == false) { StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }




        if (MovementInputAmount == 1) { State = "Running"; }
        if (MovementInputDirection == 0) { State = "Idle"; }

        if (MovementInputDirection > 0.1f && !IsLockedOn) { PlayerDirection = 1; }
        if (MovementInputDirection < -0.1f && !IsLockedOn) { PlayerDirection = -1; }
        if (IsLockedOn) { FaceTowardsEnemy(); } else { FaceTowardsInput(); }
        


        MyRb.velocity = new Vector2(MovementInputDirection * WalkSpeed, -VerticalSpeed);

        if (!IsLockedOn) { Anim.Play("PlayerAnim_WalkForward"); }
        else 
        {
            if (GetComponent<EnemyLock>().enemyToRight)
            {
                if (MovementInputDirection < 0.75f && MovementInputDirection > 0.1f)
                {
                    Anim.Play("PlayerAnim_WalkBackward");
                }
                if (MovementInputDirection < -0.75f && MovementInputDirection < -0.1f)
                {
                    Anim.Play("PlayerAnim_WalkForward");
                }
            }
            else
            {
                if (MovementInputDirection < 0.75f && MovementInputDirection > 0.1f)
                {
                    Anim.Play("PlayerAnim_WalkForward");
                }
                if (MovementInputDirection < -0.75f && MovementInputDirection < -0.1f)
                {
                    Anim.Play("PlayerAnim_WalkBackward");
                }
            }
        }
        
        
        if (MovementInputDirection == 0) { State = "Idle"; }
    }

    void Running()
    {
        IsBlocking = false;
        IsRolling = false;
        IsImmune = false;
        CanAttack = true;


        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }
        if (Stamina != MaxStamina && IsStaminaRegen == false) { StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); }



        Anim.Play("PlayerAnim_Run");

            if (MovementInputDirection >= 0.75f) { PlayerDirection = 1;}
            if (MovementInputDirection <= -0.75f) { PlayerDirection = -1;}
            if (MovementInputAmount == 0.5f) { State = "Walk"; }
            if (MovementInputDirection == 0) { State = "Idle"; }
            FaceTowardsInput(); 

           
            

            MyRb.velocity = new Vector2(MovementInputDirection * RunSpeed, -VerticalSpeed);
        
    }

    void Rolling()
    {

        if (IsGrounded || IsGroundedOnSlope)
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
        if (IsGrounded || IsGroundedOnSlope) { MyRb.velocity = new Vector2(-PlayerDirection * BackstepSpeed, 0); }
        else { MyRb.velocity = new Vector2(-PlayerDirection * BackstepSpeed, -5); }
        
    }

    void Jumping()
    {
        IsRolling = false;
        IsJumping = true;
        IsGrounded = false;
        IsGroundedOnSlope = false;
        FootAOnSlope = false;
        FootBOnSlope = false;
        IsStaminaRegen = false;
    }
    void Falling()
    {

        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;

        IsGroundedOnSlope = false;
        IsRolling = false;
        IsImmune = false;

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


        if (IsGrounded && !IsJumping && !IsLanding && State == "Falling")
        {
            StartCoroutine(Land());
            IsLanding = true;
            IsPlunging = false;
            TimeFalling = 0;
        }
        if (IsGroundedOnSlope && !IsJumping && !IsLanding && State=="Falling")
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

            }
            else
            {
                Anim.Play("PlayerAnim_JumpIdle");
            }

            if (IsPlunging)
            {
                PlungeAttackRegester();
            }


        }


    }
    void Attacking()
    {

    }

    void Blocking()
    {
        MyRb.velocity = Vector2.zero;
        IsRolling = false;
        IsImmune = false;
        if (Stamina <= 0) //ran out of stamina, break blocking and stagger
        {
           StartCoroutine(Stagger());
           Stamina = 0;
        }
    }
    void MenuOpen()
    {
        IsRolling = false;
        IsImmune = false;
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
        if (State != "Dead") {
            if (!IsJumping && ShouldCheckGrounded)
            {

                int layerMask = ~(LayerMask.GetMask("Enemy"));
                RaycastHit2D hitA = Physics2D.Raycast(GroundCheckPosA.position, Vector2.down, .65f, layerMask);
                RaycastHit2D hitB = Physics2D.Raycast(GroundCheckPosB.position, Vector2.down, .65f, layerMask);

                if (hitA.collider != null)
                {
                    if (hitA.transform.CompareTag("Ground") || hitA.transform.CompareTag("Wall"))
                    {
                        IsGrounded = true;
                        FootAOnSlope = false;
                    }
                    else if (hitA.transform.CompareTag("Slope"))
                    {
                        IsGrounded = true;
                        IsGroundedOnSlope = true;
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
                    if (hitB.transform.CompareTag("Ground") || hitB.transform.CompareTag("Wall"))
                    {
                        IsGrounded = true;
                        FootBOnSlope = false;
                    }
                    else if (hitB.transform.CompareTag("Slope"))
                    {
                        IsGrounded = true;
                        IsGroundedOnSlope = true;
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
                            GroundType = hitA.collider.GetComponent<ObjectType_AudioRef>().ObjectType;
                        }
                    }
                    else if (hitB.collider != null)
                    {
                        if (hitB.transform.CompareTag("Ground") || hitB.transform.CompareTag("Slope"))
                        {
                            GroundType = hitB.collider.GetComponent<ObjectType_AudioRef>().ObjectType;
                        }
                    }

                }

                if (!IsGrounded && State != "Rolling" && State != "BackStepping")
                {
                    State = "Falling";
                    VerticalSpeed = 5f;
                }
                else { VerticalSpeed = FallSpeed; }

                if (State != "Blocking")
                {
                    if (FootAOnSlope && FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
                    else if (FootAOnSlope && !FootBOnSlope && hitA.transform != null) { OnSlope(hitA.transform); }
                    else if (!FootAOnSlope && FootBOnSlope && hitB.transform != null) { OnSlope(hitB.transform); }
                    else if (!FootAOnSlope && !FootBOnSlope)
                    {
                        //not on slope reset speeds
                        WalkSpeed = 2.75f;
                        RunSpeed = 6f;
                    }
                }
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

                    GoingToRollDownSlope = true;

                    WalkSpeed = 4f;
                    RunSpeed = 6.5f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 0f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                }
                else
                {//going with slope

                    GoingToRollUpSlope = true;


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
                    GoingToRollUpSlope = true;

                    WalkSpeed = 4f;
                    RunSpeed = 7.5f;
                    if (IsMovingInput)
                    {
                        VerticalSpeed = 0f;
                    }
                    else { MyRb.velocity = Vector2.zero; }
                }
                else
                {//going with slope

                    GoingToRollDownSlope = true;

                    WalkSpeed = 2f;
                    RunSpeed = 3.5f;
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
                    GoingToRollDownSlope = true;

                    WalkSpeed = 2.5f;
                    RunSpeed = 4.5f;
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

                    GoingToRollUpSlope = true;
                    WalkSpeed = 3.5f;
                    RunSpeed = 6f;
                    if (IsMovingInput) { VerticalSpeed = 0f; } else { MyRb.velocity = Vector2.zero; }
                }
            }
            else if (SlopeAngle < 325 && SlopeAngle > 300)
            {
                // left a 35 or MORE slope
                if (MovementInputDirection == 1)
                { //going with slope
                    GoingToRollUpSlope = true;

                    WalkSpeed = 2f;
                    RunSpeed = 3.5f;
                    if (IsMovingInput) { VerticalSpeed = 3f; } else { MyRb.velocity = Vector2.zero; }
                    if (FootAOnSlope && !FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                    if (!FootAOnSlope && FootBOnSlope) { WalkSpeed = 2f; if (IsMovingInput) { VerticalSpeed = 1.75f; } else { VerticalSpeed = 0f; } }
                }
                else
                {//going against slope
                    GoingToRollDownSlope = true;

                    WalkSpeed = 4f;
                    RunSpeed = 7.5f;
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
        if (Stamina < 100) { Stamina += Time.deltaTime * 80; } else { Stamina = 100; IsStaminaRegen = false; }

    }
    IEnumerator StaminaRegenPause()
    {
        IsStaminaRegen = false;
        yield return new WaitForSeconds(.3f);
        IsStaminaRegen = true;
        StaminaRegenCoroutine = null;
    }


    void StaminaCatchup()
    {
        if (staminaCatchupSlider.value > StaminaSlider.value && CatchupSliders.staminaCatchupActive == false)
        {
            StartCoroutine(CatchupSliders.ManualUpdate(false));
        }
        else if (staminaCatchupSlider.value < StaminaSlider.value)
        {
            staminaCatchupSlider.value = StaminaSlider.value;
        }
    }







    IEnumerator Roll()
    {
        IsStaminaRegen = false; 
        CanRollOut = false;
        Anim.Play("PlayerAnim_Roll");

        yield return new WaitForSeconds(RollTime-.25f);

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); } StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); 
        CanRollOut = true;

        yield return new WaitForSeconds(.25f);

        MyRb.velocity = Vector2.zero;  

        if (IsMovingInput) 
        {
            State = "Walking"; 
        } 
        else
        {
            State = "Idle";
        }
        CanRollOut = false;


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

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); }
        StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); 
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

        Anim.Play("PlayerAnim_JumpIdle");
        State = "Falling";

        yield return new WaitForSeconds(.25f);
        ShouldCheckGrounded = true;
    }
    IEnumerator Land()
    {

        if (VerticalSpeed > 7) 
        {
            PlayerTakeDamage(30, true, 1);
            if (Health <= 0) { AchievementsGameManager.Instance.UnlockedAchievement(3);  }//deid to fall damage
        }


        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); } StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause()); 

        if (JumpingCoroutine != null) { StopCoroutine(JumpingCoroutine); }

        Anim.Play("PlayerAnim_JumpLand");

        yield return new WaitForSeconds(.25f);

         MyRb.velocity = new Vector2(0, -5);
  
        CanPlunge = false;
        IsPlunging = false;
        VerticalSpeed = FallSpeed;
        if (IsMovingInput) { State = "Walking"; } else { State = "Idle"; }
        IsLanding = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator LightAttack()
    {
        State = "Attacking";
        StepDistance = 5;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_LightSwing");

        yield return new WaitForSeconds(LightAttackTime-.3f);
   
        CanRollOut = true; CancelThisCoroutine = AttackCoroutine;
        CanFollowUp = true;
        CanAttack = true;

        yield return new WaitForSeconds(.3f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUp = false;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine);} StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
    IEnumerator LightAttackFollowUp()
    {
        State = "Attacking"; 
        StepDistance = 5;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_LightSwingFollowUpAttack");

        yield return new WaitForSeconds(LightAttackTime - .3f);

        CanRollOut = true; CancelThisCoroutine = AttackFollowUpCoroutine;
        CanFollowUpAgain = true;
        CanAttack = true;

        yield return new WaitForSeconds(.3f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain = false;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine);} StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    IEnumerator LightAttackFollowUp2()
    {
        State = "Attacking";
        StepDistance = 10;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_LightSwingFollowUpAttack2");

        yield return new WaitForSeconds(LightFollowUpAttackTime - .25f);

        CanRollOut = true; CancelThisCoroutine = AttackFollowUpCoroutine;
        CanFollowUpAgain2 = true;
        CanAttack = true;

        yield return new WaitForSeconds(.25f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain2 = false;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); }
        StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }




    IEnumerator HeavyAttack()
    {
        State = "Attacking";
        StepDistance = 7;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_HeavySwingFollow2");

        yield return new WaitForSeconds(HeavyAttackTime - .4f);
        
        CanRollOut = true; CancelThisCoroutine = AttackCoroutine;
        CanFollowUp = true;
        CanAttack = true;

        yield return new WaitForSeconds(.4f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUp = false; 

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine);} StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
    IEnumerator HeavyAttackFollowUp()
    {
        State = "Attacking";
        StepDistance = 5.5f;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_HeavySwingFollowUpAttack");

        yield return new WaitForSeconds(HeavyFollowUpAttackTime - .5f);

        CanRollOut = true; CancelThisCoroutine = AttackFollowUpCoroutine;
        CanFollowUpAgain = true;
        CanAttack = true;

        yield return new WaitForSeconds(.5f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain = false;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine);} StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    IEnumerator HeavyAttackFollowUp2()
    {
        State = "Attacking";
        StepDistance = 4;
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        Anim.Play("PlayerAnim_HeavySwing");

        yield return new WaitForSeconds(HeavyFollowUpAttackTime - .5f);

        CanRollOut = true; CancelThisCoroutine = AttackFollowUpCoroutine;
        CanFollowUpAgain2 = true;
        CanAttack = true;

        yield return new WaitForSeconds(.5f);

        CanRollOut = false; CancelThisCoroutine = null;
        CanFollowUpAgain2 = false;

        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); }
        StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());

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
                MyRb.velocity = new Vector2(PlayerDirection * StepDistance/8, 0);
            }
            else if (hit.transform.CompareTag("Demon"))
            {
                MyRb.velocity = new Vector2(PlayerDirection * StepDistance/8, 0);
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
        StepDistance = 4;
    }

    public void AttackStepStop()
    {
        MyRb.constraints = RigidbodyConstraints2D.FreezePositionX;
        MyRb.velocity = new Vector2(0, 0);
        StepDistance = 4;
        MyRb.constraints = RigidbodyConstraints2D.None;
        MyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator KnockDown()
    {
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        if (EstusUseCoroutine != null) { StopCoroutine(EstusUseCoroutine); }
        Anim.Play("PlayerAnim_KnockDown");
        State = "Stagger";
        IsBlocking = false;

        yield return new WaitForSeconds(0.75f);

        IsStaminaRegen = true;
        CanRollOut = true; CancelThisCoroutine = StaggerCoroutine;

        yield return new WaitForSeconds(0.5f);

        CanRollOut = false; CancelThisCoroutine = null;


        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        IsImmune = false;
        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); }
        StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());
    }


    IEnumerator Stagger()
    {
        IsStaminaRegen = false;
        CanFollowUp = false;
        CanFollowUpAgain = false;
        CanFollowUpAgain2 = false;
        CanRollOut = false;
        CanAttack = false;

        if (EstusUseCoroutine != null) { StopCoroutine(EstusUseCoroutine); }
        Anim.Play("PlayerAnim_StaggerGettingHit");
        State = "Stagger";
        IsBlocking = false;

        yield return new WaitForSeconds(0.5f);

        IsStaminaRegen = true;
        CanRollOut = true; CancelThisCoroutine = StaggerCoroutine;

        yield return new WaitForSeconds(0.25f);

        CanRollOut = false; CancelThisCoroutine = null;
        

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
        IsImmune = false;
        if (StaminaRegenCoroutine != null) { StopCoroutine(StaminaRegenCoroutine); }
        StaminaRegenCoroutine = StartCoroutine(StaminaRegenPause());
    }

    IEnumerator Parry()
    {
        Anim.Play("PlayerAnim_Parry");
        State = "Attacking";

        yield return new WaitForSeconds(0.5f);

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }


    void HoldBlock()
    {
        //IsBlocking = true;

        Anim.Play("PlayerAnim_ShieldBlockHold");
    }
    void EnterBlock()
    {
        if (EnterBlockCoroutine != null) { StopCoroutine(EnterBlockCoroutine); }
        EnterBlockCoroutine = StartCoroutine(EnterBlockWait());
        State = "Blocking";
        Anim.Play("PlayerAnim_ShieldBlockEnter");

        IsBlocking = true;
    }
    IEnumerator EnterBlockWait()
    {
        yield return new WaitForSeconds(0.25f);

        HoldBlock();
    }

    void ExitBlock()
    {
       // if (ExitBlockCoroutine != null) { StopCoroutine(ExitBlockCoroutine); }
       // ExitBlockCoroutine = StartCoroutine(ExitBlockWait());

        IsBlocking = false;
        if (EnterBlockCoroutine != null) { StopCoroutine(EnterBlockCoroutine); }
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }


      //  Anim.Play("PlayerAnim_ShieldBlockExit");
    }

    IEnumerator ExitBlockWait()
    {
        yield return new WaitForSeconds(0.15f);

        IsBlocking = false;

        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }

    IEnumerator UseEstus()
    {
        MyRb.velocity = Vector2.zero;
        Anim.Play("PlayerAnim_EstusUse");
        yield return new WaitForSeconds(0.5f);

        Health += EstusHealAmount;
        Health = Mathf.Clamp(Health, 0, 100);
        CurrentEstus--;
        CanRollOut = true;
        CancelThisCoroutine = EstusUseCoroutine;
        CanUseSecondEstus = true;

        yield return new WaitForSeconds(0.5f);

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

                RuntimeManager.PlayOneShot(Audio_Plunge_Ref, transform.position);
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakePlungeDamage();
                IsPlunging = false;
                RuntimeManager.PlayOneShot(Audio_Plunge_Ref, transform.position);
            }

        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void GiveEstus(int num)
    {
        if (num == 4)
        {
            if (MaxEstus == 2) { MaxEstus = 4; CurrentEstus += 4; Oscar.EstusUI(2);  }
            else if (MaxEstus == 0) { MaxEstus = 4; CurrentEstus += 4; Oscar.EstusUI(4); }
        }
        if (num == 2)
        {
           if (MaxEstus == 2) { MaxEstus = 4; CurrentEstus += 2; Oscar.EstusUI(2); }
           else if (MaxEstus == 0) { MaxEstus = 2; CurrentEstus += 2; Oscar.EstusUI(2); }
        }

        if (CurrentEstus > MaxEstus)
        {
            if (MaxEstus == 2) { CurrentEstus = 2; }
            if (MaxEstus == 4) { CurrentEstus = 4; }
        }

        // Debug.Log("Max was " + MaxEstus + "is now " + num); 
    }

    public void HitWall(string WallHit)
    {
        WallHitType = WallHit;
        StartCoroutine(WallStagger());
        GetComponentInChildren<AnimationAudio>().HitObjectAudio(WallHit);
    }

    IEnumerator WallStagger()
    {
        Anim.Play("PlayerAnim_StaggerHitObject");
        State = "Stagger";
        yield return new WaitForSeconds(.5f);
        if (MovementInputDirection == 0) { State = "Idle"; } else { State = "Walking"; }
    }
}
