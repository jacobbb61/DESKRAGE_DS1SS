using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerControllerV2 PC;

    private void Start()
    {
        PC = GetComponentInParent < PlayerControllerV2 > ();
    }
    public void LightSwing()
    {
        PC.LightAttack();
    }
}
