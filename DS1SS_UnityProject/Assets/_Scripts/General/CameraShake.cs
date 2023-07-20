using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Animator Anim;
    private PlayerControllerV2 PlayerControllerV2;

    public bool OutsideShake;

    private void Start()
    {
        PlayerControllerV2 = GetComponentInParent<PlayerControllerV2>();
    }

    public void CameraShake_LightShake()
    {
        Anim.Play("LightShake");
        OutsideShake = true;
    }

    public void CameraShake_HeavyShake()
    {
        Anim.Play("HeavyShake");
        OutsideShake = true;
    }

    public void CameraShake_PlayerStep()
    {
        if (!OutsideShake)
        {
            Anim.Play("PlayerStep");
        }
    }

    public void CameraShake_PlayerRunStep()
    {
        if (!OutsideShake)
        {
            Anim.Play("PlayerRunStep");
        }
    }
    public void CameraShake_AsylumDemonStep()
    {
        Anim.Play("AsylumDemonStep");
        OutsideShake = true;
    }


    public void CameraShake_DirectionalLightShake()
    {
        if (!OutsideShake)
        {
            if (PlayerControllerV2.PlayerDirection == -1)
            {
                Anim.Play("LeftlightShake");
            }
            else
            {
                Anim.Play("RightLightShake");
            }
        }
    }  
    public void CameraShake_OppositeDirectionalLightShake()
    {
        if (!OutsideShake)
        {
            if (PlayerControllerV2.PlayerDirection == 1)
            {
                Anim.Play("LeftlightShake");
            }
            else
            {
                Anim.Play("RightLightShake");
            }
        }
    }
    public void CameraShake_DirectionalHeavyShake()
    {
        if (!OutsideShake)
        {
            if (PlayerControllerV2.PlayerDirection == -1)
            {
                Anim.Play("LeftHeavyShake");
            }
            else
            {
                Anim.Play("RightHeavyShake");
            }
        }
    }


    public void CameraShake_END()
    {
        Anim.Play("Idle");
        OutsideShake = false;
    }



}
