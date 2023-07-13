using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool IsAtOrigin;


    private Animator Anim;
    private GameObject Player;
    private Rigidbody2D RB;
    private Coroutine AttackingCoroutine;

    public GameObject Assets;
    public GameObject Arrow;
    public GameObject Eyes;
    public Transform HitStartPos;
    public Slider HealthSlider;
    public Vector3 OriginPosition;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Behaviour == null) { Behaviour = "Idle"; }
        if (LookDirection == 1) { Assets.transform.localScale = new Vector3(2, 2, 2); }
        else if (LookDirection == -1) { Assets.transform.localScale = new Vector3(-2, 2, 2); }

        IsAttacking = false;

        HealthSlider.maxValue = MaxHealth;

        if (Health > 0) { IsDead = false; } else { Dead(); Behaviour = "Dead"; }
    }

    public void Respawn()
    {
        IsDead = false;
        Assets.SetActive(true);
        gameObject.SetActive(true);
        Health = MaxHealth;
        HealthSlider.value = Health;
        transform.localPosition = OriginPosition;
        Behaviour = "Idle";
    }

    void Update()
    {
        UpdateUI();
        if (Health <= 0) { StartCoroutine(Death()); }
        else
        {
            switch (Behaviour)
            {
                case "Idle":
                    LookForPlayer();
                    Anim.Play("UndeadAnim_B_Idle");
                    if (SeePlayer) { Behaviour = "Hostile"; }
                    break;
                case "Hostile":
                    LookForPlayer();
                    if (SeePlayer && IsInAttackRange()) { Behaviour = "Attacking"; }
                    if (!SeePlayer) { Behaviour = "Idle";}
                    break;
                case "Attacking":
                    if (!IsAttacking) { AttackingCoroutine = StartCoroutine(Attack()); }
                    break;
                case "Staggered":
                    StartCoroutine(Staggered());
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

    IEnumerator Death()
    {
        Behaviour = "Dead";
        if (IsAttacking) { StopCoroutine(AttackingCoroutine); }
        HealthSlider.value = 0;
        Anim.Play("UndeadAnim_B_Death");
        yield return new WaitForSeconds(3);

        Dead();

    }

    public void Dead()
    {
        Health = 0;
        IsDead = true;
        Assets.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TakeLightDamage()
    {
        Health -= 6;
    }
    public void TakeHeavyDamage()
    {
        Health -= 9;
        Behaviour = "Staggered";
    }
    IEnumerator Staggered()
    {
        Anim.Play("UndeadAnim_B_GettingHit");
        yield return new WaitForSeconds(StaggerTime);
        Behaviour = "Hostile";
    }

    void UpdateUI()
    {
        HealthSlider.value = Health;
    }

    void LookForPlayer()
    {

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
                Debug.Log(hit.transform.name);
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
    void FacePlayer()
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

    public void ReleaseArrow()
    {
        GameObject arrow = Instantiate(Arrow);
        arrow.transform.position = HitStartPos.position;

        LayerManagerV2 Layer = Player.GetComponent<PlayerManager>().Layer;
       
        if (Layer.CurrentLayerNumber == 0) { arrow.transform.parent = Layer.BackLayer.transform; }
        if (Layer.CurrentLayerNumber == 1) { arrow.transform.parent = Layer.MiddleLayer.transform; }
        if (Layer.CurrentLayerNumber == 2) { arrow.transform.parent = Layer.FrontLayer.transform; }

        arrow.GetComponent<Arrow>().Direction = -LookDirection;
        arrow.GetComponent<Arrow>().ManualStart();
        arrow.GetComponent<Arrow>().Flying = true;
    }


}
