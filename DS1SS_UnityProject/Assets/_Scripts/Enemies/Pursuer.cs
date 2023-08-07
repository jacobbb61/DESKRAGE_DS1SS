using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pursuer : MonoBehaviour
{
    public Animator Anim; 
    public Collider2D Pursuer_Collider;
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
    public bool CanPhaseChanged;
    public bool CanBeParry;

    [Header("Combat Data")]
    public float Speed;
    public float TimeToTurn;
    public float StepDistance;
    public float StaggerTime;
    public float CloseTriggerRange;

    [Header("Trigger Colliders")] 
    public Collider2D InFront_Collider;
    public Collider2D OnTop_Collider;
    public Collider2D Behind_Collider;



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

        AchievementsGameManager.Instance.UnlockedAchievement(2);

        Anim.Play("ThePursuerAnim_Death");

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
        Health -= 5;
        UpdateUI(); 
        AddDamage(5);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }
    public void TakeHeavyDamage()
    {
        Health -= 10;
        UpdateUI(); 
        AddDamage(10);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }
    public void TakePlungeDamage()
    {
        if (HasBeenPlunged == false) { Health -= 150; HasBeenPlunged = true; }
        UpdateUI(); 
        AddDamage(150);
        if (Health <= 0) { StopAllCoroutines(); StartCoroutine(Death()); }
        if (Health <= MaxHealth / 2 && !arenaManager.IsSecondPhase) { arenaManager.SecondPhase(); CanPhaseChanged = true; }
    }


    public void ToggleParry()
    {
        CanBeParry = !CanBeParry;
    }
    public void TriggerStagger()
    {
        Behaviour = "Parried";
        if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
        StartCoroutine(Staggered());
        RB.velocity = Vector2.zero;
    }
    IEnumerator Staggered()
    {
        Anim.Play("StaggerPlaceholder");
        IsAttacking = false;
        yield return new WaitForSeconds(StaggerTime);
        IsAttacking = false;
        Behaviour = "Hostile";
    }

    void AddDamage(int DMG)
    {
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
        if (arenaManager.IsSecondPhase)
        {
            int attack = Random.Range(1, 5);
            Debug.Log("Close Attack " + attack);
            switch (attack)
            {
                case 1: //combo 
                    AttackingCoroutine = StartCoroutine(CO_Attack());
                    break;

                case 2: //combo 
                    AttackingCoroutine = StartCoroutine(CO_Attack());
                    break;

                case 3: //cursed impale
                    AttackingCoroutine = StartCoroutine(CI_Attack());
                    break;

                case 4: //sheild bash
                    AttackingCoroutine = StartCoroutine(SB_Attack());
                    break;

            }
        }
        else
        {
            int attack = Random.Range(1, 5);
            Debug.Log("Close Attack " + attack);
            switch (attack)
            {
                case 1: //combo 
                    AttackingCoroutine = StartCoroutine(CO_Attack());
                    break;

                case 2: //combo 
                    AttackingCoroutine = StartCoroutine(CO_Attack());
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
            int attack = Random.Range(1, 3);
            Debug.Log("Long Attack " + attack);
            switch (attack)
            {
                case 1: //charge
                    AttackingCoroutine = StartCoroutine(CH_Attack());
                    break;

                case 2: //cursed shockwave
                    AttackingCoroutine = StartCoroutine(CS_Attack());
                    break;

            }
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Charge Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CH_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CH_AttackStep;
        Anim.Play("ThePursuerAnim_Charge");


        yield return new WaitForSeconds(CH_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(CH_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CH_AttackRegister()
    {
        if (CH_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(CH_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Cursed Imaple Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CI_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CI_AttackStep;
        Anim.Play("ThePursuerAnim_CursedImpale");


        yield return new WaitForSeconds(CI_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(CI_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CI_AttackRegister()
    {
        if (CI_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(CI_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Cursed shockwave Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator CS_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = CS_AttackStep;
        Anim.Play("ThePursuerAnim_CursedShockwave");


        yield return new WaitForSeconds(CS_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(CS_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void CS_AttackRegister()
    {
        if (CS_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(CS_AttackDamage, true, 0);
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
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = SB_AttackStep;
        Anim.Play("ThePursuerAnim_ShieldBash");


        yield return new WaitForSeconds(SB_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(SB_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void SB_AttackRegister()
    {
        if (SB_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(SB_AttackDamage, true, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Ground Pound  Attack
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator GP_Attack()
    {
        Behaviour = "Attacking";
        IsCoolingDown = false;
        StepDistance = 0;
        Anim.Play("ThePursuerAnim_GroundPound");


        yield return new WaitForSeconds(GP_AttackAnimationTime);

        Behaviour = "Hostile";
        IsCoolingDown = true;
        Anim.Play("ThePursuerAnim_Idle");
        StepDistance = 0;

        yield return new WaitForSeconds(GP_AttackCoolDownTime);
        IsCoolingDown = false;

    }

    public void GP_AttackRegister()
    {
        GP_Projectiles();
        if (GP_Collider.bounds.Contains(Player.transform.position))
        {
            PC.PlayerTakeDamage(GP_AttackDamage, true, 0);
        }
    }

    void GP_Projectiles()
    {
        GameObject Bolt1 = Instantiate(CursedShockwaveProjectile);
        Bolt1.transform.position = GroundPoundPos.position;
        Bolt1.GetComponent<ArrowV2>().Target = Behind_Collider.transform.position;

        GameObject Bolt2 = Instantiate(CursedShockwaveProjectile);
        Bolt2.transform.position = GroundPoundPos.position;
        Bolt2.GetComponent<ArrowV2>().Target = InFront_Collider.transform.position;
    }
}
