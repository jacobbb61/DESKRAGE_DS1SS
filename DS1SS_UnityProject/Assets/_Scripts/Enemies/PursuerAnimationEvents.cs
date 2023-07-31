using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PursuerAnimationEvents : MonoBehaviour
{

    public UnityEvent TurnEvent;
    public UnityEvent MoveEvent;
    public UnityEvent StopEvent;
    public UnityEvent ImmuneEvent;
    public UnityEvent Combo1Triggered;
    public UnityEvent Combo2Triggered;

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

    public void Combo1()
    {
        Combo1Triggered.Invoke();
    }
    public void Combo2()
    {
        Combo2Triggered.Invoke();
    }

}