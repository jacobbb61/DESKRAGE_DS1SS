using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IAvatarActions
{
    private Rigidbody2D myRb;
    public PlayerControls inputs;
    private Vector2 movement;
    public Interactable targetInteractable;
    private float dodgeTime;
    private bool uiOpen = false;
    private EnemyLock lockOn;
    private LayerManager layerManager;
    [SerializeField] private int faceDirection = 1;
    [SerializeField] private float facingSpeed;
    [SerializeField] private float backingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float runningTime;
    private bool layerSwapping;


    //turn around if lock on target is behind player

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
        FindObjectOfType<LayerManager>().OnLayerSwitch.AddListener(Transition);
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
        layerManager = FindObjectOfType<LayerManager>();
        lockOn = GetComponent<EnemyLock>();//might be getcomponentinchildren
        gameObject.layer = layerManager.GetLayer(layerManager.activeLayer).gameObject.layer;
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
        if(context.started)
        {
            if(targetInteractable!=null)
            {
                targetInteractable.Interact();
            }
        }
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
        //throw new System.NotImplementedException();
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
        if(uiOpen)
        {
            if(context.started)
            {
                //UIBack
            }
        }
        else if(context.performed)
        {
            if(context.duration > dodgeTime)
            {
                //run
            }
        }
        if(context.canceled)
        {
            if(context.duration <= dodgeTime)
            {
                //roll
            }
        }
    }

    void IAvatarActions.OnUseItem(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if(context.started)
        {
           // lockOn.running = !lockOn.running;
        }
    }

    internal void Transition(float t)
    {
        StartCoroutine(ChangeLayer(t));
    }

    internal IEnumerator ChangeLayer(float t)
    {
        layerSwapping = true;
        float time = 0;
        gameObject.layer = 9;
        while (time < t)
        {
            yield return null;
            time += Time.deltaTime;
        }
        gameObject.layer = layerManager.GetLayer().gameObject.layer;
        layerSwapping = false;
    }

    private IEnumerator Roll()
    {
        yield return null;
    }
}
