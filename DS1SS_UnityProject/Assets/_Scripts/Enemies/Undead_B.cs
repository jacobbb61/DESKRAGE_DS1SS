using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
public class Undead_B : MonoBehaviour
{

    [Header("Stats")]
    public float Health;
    public float MaxHealth;
    public string Behaviour;
    public int LookDirection;

    [Header("Move and Detect Data")]
    public float ForwardDetectionRange;
    public float BackwardsDetectionRange;
    public float AttackTriggerRange;

    [Header("Combat Data")]
    public float TimeBeforeAttack;
    public float AttackAnimationTime;
    public float AttackCoolDownTime;
    public float StaggerTime;
    public float AttackDamage;

    [Header("Bools")]
    public bool SeePlayer;
    public bool IsAttacking;
    public bool IsDead;
    public bool IsDying;
    public bool IsAtOrigin;


    private Animator Anim;
    public EnemySaveManager EnemySaveManager;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;

    public GameObject Assets;
    public GameObject Arrow;
    public GameObject Eyes;
    public Transform HitStartPos;
    public Vector3 Target;
    public Slider HealthSlider;
    public Vector3 OriginPosition; 
    public TextMeshProUGUI DamagerNumber;
    public int DamageTakenInTime;

    [Header("Audio")]
    public EventReference GruntAudio;
    public EventReference HitAudio;
    public EventReference DeathAudio;

    private float RandomGruntTime;
    private float RandomGruntTarget;
    private void Start()
    {
        RandomGruntTarget = Random.Range(3, 10);
        RandomGruntTime = 0;
        ManualStart();
    }

