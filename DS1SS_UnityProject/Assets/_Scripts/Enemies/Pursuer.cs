using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pursuer : MonoBehaviour
{
    private Animator Anim;
    private GameObject Player;
    private PlayerControllerV2 PC;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;
    [SerializeField] private PursuerArena arenaManager;

    public GameObject Assets;
    public GameObject UIAssets;
    public Slider HealthSlider;
    public Vector3 OriginPosition;


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
    public Collider2D GP_Collider;

    [Header("Hammer Sweep")]
    public float HSP_AttackDamage;
    public float HSP_AttackAnimationTime;
    public float HSP_AttackCoolDownTime;
    public Collider2D HSP_Collider;

    [Header("Hammer Swing")]
    public float HSG_AttackDamage;
    public float HSG_AttackAnimationTime;
    public float HSG_AttackCoolDownTime;
    public Collider2D HSG_Collider;

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
    public float DHS_AttackDamage;
    public float DHS_AttackAnimationTime;
    public float DHS_AttackCoolDownTime;
    public Collider2D DHS_Collider;

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
    }
    public void ManualStart()
    {
        HasBeenPlunged = false;
        if (Behaviour == null) { Behaviour = "Idle"; }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;

        transform.localPosition = OriginPosition;

        IsAttacking = false;

        if (arenaManager.currentState == "Open") { Dead(); Behaviour = "Dead"; }

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }

        if (IsActive) { UIAssets.SetActive(true); } else { UIAssets.SetActive(false); }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// updates
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (Health <= 0) { StartCoroutine(Death()); }
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

                    if (IsFacingPlayer && !IsByPlayer()) { Walk(); }
                    else { Anim.Play("AsylumDemonAnim_Idle"); }


                    if (!IsCoolingDown)
                    {
                        if (IsInCloseRange()) { ChooseCloseRangeAttack(); }
                        else
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
                default:
                    Behaviour = "Idle";
                    break;
            }
        }
    }

    void UpdateUI()
    {
        HealthSlider.value = Health;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// death
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator Death()
    {
        Behaviour = "Dying";

        AchievementsGameManager.Instance.UnlockedAchievement(2);

        Anim.Play("AsylumDemonAnim_Death");

        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }

        HealthSlider.value = 0; UpdateUI();

        yield return new WaitForSeconds(3);

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
        Health -= 6; 
        UpdateUI();
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }
    }
    public void TakeHeavyDamage()
    {
        Health -= 9; 
        UpdateUI();
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }
    }
    public void TakePlungeDamage()
    {
        if (HasBeenPlunged == false) { Health -= 50; HasBeenPlunged = true; }
        UpdateUI();
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); }
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
        int attack = Random.Range(1, 6);
        Debug.Log("Close Attack " + attack);
        switch (attack)
        {
            case 1: //Hammer Swing
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

            case 2: //Double Hammer Swing
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

            case 3: //Ground Pound
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

            case 4: //Hammer Sweep
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

            case 5: //Hammer Drive
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

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
}
