using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerControllerV2 PC;
    public Transform HitStartPos;

    private void Start()
    {
        PC = GetComponentInParent < PlayerControllerV2 > ();
    }




    public void LightAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.LightAttackRange);


        if (hit.collider != null)
        {
            
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeLightDamageEvent.Invoke();
            }
            else
            {
                Debug.Log(hit.transform.name);
            }

        }
    }
    public void LightAttackFollowUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.LightAttackFollowUpRange);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeLightDamageEvent.Invoke();
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
    public void HeavyAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackRange);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeHeavyDamageEvent.Invoke();
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
    public void HeavyAttackFollowUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackFollowUpRange);


        if (hit.collider != null)
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySaveManager>().TakeHeavyDamageEvent.Invoke();
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
}