    public void ManualStart()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        EnemySaveManager = GetComponent<EnemySaveManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }

        IsAttacking = false;
        IsDying = false;

        HealthSlider.maxValue = MaxHealth;

        if (Health > 0) { IsDead = false; IsDying = false; EnemySaveManager.IsLockOnAble = true; } else { Dead(); Behaviour = "Dead"; }
    }

    public void Respawn()
    {
        EnemySaveManager.IsLockOnAble = true;
        DamagerNumber.gameObject.SetActive(false);
        SeePlayer = false;
        IsDead = false;
        Assets.SetActive(true);
        gameObject.SetActive(true);
        Health = MaxHealth;
        HealthSlider.value = Health;
        transform.localPosition = OriginPosition;
        Behaviour = "Idle"; 
        SeePlayer = false;
    }

    void Update()
    {
        UpdateUI();
        if (Health > 0)
        {
            IsDead = false; IsDying = false;
            switch (Behaviour)
            {
                case "Idle":
                    LookForPlayer();
                    Anim.Play("UndeadAnim_B_Idle");
                    if (SeePlayer) { Behaviour = "Hostile"; }

                    if (RandomGruntTime >= RandomGruntTarget)
                    {
                        RandomGruntTime = 0;
                        RandomGruntTarget = Random.Range(2, 8);
                        RuntimeManager.PlayOneShot(GruntAudio, transform.position);

                    }
                    else
                    {
                        RandomGruntTime += Time.deltaTime;
                    }


                    break;
                case "Hostile":
                    
                    Anim.Play("UndeadAnim_B_Idle");
                    if (IsInAttackRange()) { Behaviour = "Attacking"; }


                    if (RandomGruntTime >= RandomGruntTarget)
                    {
                        RandomGruntTime = 0;
                        RandomGruntTarget = Random.Range(2, 8);
                        RuntimeManager.PlayOneShot(GruntAudio, transform.position);

                    }
                    else
                    {
                        RandomGruntTime += Time.deltaTime;
                    }

                    break;
                case "Attacking":
                    if (!IsAttacking) { AttackingCoroutine = StartCoroutine(Attack()); }
                    break;
                case "Staggered":
                    RB.velocity = Vector2.zero;
                    break;
                case "Dying":
                    RB.velocity = Vector2.zero;
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
    void Death()
    {
            DamagerNumber.gameObject.SetActive(false);

            EnemySaveManager.IsLockOnAble = false;
            RB.velocity = Vector2.zero;
            IsAttacking = false;
            EnemySaveManager.CanBeParry = false;

            Behaviour = "Dying";
            StopAllCoroutines();
            HealthSlider.value = 0;
            Anim.Play("UndeadAnim_B_Death");
            StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {

        yield return new WaitForSeconds(3);

        Dead();

    }
    private void OnDisable()
    {
        if (Behaviour == "Dying") { Dead(); }
        DamagerNumber.gameObject.SetActive(false);
        DamageTakenInTime = 0;
        DamagerNumber.text = DamageTakenInTime.ToString();
    }
    private void OnEnable()
    {
        if (Behaviour == "Attacking")
        {
            Behaviour = "Hostile";
        }
        IsAttacking = false;
        SeePlayer = false;
    }

    public void Dead()
    {
        Behaviour = "Dead";
        Health = 0;
        IsDead = true;
        Assets.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TakeLightDamage()
    {
        if (!IsDying)
        {
            Health -= 5;
            AddDamage(5);
            RuntimeManager.PlayOneShot(HitAudio, transform.position);
            if (Health <= 0) { Death(); RuntimeManager.PlayOneShot(DeathAudio, transform.position); return; }
            if (Behaviour == "Idle")
            {
                Behaviour = "Attacking";
            }
        }
    }
    public void TakeHeavyDamage()
    {
        if (!IsDying)
        {
            Health -= 10;
            AddDamage(10);
            RuntimeManager.PlayOneShot(HitAudio, transform.position);
            if (Health <= 0) { Death(); RuntimeManager.PlayOneShot(DeathAudio, transform.position); return; }
            if (AttackingCoroutine != null) { StopCoroutine(AttackingCoroutine); }
            StartCoroutine(Staggered());
        }
    }
    void AddDamage(int DMG)
    {
        Anim.Play("UndeadAnim_B_HitRegister_Hit");
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
    IEnumerator Staggered()
    {
        Anim.Play("UndeadAnim_B_GettingHit");
        yield return new WaitForSeconds(StaggerTime);
        Behaviour = "Hostile";
        IsAttacking = false;
        SeePlayer = true;
    }

    void UpdateUI()
    {
        HealthSlider.value = Health;
    }

    void LookForPlayer()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < 4)
        {
            SeePlayer = true;
            return;
        }
        float Range = 0;

        if (transform.position.x > Player.transform.position.x && LookDirection == 1) //looking towards player
        {
            Range = ForwardDetectionRange;
        }
        if (transform.position.x < Player.transform.position.x && LookDirection == -1) //looking towards player
        {
            Range = ForwardDetectionRange;
        }
        if (transform.position.x > Player.transform.position.x && LookDirection == -1) //looking away from player
        {
            Range = BackwardsDetectionRange;
        }
        if (transform.position.x < Player.transform.position.x && LookDirection == 1) //looking away from player
        {
            Range = BackwardsDetectionRange;
        }




        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < Range)
        {
            int layerMask = ~(LayerMask.GetMask("Ignore Raycast"));
            RaycastHit2D hit = Physics2D.Raycast(Eyes.transform.position, (Player.transform.position - Eyes.transform.position), Range, layerMask);
            if (hit.transform != null)
            {
               // Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Player"))
                {
                    SeePlayer = true;
                }
                else
                {
                    SeePlayer = false;
                }

            }
            else
            {
                SeePlayer = false;
            }
        }
        else
        {
            SeePlayer = false;
        }
    }
    public void FacePlayer()
    {
        if (transform.position.x > Player.transform.position.x)
        {
            LookDirection = 1;
        }
        else
        {
            LookDirection = -1;
        }

        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }
    }

    bool IsInAttackRange()
    {
        if (Vector3.Distance(Eyes.transform.position, Player.transform.position) < AttackTriggerRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        FacePlayer();
        Anim.Play("UndeadAnim_B_ShootingArrow");
        yield return new WaitForSeconds(AttackAnimationTime);
        Anim.Play("UndeadAnim_B_Idle");
        yield return new WaitForSeconds(AttackCoolDownTime);
        IsAttacking = false;
        Behaviour = "Idle";
    }

    public void AssignTarget()
    {
       Target = Player.transform.position;
    }

    public void ReleaseArrow()
    {
        GameObject arrow = Instantiate(Arrow);
        arrow.transform.position = HitStartPos.position;

        LayerManagerV2 Layer = Player.GetComponent<PlayerManager>().Layer;
       
        if (Layer.CurrentLayerNumber == 0) { arrow.transform.parent = Layer.BackLayer.transform; }
        if (Layer.CurrentLayerNumber == 1) { arrow.transform.parent = Layer.MiddleLayer.transform; }
        if (Layer.CurrentLayerNumber == 2) { arrow.transform.parent = Layer.FrontLayer.transform; }

        // arrow.GetComponent<Arrow>().Direction = -LookDirection;
        // arrow.GetComponent<Arrow>().ManualStart();
        // arrow.GetComponent<Arrow>().Flying = true;
        if (Target != null) { arrow.GetComponent<ArrowV2>().Target = Target; } else
        {
            arrow.GetComponent<ArrowV2>().Target = Player.transform.position;
        }
       

    }


}
