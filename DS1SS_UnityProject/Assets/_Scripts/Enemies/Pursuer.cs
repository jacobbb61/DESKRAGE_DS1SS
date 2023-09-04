using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pursuer : MonoBehaviour
{
    public Animator Anim; 
    public Collider2D Pursuer_Collider;
    public Collider2D Door_Collider;
    private GameObject Player;
    private PlayerControllerV2 PC;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;
    [SerializeField] private PursuerArena arenaManager;

    public GameObject Assets;
    public GameObject UIAssets;
    public Slider HealthSlider;
    public TextMeshProUGUI DamagerNumber;
    public Vector3 OriginPosition;
    public int DamageTakenInTime;

    public GameObject ParryIndicator;

    [Header("Stats")]
    public float Health;
    public float MaxHealth;
    public string Behaviour;

    public int LookDirection;
    public int CurrnetLookDirection;
    public bool IsFacingPlayer;
    public bool IsTurning;

    [Header("Bools")]
    public bool HitPlayer;
    public bool IsActive;
    public bool IsAttacking;
    public bool IsCoolingDown;
    public bool IsDead;
    public bool HasBeenPlunged;
    public bool CanPhaseChanged;
    public bool CanBeParry;

    [Header("Combat Data")]
    public float Speed;
    public float TimeToTurn;
    public float StepDistance;
    public float StaggerTime;
    public float CloseTriggerRange;


    [Header("TRigger Colliders")]
    public Collider2D InFrontCol;
    public Collider2D OnTopCol;

    public Transform left;
    public Transform right;



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Combo")] //close
    public float CO_AttackDamage;
    public float CO_AttackAnimationTime;
    public float CO_AttackCoolDownTime;
    public float CO_AttackStep;
    public Collider2D CO1_Collider;
    public Collider2D CO2_Collider;

    [Header("Charge")] //long
    public float CH_AttackDamage;
    public float CH_AttackAnimationTime;
    public float CH_AttackCoolDownTime;
    public float CH_AttackStep;
    public Collider2D CH_Collider;

    [Header("Cursed Impale")] //close
    public float CI_AttackDamage;
    public float CI_AttackAnimationTime;
    public float CI_AttackCoolDownTime;
    public float CI_AttackStep;
    public Collider2D CI_Collider;

    [Header("Cursed Shockwave")] //long 
    public float CS_AttackDamage;
    public float CS_AttackAnimationTime;
    public float CS_AttackCoolDownTime;
    public float CS_AttackStep;
    public Collider2D CS_Collider;
    public GameObject CursedShockwaveProjectile;
    public Transform CursedShockwavePos;

    [Header("Sheild Bash")] //close
    public float SB_AttackDamage;
    public float SB_AttackAnimationTime;
    public float SB_AttackCoolDownTime;
    public float SB_AttackStep;
    public Collider2D SB_Collider;

    [Header("Ground Pound")] //close/long   SECOND PHASE
    public float GP_AttackDamage;
    public float GP_AttackAnimationTime;
    public float GP_AttackCoolDownTime;
    public Collider2D GP_Collider;
    public Transform GroundPoundPos;

    [Header("Slam Finish")] //close
    public float SMF_AttackDamage;
    public float SMF_AttackAnimationTime;
    public float SMF_AttackCoolDownTime;
    public float SMF_AttackStep;
    public Collider2D SMF_Collider;

    [Header("Spinning Finish")] //close
    public float SPF_AttackDamage;
    public float SPF_AttackAnimationTime;
    public float SPF_AttackCoolDownTime;
    public float SPF_AttackStep;
    public Collider2D SPF_Collider;

    [Header("Double Stab")] //long
    public float DS_AttackDamage;
    public float DS_AttackAnimationTime;
    public float DS_AttackCoolDownTime;
    public float DS_AttackStepBack;
    public float DS_AttackStepForward;
    public int DS_AttackStepSwap;
    public Collider2D DS_Collider;


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
        if (Behaviour == null) { Behaviour = "FirstTime"; }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(1, 1, 1); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-1, 1, 1); }
        CurrnetLookDirection = LookDirection;

        transform.localPosition = OriginPosition;

        IsAttacking = false;

        if (arenaManager.currentState == "Open") { Dead(); Behaviour = "Dead"; }

        if (Health > 0) { IsDead = false; Assets.SetActive(true); } else { Dead(); Behaviour = "Dead"; }

        if (IsActive) { UIAssets.SetActive(true); } else { UIAssets.SetActive(false); }
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
                    Anim.Play("ThePursuerAnim_Idle");
                    break;
                case "Hostile":

                    Door_Collider.enabled = true;

                    FacePlayer();

                    if (IsFacingPlayer && !IsByPlayer()) { Walk(); }
                    else { Anim.Play("ThePursuerAnim_Idle"); }


                    if (!IsCoolingDown)
                    { 
                        if (CanPhaseChanged == true) { AttackingCoroutine = StartCoroutine(GP_Attack()); CanPhaseChanged = false; }
                        else
                        {
                            if (IsInCloseRange()) { ChooseCloseRangeAttack(); }
                            else
                            {
                                ChooseLongRangeAttack();
                            }
                        }
                    }

                    break;
                case "Attacking":

                    break;
                case "Parried":
                    break;
                case "Dying":

                    break;
                case "Cinematic":

                    break;
                case "FirstTime":
                    Assets.SetActive(false);
                    break;
                case "Dead":
                    Dead();
                    break;
                default:
                    Behaviour = "FirstTime";
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
        Player.GetComponent<EnemyLock>().LockedOn = false;


        Player.GetComponent<PlayerControllerV2>().FadeOutMusic = true;

        AchievementsGameManager.Instance.UnlockedAchievement(2);

        Anim.Play("ThePursuerAnim_Death");

        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }

        HealthSlider.value = 0; UpdateUI();

        yield return new WaitForSeconds(3.3f);
        Player.GetComponent<EnemyLock>().LockedOn = false;

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
        Health -= 5;
        UpdateUI(); 
        AddDamage(5);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); arenaManager.Victory(); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }
    public void TakeHeavyDamage()
    {
        Health -= 10;
        UpdateUI(); 
        AddDamage(10);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); arenaManager.Victory(); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }
    public void TakePlungeDamage()
    {
        if (HasBeenPlunged == false) { Health -= 100; HasBeenPlunged = true; }
        UpdateUI(); 
        AddDamage(100);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); arenaManager.Victory(); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }


    public void ToggleParryOn()
    {
        CanBeParry = true; 
        ParryIndicator.SetActive(true); 

    }
    public void ToggleParryOff()
    {
        CanBeParry = false;
        ParryIndicator.SetActive(false);
    }

    public void TriggerStagger()
    {
        ParryIndicator.SetActive(false);
        Behaviour = "Parried";
        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
        StartCoroutine(Staggered());
        RB.velocity = Vector2.zero;
        CanBeParry = false;
    }
    IEnumerator Staggered()
    {
        ParryIndicator.SetActive(false);
        CanBeParry = false;
        Anim.Play("StaggerPlaceholder");
        IsAttacking = false;
        ParryIndicator.SetActive(false);
        yield return new WaitForSeconds(StaggerTime);
        IsAttacking = false;
        Behaviour = "Hostile";
        CanBeParry = false;
    }

    void AddDamage(int DMG)
    {
        Anim.Play("Pursuer_HitRegester_Hit");
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
    bool IsONTopOfPlayer()
    {
        if (Pursuer_Collider.bounds.Contains(Player.transform.position))
        {
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
                int SmartAttack1 = Random.Range(1, 4);
                if (SmartAttack1 == 1) { AttackingCoroutine = StartCoroutine(CO_Attack()); }
                else if (SmartAttack1 == 2) { AttackingCoroutine = StartCoroutine(CI_Attack()); }
                else
                {
                 AttackingCoroutine = StartCoroutine(SB_Attack()); 
                }
            }
            else if (OnTopCol.bounds.Contains(Player.transform.position))
            {
                    AttackingCoroutine = StartCoroutine(DS_Attack());
                    StepDistance = DS_AttackStepBack;
            }





        }
        else //random close range
        {
            int attack = Random.Range(1, 5);
            Debug.Log("Close Attack " + attack);
            switch (attack)
            {

                case 1: //combo 
                    AttackingCoroutine = StartCoroutine(CO_Attack());
                    break;

                case 2: //DoubleStab
                    AttackingCoroutine = StartCoroutine(DS_Attack());
                    StepDistance = DS_AttackStepBack;
                    break;

                case 3: //cursed impale
                    AttackingCoroutine = StartCoroutine(CI_Attack());
                    break;

                case 4: //sheild bash
                    AttackingCoroutine = StartCoroutine(SB_Attack());
                    break;

            }
        }
     
    }
    void ChooseLongRangeAttack()
    {
        if (arenaManager.IsSecondPhase)
        {
            int attack = Random.Range(1, 5);
            Debug.Log("Long Attack " + attack);
            switch (attack)
            {

                case 1: //charge
                    AttackingCoroutine = StartCoroutine(CH_Attack());
                    break;

                case 2: //charge
                    AttackingCoroutine = StartCoroutine(CH_Attack());
                    break;

                case 3: //cursed shockwave
                    AttackingCoroutine = StartCoroutine(CS_Attack());
                    break;

                case 4: //GroundPound
                    AttackingCoroutine = StartCoroutine(GP_Attack());
                    break;

            }
        }
        else
        {
            int attack = Random.Range(1, 4);
            Debug.Log("Long Attack " + attack);
            switch (attack)
            {
                case 1: //charge
                    AttackingCoroutine = StartCoroutine(CH_Attack());
                    break;

                case 2: //charge
                    AttackingCoroutine = StartCoroutine(CH_Attack());
                    break;

                case 3: //cursed shockwave
                    AttackingCoroutine = StartCoroutine(CS_Attack());
                    break;

            }
        }
    }

    public void ChooseFinsiherAttack()
    {
        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
        int attack = Random.Range(1, 3);
        Debug.Log("Long Attack " + attack);
        switch (attack)
        {
            case 1: //Slam
                AttackingCoroutine = StartCoroutine(SMF_Attack());
                break;

            case 2: //Spinning
                AttackingCoroutine = StartCoroutine(SPF_Attack());
                break;

        }
    }

    public void HitPlayerReset()
    {
        HitPlayer = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Combo Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CO_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CO_AttackStep;
        Anim.Play("ThePursuerAnim_ComboAttack");


        yield return new WaitForSeconds(CO_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(CO_AttackCoolDownTime);
        IsCoolingDown = false;
        
    }

    public void CO1_AttackRegister()
    {
        if (CO1_Collider.bounds.Contains(Player.transform.position)&&HitPlayer==false)
        {
            PC.PlayerTakeDamage(CO_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }
    public void CO2_AttackRegister()
    {
        if (CO2_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(CO_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Charge Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CH_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CH_AttackStep;
        Anim.Play("ThePursuerAnim_Charge");


        yield return new WaitForSeconds(CH_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(CH_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CH_AttackRegister()
    {
        if (CH_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(CH_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Cursed Imaple Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CI_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CI_AttackStep;
        Anim.Play("ThePursuerAnim_CursedImpale");


        yield return new WaitForSeconds(CI_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(CI_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CI_AttackRegister()
    {
        if (CI_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(CI_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Cursed shockwave Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CS_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CS_AttackStep;
        Anim.Play("ThePursuerAnim_CursedShockwave");


        yield return new WaitForSeconds(CS_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(CS_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CS_AttackRegister()
    {
        if (CS_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(CS_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }
    public void CursedShockwaveFire()
    {
        GameObject Bolt = Instantiate(CursedShockwaveProjectile);
        Bolt.transform.position = CursedShockwavePos.position;
        Bolt.GetComponent<ArrowV2>().Target = Player.transform.position;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Shield Bash  Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator SB_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = SB_AttackStep;
        Anim.Play("ThePursuerAnim_ShieldBash");


        yield return new WaitForSeconds(SB_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(SB_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void SB_AttackRegister()
    {
        if (SB_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(SB_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Ground Pound  Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator GP_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = 0;
        Anim.Play("ThePursuerAnim_GroundPound");


        yield return new WaitForSeconds(GP_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(GP_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void GP_AttackRegister()
    {
         GP_Projectiles();

        if (GP_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(GP_AttackDamage, true, 1);
            HitPlayer = true;
        }
    }

    void GP_Projectiles()
    {
        GameObject Bolt1 = Instantiate(CursedShockwaveProjectile);
        Bolt1.transform.position = GroundPoundPos.position;
        Bolt1.GetComponent<ArrowV2>().Target = left.position;

        GameObject Bolt2 = Instantiate(CursedShockwaveProjectile);
        Bolt2.transform.position = GroundPoundPos.position;
        Bolt2.GetComponent<ArrowV2>().Target = right.position;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Slam Finisher  Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator SMF_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = SMF_AttackStep;
        Anim.Play("ThePursuerAnim_SlamFinisher");


        yield return new WaitForSeconds(SMF_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(SMF_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void SMF_AttackRegister()
    {
        if (SMF_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(SMF_AttackDamage, true, 1);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Spinning Finisher  Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator SPF_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = SPF_AttackStep;
        Anim.Play("ThePursuerAnim_SpinningFinisher");


        yield return new WaitForSeconds(SPF_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(SPF_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void DoubleSpin()
    {
        int attack = Random.Range(1, 6);
        switch (attack)
        {
            case 1: //Spinning
                StopCoroutine(AttackingCoroutine);
                AttackingCoroutine = StartCoroutine(SPF_Attack());
                break;
            default:
                return;
        }
    }

    public void SPF_AttackRegister()
    {
        if (SPF_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(SPF_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Double stab Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator DS_Attack()
    {
        HitPlayer = false;
        Behaviour = "Attacking";
        IsCoolingDown = false;
        Anim.Play("ThePursuerAnim_DoubleStab");


        yield return new WaitForSeconds(DS_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;
        HitPlayer = false;

        yield return new WaitForSeconds(DS_AttackCoolDownTime);
        IsCoolingDown = false;
        DS_AttackStepSwap = 0;
    }
    public void DS_AttackStepSwitch()
    {
        StepDistance = DS_AttackStepForward;
    }

    public void AttackPunish()
    {
        if (InFrontCol.bounds.Contains(Player.transform.position))
        {

            StopCoroutine(AttackingCoroutine);
            AttackingCoroutine = StartCoroutine(SPF_Attack());
            return;
        }

        else if (OnTopCol.bounds.Contains(Player.transform.position))
        {
            StopCoroutine(AttackingCoroutine);
            AttackingCoroutine = StartCoroutine(SMF_Attack());
        }
    }


    public void DS_AttackRegister()
    {

        if (DS_Collider.bounds.Contains(Player.transform.position) && HitPlayer == false)
        {
            PC.PlayerTakeDamage(DS_AttackDamage, true, 0);
            HitPlayer = true;
        }
    }
}
