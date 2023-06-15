using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public float Speed;
    public float VerticalSpeed;

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
    public bool IsAttackStepping;
    public bool CanAttack;
    public bool CanFollowUp;
    private bool SwapLightAnim;

    [Header("Combat Data to edit")]
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

    public InteractableV2 Interactable;

    //UI
    private CanvasManager CanvasManager;
    private Slider StaminaSlider;

    private void Start()
    {
        MyRb = GetComponent<Rigidbody2D>();
        MyCol = GetComponent<CapsuleCollider2D>();
        Speed = WalkSpeed;
        VerticalSpeed = FallSpeed;
        if (SceneManager.GetActiveScene().name == "Build")
        {
            CanvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            StaminaSlider = CanvasManager.PlayerStaminaSlider;
        }
        StartCoroutine(StaminaRegenPause());        
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
        if(CanMove && !IsUiOpen)
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

    }
    public void A(InputAction.CallbackContext context)
    {
        if (CanMove && !IsUiOpen)
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
        if (CanMove && IsGrounded && Interactable != null && !IsUiOpen)
        {
            if (context.action.triggered)
            {
                Interactable.InvokeEvent();
            }
        }

    }

    public void RT(InputAction.CallbackContext context)
    {

        if (CanMove && IsGrounded && !IsUiOpen && !CanFollowUp && CanAttack && Stamina > 0)
        {
            if (context.action.triggered)
            {
                HeavyAttackCoroutine = StartCoroutine(HeavyAttack());
                Stamina -= 35f;
            }
        }
        if (IsGrounded && !IsUiOpen && CanFollowUp && CanAttack && Stamina > 0)
        {
            if (context.action.triggered)
            {
                if (HeavyAttackCoroutine != null) { StopCoroutine(HeavyAttackCoroutine); }
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
        if (IsGrounded && !IsUiOpen && CanFollowUp && CanAttack && Stamina > 0)
        {
            if (context.action.triggered)
            {
                if (LightAttackCoroutine != null) { StopCoroutine(LightAttackCoroutine); }
                StartCoroutine(LightAttackFollowUp());
                Stamina -= 20f;
            }
        }

    }


    public void Update()
    {
        GroundCheck();
        UpdateUI();
        if (IsStaminaRegen) { StaminaRegen(); }
        if (Stamina <= 0 && IsGrounded) { StartCoroutine(StaminaRegenPause()); IsRunning = false; }


        if (PlayerDirection ==1 ) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (PlayerDirection ==-1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        if (IsGrounded && CanMove && IsIdleAnim && !IsMovingInput)
        {
            Anim.Play("Prototype_Idle");  
        }

        if (IsGrounded && CanMove && IsMovingInput)
        {
            
            if (MovementInputDirection > 0.2f) { PlayerDirection = 1; }
            if (MovementInputDirection < -0.2f) { PlayerDirection = -1; }
            if (!IsRunning)
            {
                MyRb.velocity = new Vector2(MovementInputDirection * WalkSpeed, MyRb.velocity.y);
                IsStaminaRegen = true;                
                Anim.Play("Prototype_Walking");
            }
            else if (IsRunning && Stamina > 0)
            {
                Stamina -= Time.deltaTime * 8.5f; IsStaminaRegen = false;
                MyRb.velocity = new Vector2(MovementInputDirection * RunSpeed, MyRb.velocity.y);
                Anim.Play("Prototype_Running");
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
        RaycastHit2D hitA = Physics2D.Raycast(GroundCheckPosA.position, Vector2.down, .1f);
        RaycastHit2D hitB = Physics2D.Raycast(GroundCheckPosB.position, Vector2.down, .1f);
        

        if(hitA.collider != null)
        {
            if (hitA.transform.CompareTag("Ground"))
            {
                IsGrounded = true;
            }
            else if (hitA.transform.CompareTag("Slope"))
            {
                IsGrounded = true;
                OnSlope();
            }
            else 
            {
                IsGrounded = false;
            }
        }
        if (hitB.collider != null)
        {
            if (hitB.transform.CompareTag("Ground"))
            {
                IsGrounded = true;
            }
            else if (hitB.transform.CompareTag("Slope"))
            {
                IsGrounded = true;
                OnSlope();
            }
            else
            {
                IsGrounded = false;
            }

        }
        if (hitA.collider == null && hitB.collider == null)
        {
            IsGrounded = false;
        }
        if (IsGrounded) { VerticalSpeed = 0.5f; } else { VerticalSpeed = FallSpeed; }
    }
    void OnSlope()
    {
        if (IsGrounded && !IsMovingInput && !IsJumping && !IsRolling) 
        { 
            MyRb.velocity = Vector2.zero; 
            MyRb.constraints = RigidbodyConstraints2D.FreezeAll; 
        } 
        else 
        {
            MyRb.constraints = RigidbodyConstraints2D.None;
            MyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


    public void UpdateUI()
    {
        if (StaminaSlider != null) { StaminaSlider.value = Stamina; }
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
        Anim.Play("Prototype_Roll");

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
        Anim.Play("Prototype_BackStep");
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
        Anim.Play("Prototype_Jumping");
        IsJumping = true;
        IsStaminaRegen = false;
        if (!IsRunning) { MyRb.velocity = new Vector2(MovementInputDirection * JumpHorizontalSpeed, JumpVerticalSpeed); }
        else { MyRb.velocity = new Vector2(MovementInputDirection * JumpRunHorizontalSpeed, JumpVerticalSpeed); }

        yield return new WaitForSeconds(.1f);

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
        IsJumping = false;
        VerticalSpeed = FallSpeed;
        MyRb.velocity = Vector2.zero;
        DodgeDirection = 0;
        IsIdleAnim = true;
        Anim.Play("Prototype_Idle");
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
    }

    IEnumerator LightAttack()
    {
        AttackStart();
        Anim.Play("Prototype_LightSwing");
        AttackStepMove(LightAttackStep);
        yield return new WaitForSeconds(LightAttackTime-0.35f);
        CanFollowUp = true;
        IsAttackStepping = false; 
        CanMove = true;
        CanAttack = true;
        yield return new WaitForSeconds(0.35f);
        CanFollowUp = false;
        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator LightAttackFollowUp()
    {
        AttackStart();
        Anim.Play("Prototype_LightSwingFollowUp");
        AttackStepMove(LightAttackStep);
        yield return new WaitForSeconds(LightFollowUpAttackTime);
        IsAttackStepping = false; 
        CanMove = true;
        CanAttack = true;
        StartCoroutine(StaminaRegenPause());
    }

    IEnumerator HeavyAttack()
    {
        AttackStart();
        Anim.Play("Prototype_HeavyAttack");
        AttackStepMove(HeavyAttackStep);
        CanFollowUp = false;
        yield return new WaitForSeconds(HeavyAttackTime - 0.35f);
        CanFollowUp = true;
        IsAttackStepping = false; 
        CanMove = true;
        CanAttack = true;
        yield return new WaitForSeconds(0.35f);
        CanFollowUp = false;
        StartCoroutine(StaminaRegenPause());
    }
    IEnumerator HeavyAttackFollowUp()
    {
        AttackStart();
        Anim.Play("Prototype_HeavyFollowUpAttack");
        AttackStepMove(HeavyAttackStep);
        yield return new WaitForSeconds(HeavyFollowUpAttackTime);
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
}
