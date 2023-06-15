using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<UndeadDummy>().TakeDamage(PC.LightAttackDamage, false);
            }
        }
    }
    public void LightAttackFollowUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.LightAttackFollowUpRange);


        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<UndeadDummy>().TakeDamage(PC.LightAttackDamage, false);
            }
        }
    }
    public void HeavyAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackRange);


        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<UndeadDummy>().TakeDamage(PC.HeavyAttackDamage, true);
            }
        }
    }
    public void HeavyAttackFollowUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(HitStartPos.position, new Vector2(PC.PlayerDirection, 0), PC.HeavyAttackFollowUpRange);


        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<UndeadDummy>().TakeDamage(PC.HeavyAttackDamage, true);
            }
        }
    }
}
