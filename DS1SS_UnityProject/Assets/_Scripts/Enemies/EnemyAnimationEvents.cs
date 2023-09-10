using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

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
    public UnityEvent AttackAudioTriggered;

    public string GroundType;
    public EventReference WalkAudioRef;
    public EventReference AttackAudioRef;
    public EventReference AttackBowAudioRef;
    public EventReference AttackReleaseAudioRef;

    public bool isBoss;

    public GameObject ParryIndicator;
    public void WalkAudio()
    {
        if (!isBoss)
        {


            EventInstance walk = RuntimeManager.CreateInstance(WalkAudioRef);
            RuntimeManager.AttachInstanceToGameObject(walk, transform, GetComponentInParent<Rigidbody2D>());

            walk.setParameterByName("WalkorRun", 1);


            switch (GroundType)
            {
                case "Grass":
                    walk.setParameterByName("Terrain", 0);
                    break;
                case "StoneDirty":
                    walk.setParameterByName("Terrain", 1);
                    break;
                case "Stone":
                    walk.setParameterByName("Terrain", 2);
                    break;
                case "Wood":
                    walk.setParameterByName("Terrain", 3);
                    break;
                case "Snow":
                    walk.setParameterByName("Terrain", 5);
                    break;
                case "WetStone":
                    walk.setParameterByName("Terrain", 6);
                    break;
                default:
                    //RuntimeManager.PlayOneShot(WalkAudioRef, transform.position);
                    break;
            }

            walk.start();
            walk.release();
        }
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

    public void Attack1()
    {
        Attack1Triggered.Invoke();
    }
    public void AttackAudio()
    {
        if (!isBoss)
        {
            RuntimeManager.PlayOneShot(AttackAudioRef, transform.position);
            //FMODUnity.RuntimeManager.PlayOneShotAttached(AttackAudioRef, this.gameObject);
        }
    }
    public void AttackReleaseAudio()
    {

        RuntimeManager.PlayOneShot(AttackReleaseAudioRef, transform.position);
    }
    public  void Attack2()
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
        if (ParryIndicator != null) { ParryIndicator.gameObject.SetActive(!ParryIndicator.activeInHierarchy); }
    }
}
