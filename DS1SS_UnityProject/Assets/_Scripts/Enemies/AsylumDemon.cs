using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AsylumDemon : MonoBehaviour
{
    public Animator Anim;
    private GameObject Player;
    private PlayerControllerV2 PC;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;
    [SerializeField] private AsylumDemonArena arenaManager;

    public GameObject Assets;
    public GameObject UIAssets;
    public Slider HealthSlider;
    public TextMeshProUGUI DamagerNumber;
    public Vector3 OriginPosition;
    public int DamageTakenInTime;


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
    public bool IsCoolingDown;
    public bool IsDead;
    public bool IsImmune;
    public bool HasBeenPlunged;

    [Header("Combat Data")]
    public float Speed;
    public float TimeToTurn;
    public float StepDistance;
    public float StaggerTime;
    public float CloseTriggerRange;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("TRigger Colliders")]
    public Collider2D InFrontCol;
    public Collider2D OnTopCol;
    public Collider2D BehindCol;

    [Header("Hammer Drive")]
    public float HD_AttackDamage;
    public float HD_AttackAnimationTime;
    public float HD_AttackCoolDownTime;
    public Collider2D HD_Collider;

    [Header("Ranged Hammer")]
    public float RH_AttackDamage;
    public float RH_AttackDamage_AOE;
    public float RH_AttackAnimationTime;
    public float RH_AttackCoolDownTime;
    public float RH_StepDistance;
    public Collider2D RH_Collider;
    public Collider2D RH_Collider_AOE;

    [Header("Ground Pound")]
    public float GP_AttackDamage;
    public float GP_AttackAnimationTime;
    public float GP_AttackCoolDownTime;
    public float GP_StepDistance;
    public Collider2D GP_Collider;

    [Header("Hammer Sweep")]
    public float HSP_AttackAnimationTime;
    public float HSP_AttackCoolDownTime;

    [Header("Hammer Front Swing")]
    public float HFS_AttackDamage;
    public float HFS_AttackAnimationTime;
    public float HFS_AttackCoolDownTime;
    public Collider2D HFS_Collider;

    [Header("Hammer Back Swing")]
    public float HBS_AttackDamage;
    public float HBS_AttackAnimationTime;
    public float HBS_AttackCoolDownTime;
    public Collider2D HBS_Collider;

    [Header("Leaping Hammer Smash")]
    public float LH_AttackDamage;
    public float LH_AttackAnimationTime;
    public float LH_AttackCoolDownTime;
    public float LH_StepDistance;
    public Collider2D LH_Collider;

    [Header("Double Hammer Swing")]
    public float DHS_AttackAnimationTime;
    public float DHS_AttackCoolDownTime;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// start
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PC = Player.GetComponent<PlayerControllerV2>();
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
        if (arenaManager.currentState == "FirstTime") { Assets.SetActive(false); }
    }
    public void ManualStart()
    {
        HasBeenPlunged = false;
        if (Behaviour == null) { Behaviour = "Idle"; }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;

        if (arenaManager.currentState == "FirstTime") { transform.localPosition = OriginPosition; }

        IsAttacking = false;

        transform.localPosition = OriginPosition;

        if (arenaManager.currentState == "Open") { Dead(); Behaviour = "Dead"; }

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }

        if (IsActive) { UIAssets.SetActive(true); }else { UIAssets.SetActive(false); }
    }

    public void ResetPos()
    {
        if (IsDead == false)
        {
            transform.localPosition = OriginPosition;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// updates
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (Health <= 0) { }// StartCoroutine(Death()); }
        else
        {
            IsDead = false;
            switch (Behaviour)
            {
                case "Idle":
                    Anim.Play("AsylumDemonAnim_Idle");
                    break;
                case "Hostile":
                    
                    FacePlayer();

                    if (IsFacingPlayer && !IsByPlayer()) {  Walk(); }
                    else { Anim.Play("AsylumDemonAnim_Idle"); }


                    if (!IsCoolingDown)
                    {
                        if (IsInCloseRange()) { ChooseCloseRangeAttack(); } else
                        {
                            ChooseLongRangeAttack();
                        }
                    }

                    break;
                case "Attacking":

                    break;
                case "Parried":
                    break;
                case "Dying":
                    
                    break;
                case "Dead":
                    Dead();
                    break;
                case "Cinematic":

                    break;
                case "FirstTime":
                    Assets.SetActive(false);
                    break;
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
    }

    public void UpdateUI()
    {
            HealthSlider.value = Health;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// death
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator Death()
    {
        Behaviour = "Dying";
        IsImmune = true;
        Player.GetComponent<PlayerControllerV2>().FadeOutMusic = true;
        Anim.Play("AsylumDemonAnim_Death");

        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }

        HealthSlider.value = 0; UpdateUI();

        yield return new WaitForSeconds(3.3f);

        Behaviour = "Dead";
        Dead();

    }
    public void Dead()
    {
        if (arenaManager.currentState != "Open") { arenaManager.BossKilled(); }
        Health = 0;
        IsDead = true;
        Assets.SetActive(false);
        gameObject.SetActive(false);
        
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// take damage
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void TakeLightDamage()
    {
        if (!IsImmune)
        {
            Health -= 5;
            AddDamage(5);
            UpdateUI();
            if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
            if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }
        }
    }
    public void TakeHeavyDamage()
    {
        if (!IsImmune)
        {
            Health -= 10;
            AddDamage(10);
            UpdateUI();
            if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
            if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }
        }
    }
    public void TakePlungeDamage()
    {
        if (HasBeenPlunged == false) { Health -= 150; HasBeenPlunged = true; }
        AddDamage(150);
        UpdateUI();
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }

    }

    public void ToggleImmunity()
    {
        IsImmune = !IsImmune;
    }

    void AddDamage(int DMG)
    {
        Anim.Play("AsylumDemonAnim_HitRegester_Hit");
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






    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// movement
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            else
            {
                IsFacingPlayer = true;
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
    public void InstantTurn()
    {
        if (transform.position.x > Player.transform.position.x)
        {
            LookDirection = 1;
        }
        else
        {
            LookDirection = -1;
        }
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

    public void AttackStep()
    {
        RB.velocity = new Vector2(StepDistance * -LookDirection, -5);
    }
    public void AttackStop()
    {
        RB.velocity = Vector2.zero;
    }

    bool IsByPlayer()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// attack triggers
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool IsInCloseRange()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < CloseTriggerRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool IsBehind()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < CloseTriggerRange)
        {
            Anim.Play("AsylumDemonAnim_Idle");
            return true;
        }
        else
        {
            return false;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// attack pick
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void ChooseCloseRangeAttack()
    {

        int SmartAttack = Random.Range(1, 3);
        if (SmartAttack == 1)//smart close range
        {
            if (InFrontCol.bounds.Contains(Player.transform.position))
            {
                int SmartAttack1 = Random.Range(1, 3);
                if (SmartAttack1 == 1) { AttackingCoroutine = StartCoroutine(HFS_Attack()); }
                else { AttackingCoroutine = StartCoroutine(HD_Attack()); }
            }
            else if (BehindCol.bounds.Contains(Player.transform.position))
            {
                AttackingCoroutine = StartCoroutine(HBS_Attack());
            }
            else if (OnTopCol.bounds.Contains(Player.transform.position))
            {
                int SmartAttack2 = Random.Range(1, 3);
                if (SmartAttack2 == 1) {  AttackingCoroutine = StartCoroutine(GP_Attack());}
                else { AttackingCoroutine = StartCoroutine(DHS_Attack()); }
            }
        }
        else //random close range
        {
            int attack = Random.Range(1, 7);
            Debug.Log("Close Attack " + attack);
            switch (attack)
            {
                case 1: //Hammer Swing
                    AttackingCoroutine = StartCoroutine(HFS_Attack());
                    break;

                case 2: //Hammer Back Swing
                    AttackingCoroutine = StartCoroutine(HBS_Attack());
                    break;

                case 3: //Double Hammer Swing
                    AttackingCoroutine = StartCoroutine(DHS_Attack());
                    break;

                case 4: //Ground Pound
                    AttackingCoroutine = StartCoroutine(GP_Attack());
                    break;

                case 5: //Hammer Sweep
                    AttackingCoroutine = StartCoroutine(HSP_Attack());
                    break;

                case 6: //Hammer Drive
                    AttackingCoroutine = StartCoroutine(HD_Attack());
                    break;

            }
        }
    }
    void ChooseLongRangeAttack()
    {
        int attack = Random.Range(1, 3);
        Debug.Log("Long Attack " + attack);
        switch (attack)
        {
            case 1: //Ranged Hammer
                AttackingCoroutine = StartCoroutine(RH_Attack());
                break;

            case 2: //Leaping Hammer Swing
                AttackingCoroutine = StartCoroutine(LH_Attack());
                break;

        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Hammer Drive
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator HD_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;

        Anim.Play("AsylumDemonAnim_HammerDrive");


        yield return new WaitForSeconds(HD_AttackAnimationTime);
        IsImmune = false;
        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");

        yield return new WaitForSeconds(HD_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void HD_AttackRegister()
    {
        if (HD_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(HD_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Ranged Hammer
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator RH_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = RH_StepDistance;

        Anim.Play("AsylumDemonAnim_RangedHammer");


        yield return new WaitForSeconds(RH_AttackAnimationTime);

        IsImmune = false;
        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(RH_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void RH_AttackRegister()
    {
        if (RH_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(RH_AttackDamage, true, 0);
        }
    }
    public void RH_AOE_AttackRegister()
    {
        if (RH_Collider_AOE.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(RH_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Leaping Hammer Smash
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator LH_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = LH_StepDistance;

        Anim.Play("AsylumDemonAnim_LeapingHammerSmash");


        yield return new WaitForSeconds(LH_AttackAnimationTime);

        IsImmune = false;
        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(LH_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void LH_AttackRegister()
    {
        if (LH_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(LH_AttackDamage, true, 0);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Ground Pound
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator GP_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = GP_StepDistance;

        Anim.Play("AsylumDemonAnim_GroundPoung");


        yield return new WaitForSeconds(GP_AttackAnimationTime);

        IsImmune = false;

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(GP_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void GP_AttackRegister()
    {
        if (GP_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(GP_AttackDamage, true, 0);
        }
    }    
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Hammer Swing Back
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator HBS_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;

        Anim.Play("AsylumDemonAnim_HammerBackSwing");


        yield return new WaitForSeconds(HBS_AttackAnimationTime);

        IsImmune = false;

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(HBS_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void HBS_AttackRegister()
    {
        if (HBS_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(HBS_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Hammer Swing front
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator HFS_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;

        Anim.Play("AsylumDemonAnim_HammerSwing");


        yield return new WaitForSeconds(HFS_AttackAnimationTime);

        IsImmune = false;

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(HFS_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void HFS_AttackRegister()
    {
        if (HFS_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(HFS_AttackDamage, true, 0);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Hammer Double Swing
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator DHS_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;

        Anim.Play("AsylumDemonAnim_DoubleHammerSwing");


        yield return new WaitForSeconds(DHS_AttackAnimationTime);

        IsImmune = false;

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(DHS_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Hammer Double Swing
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator HSP_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;

        Anim.Play("AsylumDemonAnim_HammerSweep");


        yield return new WaitForSeconds(HSP_AttackAnimationTime);

        IsImmune = false;

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("AsylumDemonAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(HSP_AttackCoolDownTime);
        IsCoolingDown = false;

    }
}
