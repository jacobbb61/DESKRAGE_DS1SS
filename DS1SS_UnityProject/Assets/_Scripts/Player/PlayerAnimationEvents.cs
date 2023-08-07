using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerControllerV2 PC;
    private AnimationAudio AnimationAudio;
    public Transform HitStartPos;
    private void Start()
    {
        PC = GetComponentInParent < PlayerControllerV2 > ();
        AnimationAudio = GetComponent <AnimationAudio> ();
    }


    public void RollStart()
    {
        PC.IsRolling = true;
    }
    public void RollEnd()
    {
        PC.IsRolling = false;
    }

    public void LightAttack()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.LightAttackRange, layerMask);


        if (hit.collider != null)
        {
            
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeLightDamageEvent.Invoke();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Demon"))
            {
                hit.transform.GetComponent<AsylumDemon>().TakeLightDamage();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakeLightDamage();
                AnimationAudio.HitMetal();
                return;
            }
            else if (hit.transform.CompareTag("Wall"))
            {
                PC.HitWall(hit.transform.GetComponent<ObjectType_AudioRef>().ObjectType);
            }
            else if (hit.transform.CompareTag("Pot"))
            {
                hit.transform.GetComponent<Pot>().BreakPot();
            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }

        }
    }
    public void LightAttackFollowUp()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.LightAttackFollowUpRange, layerMask);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeLightDamageEvent.Invoke();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Demon"))
            {
                hit.transform.GetComponent<AsylumDemon>().TakeLightDamage();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakeLightDamage();
                AnimationAudio.HitMetal();
                return;
            }
            else if (hit.transform.CompareTag("Wall"))
            {
                PC.HitWall(hit.transform.GetComponent<ObjectType_AudioRef>().ObjectType);
            }
            else if (hit.transform.CompareTag("Pot"))
            {
                hit.transform.GetComponent<Pot>().BreakPot();
            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }
        }
    }
    public void HeavyAttack()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackRange, layerMask);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeHeavyDamageEvent.Invoke(); 
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Demon"))
            {
                hit.transform.GetComponent<AsylumDemon>().TakeHeavyDamage();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakeHeavyDamage();
                AnimationAudio.HitMetal();
                return;
            }
            else if (hit.transform.CompareTag("Wall"))
            {
                PC.HitWall(hit.transform.GetComponent<ObjectType_AudioRef>().ObjectType);
            }
            else if (hit.transform.CompareTag("Pot"))
            {
                hit.transform.GetComponent<Pot>().BreakPot();
            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }
        }
    }
    public void HeavyAttackFollowUp()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackFollowUpRange, layerMask);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeHeavyDamageEvent.Invoke();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Demon"))
            {
                hit.transform.GetComponent<AsylumDemon>().TakeHeavyDamage();
                AnimationAudio.HitFlesh();
                return;
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {
                hit.transform.GetComponent<Pursuer>().TakeHeavyDamage();
                AnimationAudio.HitMetal();
                return;
            }
            else if (hit.transform.CompareTag("Wall"))
            {
                PC.HitWall(hit.transform.GetComponent<ObjectType_AudioRef>().ObjectType);
            }
            else if (hit.transform.CompareTag("Pot"))
            {
                hit.transform.GetComponent<Pot>().BreakPot();
            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }
        }
    }

    public void AttackStep()
    {
        PC.AttackStep();
    }

    public void Parry()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackFollowUpRange, layerMask);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                if (hit.transform.GetComponent<EnemySaveManager>().CanBeParry == true)
                {
                    hit.transform.GetComponent<EnemySaveManager>().ParryEvent.Invoke();
                }
            }
            else if (hit.transform.CompareTag("Pursuer"))
            {

                if (hit.transform.GetComponent<Pursuer>().CanBeParry == true)
                {
                    hit.transform.GetComponent<Pursuer>().TriggerStagger();
                }

            }
            else
            {
                Debug.Log("Player hit" + hit.transform.name);
            }
        }
    }

}
