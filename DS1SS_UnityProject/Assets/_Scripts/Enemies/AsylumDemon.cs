using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsylumDemon : MonoBehaviour
{   
    private Animator Anim;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;
    [SerializeField] private AsylumDemonArena arenaManager;

    public GameObject Assets;
    public GameObject UIAssets;
    public Transform HitStartPos;
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

    [Header("Combat Data")]
    public float Speed;
    public float TimeToTurn;
    public float StaggerTime;
    public float CloseTriggerRange;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Hammer Drive")]
    public float HD_AttackDamage;
    public float HD_AttackRangeDistnace;
    public float HD_AttackAnimationTime;
    public float HD_AttackCoolDownTime;
    public Transform HD_HitStartPos;

    [Header("Ranged Hammer")]
    public float RH_AttackDamage;
    public float RH_AttackRangeDistnace;
    public float RH_AttackAnimationTime;
    public float RH_AttackCoolDownTime;
    public Transform RH_HitStartPos;

    [Header("Ground Pound")]
    public float GP_AttackDamage;
    public float GP_AttackRangeDistnace;
    public float GP_AttackAnimationTime;
    public float GP_AttackCoolDownTime;
    public Transform GP_HitStartPos;

    [Header("Hammer Sweep")]
    public float HSP_AttackDamage;
    public float HSP_AttackRangeDistnace;
    public float HSP_AttackAnimationTime;
    public float HSP_AttackCoolDownTime;
    public Transform HSP_HitStartPos;

    [Header("Hammer Swing")]
    public float HSG_AttackDamage;
    public float HSG_AttackRangeDistnace;
    public float HSG_AttackAnimationTime;
    public float HSG_AttackCoolDownTime;
    public Transform HSG_HitStartPos;

    [Header("Hammer Back Swing")]
    public float HBS_AttackDamage;
    public float HBS_AttackRangeDistnace;
    public float HBS_AttackAnimationTime;
    public float HBS_AttackCoolDownTime;
    public Transform HBS_HitStartPos;

    [Header("Leaping Hammer Smash")]
    public float LH_AttackDamage;
    public float LH_AttackRangeDistnace;
    public float LH_AttackAnimationTime;
    public float LH_AttackCoolDownTime;
    public Transform LH_HitStartPos;

    [Header("Double Hammer Swing")]
    public float DHS_AttackDamage;
    public float DHS_AttackRangeDistnace;
    public float DHS_AttackAnimationTime;
    public float DHS_AttackCoolDownTime;
    public Transform DHS_HitStartPos;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// start
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ManualStart()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;


        IsAttacking = false;

        HealthSlider.maxValue = MaxHealth;

        if(arenaManager.currentState == "Open") { Dead(); Behaviour = "Dead"; }

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }

        if (IsActive) { UIAssets.SetActive(true); }else { UIAssets.SetActive(false); }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// updates
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        UpdateUI();
        if (Health <= 0) { StartCoroutine(Death()); }
        else
        {
            IsDead = false;
            switch (Behaviour)
            {
                case "Idle":
                    FacePlayer();
                    if (IsActive) { Behaviour = "Hostile"; UIAssets.SetActive(true); }
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
                           // ChooseLongRangeAttack();
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
        
        Anim.Play("AsylumDemonAnim_Death");

        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }

        HealthSlider.value = 0;
        
        yield return new WaitForSeconds(3);

        Behaviour = "Dead";
        Dead();

    }
    public void Dead()
    {
        arenaManager.BossKilled();
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
    }
    public void TakeHeavyDamage()
    {
        Health -= 9;
    }
    public void TakePlungeDamage()
    {
        Health -= 50;
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
        RB.velocity = new Vector2((-Speed - 1) * LookDirection, -5);
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
        int attack = Random.Range(1, 5);
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
        int attack = Random.Range(1, 2);
        Debug.Log("Long Attack " + attack);
        switch (attack)
        {
            case 1: //Ranged Hammer
                AttackingCoroutine = StartCoroutine(HD_Attack());
                break;

            case 2: //Leaping Hammer Swing
                AttackingCoroutine = StartCoroutine(HD_Attack());
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
        RaycastHit2D hit1 = Physics2D.Raycast(HD_HitStartPos.position, new Vector2(-1, 0), HD_AttackRangeDistnace/2);
        RaycastHit2D hit2 = Physics2D.Raycast(HD_HitStartPos.position, new Vector2(1, 0), HD_AttackRangeDistnace/2);

        bool hitplayer = false;

        if (hit1.collider != null)
        {
            if (hit1.transform.CompareTag("Player"))
            {
                hit1.transform.GetComponent<PlayerControllerV2>().PlayerTakeDamage(HD_AttackDamage, true, 0);
                hitplayer = true;
            }
        }
        
        if(hit2.collider != null && hitplayer == false)
        {
            if (hit2.transform.CompareTag("Player"))
            {
                hit2.transform.GetComponent<PlayerControllerV2>().PlayerTakeDamage(HD_AttackDamage, true, 0);
            }
        }
    }


}
