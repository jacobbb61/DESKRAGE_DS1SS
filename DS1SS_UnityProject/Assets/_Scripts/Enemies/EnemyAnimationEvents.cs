using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEvents : MonoBehaviour
{
    public UnityEvent TurnEvent;
    public UnityEvent MoveEvent;
    public UnityEvent StopEvent;
    public UnityEvent ImmuneEvent;
    public UnityEvent Attack1Triggered;
    public UnityEvent Attack2Triggered;
    public UnityEvent Attack3Triggered;
    public UnityEvent Attack4Triggered;
    public UnityEvent Attack5Triggered;
    public UnityEvent Attack6Triggered;
    public UnityEvent Attack7Triggered;
    public UnityEvent Attack8Triggered;
    public UnityEvent Attack9Triggered;


    public void Turn()
    {
        TurnEvent.Invoke();
    }
    public void Move()
    {
        MoveEvent.Invoke();
    }
    public void Stop()
    {
        StopEvent.Invoke();
    }
    public void Immune()
    {
        ImmuneEvent.Invoke();
    }

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
    public void Attack5()
    {
        Attack5Triggered.Invoke();
    }
    public void Attack6()
    {
        Attack6Triggered.Invoke();
    }
    public void Attack7()
    {
        Attack7Triggered.Invoke();
    }
    public void Attack8()
    {
        Attack8Triggered.Invoke();
    }
    public void Attack9()
    {
        Attack9Triggered.Invoke();
    }
}
