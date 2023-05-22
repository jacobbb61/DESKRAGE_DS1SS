using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerController : MonoBehaviour, IAvatarActions
{
    private PlayerControls inputs;
    private Vector2 movement;

    private void Awake()
    {
        inputs = new PlayerControls();
        inputs.Avatar.SetCallbacks(this);
    }
    private void OnEnable()
    {
        if(inputs != null)
        {
            inputs.Enable();
        }
        else
        {
            Awake();
            OnEnable();
        }
    }

    private void OnDisable()
    {
        if(inputs !=null)
        {
            inputs.Disable();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * New Input system notes
     * 
     * context.started is true the frame the button starts to be pushed
     * context.performed is true while the button is down
     * context.cancelled is true the frame the button stops being pushed
     * 
     * context.duration is the length of time the button has been down for (context.time and context.startTime are also available)
     * 
     * context.ReadValue<TValue>() outputs the value of the action, in these cases, the Vector2 of the left stick and D-pad as follows
     * (for keyboard, these are composites made out of wasd and arrows)
     * 
     *      1
     *  
     * -1   0   1
     *      
     *     -1
     * */


    void IAvatarActions.OnBlock(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnHeavyAttack(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnLightAtatck(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnMiniUIMove(InputAction.CallbackContext context)
    {
        //this might get separated and moved later
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void IAvatarActions.OnOpenUI(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnParry(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnRollSprint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void IAvatarActions.OnUseItem(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
