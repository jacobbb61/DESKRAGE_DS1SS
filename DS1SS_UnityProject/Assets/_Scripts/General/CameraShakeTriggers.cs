using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShakeTriggers : MonoBehaviour
{
    public CameraShake CameraShake;

    public void CameraShake_NightmarePursuer()
    {
        CameraShake.CameraShake_NightmarePursuer();
    }

    public void CameraShake_LightShake()
    {
        CameraShake.CameraShake_LightShake();
    }

    public void CameraShake_HeavyShake()
    {
        CameraShake.CameraShake_HeavyShake();
    }

    public void CameraShake_PlayerStep()
    {
        CameraShake.CameraShake_PlayerStep();
    }

    public void CameraShake_PlayerRunStep()
    {
        CameraShake.CameraShake_PlayerRunStep();
    }
    public void CameraShake_AsylumDemonStep()
    {
        CameraShake.CameraShake_AsylumDemonStep();
    }


    public void CameraShake_DirectionalLightShake()
    {
        CameraShake.CameraShake_DirectionalLightShake();
    }
    public void CameraShake_OppositeDirectionalLightShake()
    {
        CameraShake.CameraShake_OppositeDirectionalLightShake();
    }
    public void CameraShake_DirectionalHeavyShake()
    {
        CameraShake.CameraShake_DirectionalHeavyShake();
    }

}
