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
    public UnityEvent DoubleSpinTriggered;
    public UnityEvent DoubleStabStepSwapTriggered;
    public UnityEvent InstantChargeTriggered;
    public UnityEvent DissapearTriggered;
    public UnityEvent ReapearTriggered;
    public UnityEvent FinishTriggered;
    public UnityEvent SpawnTriggered;

    public GameObject ParryIndicator;

    public void Spawn()
    {
        SpawnTriggered.Invoke();
    }
    public void Dissapear()
    {
        DissapearTriggered.Invoke();
    }
    public void Finish()
    {
        FinishTriggered.Invoke();
    }
    public void Reapear()
    {
        ReapearTriggered.Invoke();
    }

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
    public void Punish()
    {
        DoubleStabPunishTriggered.Invoke();
    }

    public void DoubleSpin()
    {
        DoubleSpinTriggered.Invoke();
    }

    public void InstantCharge()
    {
        InstantChargeTriggered.Invoke();
    }

    public void DoubleStabStepSwap()
    {
        DoubleStabStepSwapTriggered.Invoke();
    }
}