using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEvents : MonoBehaviour
{
    public UnityEvent Attack1Triggered;
    public UnityEvent Attack2Triggered;
    public UnityEvent Attack3Triggered;
    public UnityEvent Attack4Triggered;


    public void Attack1()
    {
        Attack1Triggered.Invoke();
    }
    public void Attack2()
    {
        Attack2Triggered.Invoke();
    }
    public void Attack3()
    {
        Attack3Triggered.Invoke();
    }
    public void Attack4()
    {
        Attack4Triggered.Invoke();
    }
}
