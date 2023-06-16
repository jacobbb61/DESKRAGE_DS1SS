using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UndeadDummy : MonoBehaviour
{
    public bool IsDead;
    public float Health;
    public float MaxHealth;

    public Slider HealthBarSlider;
    public Animator Anim;
    public GameObject Assets;

    public Vector3 OriginPosition;
         

    public void Start()
    {
        if (Health <= 0) { Dead(); }
        HealthBarSlider.value = Health;
        HealthBarSlider.maxValue = MaxHealth;   
    }


    public void Respawn()
    {
        IsDead = true;
        Assets.SetActive(true);
        Health = MaxHealth;
        HealthBarSlider.value = Health;
        transform.localPosition = OriginPosition;
        Debug.Log("REspawned");
    }





    public void TakeDamage(float Damage, bool Staggered)
    {
        if (!IsDead)
        {
            Health -= Damage;

            HealthBarSlider.value = Health;

            if (Staggered)
            {
                Anim.Play("UndeadDumyStagger");
            }
            else
            {
                Anim.Play("UndeadDummyHit");
            }



            if (Health <= 0) { StartCoroutine(Death()); }
        }
    }

    IEnumerator Death()
    {
        HealthBarSlider.value = 0;
        Anim.Play("UndeadDummyDeath");
        yield return new WaitForSeconds(1);

        Dead();

    }

    public void Dead()
    {
        IsDead = true;
        Assets.SetActive(false);
    }
}
