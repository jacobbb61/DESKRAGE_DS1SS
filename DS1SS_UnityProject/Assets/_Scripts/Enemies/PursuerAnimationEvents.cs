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
    public UnityEvent ParryOnEvent;
    public UnityEvent ParryOffEvent;
    public UnityEvent HitPlayerResetEvent;
    public UnityEvent Combo1Triggered;
    public UnityEvent Combo2Triggered;
    public UnityEvent ChargeTriggered;
    public UnityEvent CursedImpaleTriggered;
    public UnityEvent CursedShockwaveTriggered;
    public UnityEvent CursedShockwaveProjectileTriggered;
    public UnityEvent SheildBashTriggered;
    public UnityEvent GroundPoundTriggered;
    public UnityEvent ComboFinsherTriggered;
    public UnityEvent SlamFinsherTriggered;
    public UnityEvent SpinningFinsherTriggered;
    public UnityEvent DoubleStabTriggered;
    public UnityEvent DoubleStabPunishTriggered;
    public UnityEvent DoubleStabStepSwapTriggered;

    public GameObject ParryIndicator;
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
    public void ParryOn()
    {
        ParryOnEvent.Invoke(); 
    }   
    public void ParryOff()
    {
        ParryOffEvent.Invoke(); 
    }
    public void HitPlayerReset()
    {
        HitPlayerResetEvent.Invoke();
    }

    public void Combo1()
    {
        Combo1Triggered.Invoke();
    }
    public void Combo2()
    {
        Combo2Triggered.Invoke();
    }
    public void Charge()
    {
        ChargeTriggered.Invoke();
    }
    public void CursedImpale()
    {
        CursedImpaleTriggered.Invoke();
    }
    public void CursedShockwave()
    {
        CursedShockwaveTriggered.Invoke();
    }
    public void CursedShockwaveProjectile()
    {
        CursedShockwaveProjectileTriggered.Invoke();
    }
    public void SheildBash()
    {
        SheildBashTriggered.Invoke();
    }   
    public void GroundPound()
    {
        GroundPoundTriggered.Invoke();
    }
    public void ComboFinsher()
    {
        ComboFinsherTriggered.Invoke();
    }

    public void SlamTrigger()
    {
        SlamFinsherTriggered.Invoke();
    }

    public void SpinningFinsher()
    {
        SpinningFinsherTriggered.Invoke();
    }
    public void DoubleStab()
    {
        DoubleStabTriggered.Invoke();
    }
    public void DoubleStabPunish()
    {
        DoubleStabPunishTriggered.Invoke();
    }
    public void DoubleStabStepSwap()
    {
        DoubleStabStepSwapTriggered.Invoke();
    }
}