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

    [Header("Combo")]
    public float CO_AttackDamage;
    public float CO_AttackAnimationTime;
    public float CO_AttackCoolDownTime;
    public float CO_AttackStep;
    public Collider2D CO1_Collider;
    public Collider2D CO2_Collider;



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
                    Anim.Play("ThePursuerAnim_Idle");
                    break;
                case "Hostile":

                    FacePlayer();

                    if (IsFacingPlayer && !IsByPlayer()) { Walk(); }
                    else { Anim.Play("ThePursuerAnim_Idle"); }


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

        AchievementsGameManager.Instance.UnlockedAchievement(2);

        Anim.Play("ThePursuerAnim_Death");

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
        Anim.Play("ThePursuerAnim_Idle");
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
        Anim.Play("ThePursuerAnim_MoveForward");
    }

    public void AttackStep()
    {
        RB.velocity = new Vector2(StepDistance * -LookDirection, 0);
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
            Anim.Play("ThePursuerAnim_Idle");
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
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

            case 2: //Double Hammer Swing
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

            case 3: //Ground Pound
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

            case 4: //Hammer Sweep
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

            case 5: //Hammer Drive
                AttackingCoroutine = StartCoroutine(CO_Attack());
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
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

            case 2: //Leaping Hammer Swing
                AttackingCoroutine = StartCoroutine(CO_Attack());
                break;

        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Combo Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CO_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CO_AttackStep;
        Anim.Play("ThePursuerAnim_ComboAttack");


        yield return new WaitForSeconds(CO_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(CO_AttackCoolDownTime);
        IsCoolingDown = false;
        
    }

    public void CO1_AttackRegister()
    {
        if (CO1_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(CO_AttackDamage, true, 0);
        }
    }
    public void CO2_AttackRegister()
    {
        if (CO2_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(CO_AttackDamage, true, 0);
        }
    }
}
