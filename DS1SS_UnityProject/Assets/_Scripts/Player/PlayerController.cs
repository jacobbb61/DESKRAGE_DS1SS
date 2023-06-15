using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IAvatarActions
{
    [Header("Player Stats")]

    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRecharge;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed = 3.2f; //Distance = speed*canceltime?
    [SerializeField] private float rollSpeed = 4f, rollTime = 1.3f, rollCancelTime = 1f, rollStaminaCost = 30f, rollRechargePause = 0.65f; //Distance = speed*canceltime?
    [SerializeField] private float jumpSpeed = 3f, jumpTime = 2f / 3f, jumpCancelTime = 2f / 3f, jumpStaminaCost = 30f, jumpRechargePause = 0.65f; //Distance = speed*canceltime?
    [SerializeField] private float runSpeed = 4f, runStaminaCost = 8.5f, runRechargePause = 0.65f; //Distance = speed*canceltime?
    [SerializeField] private float backSpeed = 4f, backTime = 1.3f, backCancelTime = 1f, backStaminaCost = 30f, backRechargePause = 0.65f; //This is what happens instead of rolling when stationary


    [Header("Attack Values")]
    [SerializeField] private float lightAttackFieldDuration = 0.5f;
    [SerializeField] private float lightAttackWindupTime, lightAttackWindDownTime;

    public enum PlayerMode
    {
        DEFAULT,
        RUNNING,
        ROLLING,
        JUMPING,
        BACKSTEPPING,
        LIGHTATTACK,
        HEAVYATTACK,
        BLOCK,
        PARRY
    }
    [Header("Other")]
    [SerializeField]
    private PlayerMode mode;
    private GroudCheck gc;
    private Rigidbody2D myRb;
    public PlayerControls inputs;
    [SerializeField]
    private Vector2 movement;
    public Interactable targetInteractable;
    [SerializeField]
    private float actionTime = 0;
    private bool uiOpen = false;
    private EnemyLock lockOn;
    private LayerManager layerManager;
    [SerializeField] private int faceDirection = 1;
    [SerializeField]
    private bool layerSwapping;
    [SerializeField]
    private float lastActionRechargeTime;

    //turn around if lock on target is behind player

    private void Awake()
    {
        inputs = new PlayerControls();
        inputs.Avatar.SetCallbacks(this);
    }
    private void OnEnable()
    {
        if (inputs != null)
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
        if (inputs != null)
        {
            inputs.Disable();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroudCheck>();
        layerManager = FindObjectOfType<LayerManager>();
        lockOn = GetComponent<EnemyLock>();//might be getcomponentinchildren
        gameObject.layer = layerManager.GetLayer(layerManager.activeLayer).gameObject.layer;
        gc.gameObject.layer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(faceDirection, 1, 1);
        if (layerSwapping)
        {
            myRb.velocity = Vector2.zero;
            return;
        }
        actionTime += Time.deltaTime;
        switch (mode)
        {
            case PlayerMode.DEFAULT:
                if (gc.grounded)
                {
                    if (Mathf.Abs(movement.x) > 0.05f)
                    {
                        myRb.velocity = new Vector2(movement.x > 0 ? walkSpeed : -walkSpeed, myRb.velocity.y);
                        faceDirection = movement.x > 0 ? 1 : -1;
                    }
                    else
                    {
                        myRb.velocity = new Vector2(0, myRb.velocity.y);
                    }
                    if (actionTime >= lastActionRechargeTime)
                    {
                        currentStamina = Mathf.Min(currentStamina + staminaRecharge * Time.deltaTime, maxStamina);
                    }
                }
                break;
            case PlayerMode.RUNNING:
                if (Mathf.Abs(movement.x) > 0.05f)
                {
                    myRb.velocity = new Vector2(movement.x > 0 ? runSpeed : -runSpeed, 0f);
                    currentStamina = Mathf.Max(currentStamina - runStaminaCost * Time.deltaTime, myRb.velocity.y);
                    faceDirection = movement.x > 0 ? 1 : -1;
                    if (currentStamina <= 0.05f)
                    {
                        lastActionRechargeTime = runRechargePause;
                        mode = PlayerMode.DEFAULT;
                        actionTime = 0;
                    }
                }
                else
                {
                    myRb.velocity = new Vector2(0, myRb.velocity.y);
                }
                break;
            case PlayerMode.ROLLING://need to get facing direction and lock it in
                myRb.velocity = new Vector2(rollSpeed * faceDirection, myRb.velocity.y);
                if (actionTime > rollTime)
                {
                    mode = PlayerMode.DEFAULT;
                    lastActionRechargeTime = rollRechargePause;
                    actionTime = 0;
                }
                break;
            case PlayerMode.JUMPING:
                if (gc.grounded)
                {
                    myRb.velocity = new Vector2(myRb.velocity.x, 6f);//this initial upwards velocity is calculated
                }
                else
                {
                    mode = PlayerMode.DEFAULT;
                    lastActionRechargeTime = jumpRechargePause;
                    actionTime = 0;
                }
                break;
            default:
                break;
        }
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
        if (context.started)
        {
            if (targetInteractable != null)
            {
                targetInteractable.Interact();
                //Debug.Log("Interacted");
            }
        }
    }

    void IAvatarActions.OnJump(InputAction.CallbackContext context)
    {
        //check if grounded then jump if appropriate

        /* some motion maths for me in my head
         * v=u+at
         * s=ut+0.5att
         * vv=uu+2as
         * s=0.5vt+0.5ut
         * s=vt-0.5att
         * 
         * jump to apex
         * v=0
         * u=? //6
         * t=2/3
         * s=2
         * a=? //-9
         * 
         * s=0.5ut
         * 2=1/3u
         * u=6
         * 
         * s=-0.5att
         * 2=-0.5(4/9)a
         * a=-9
         */

        if (context.started && gc.grounded && currentStamina >= jumpStaminaCost)
        {
            currentStamina -= jumpStaminaCost;
            mode = PlayerMode.JUMPING;
        }
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
        if (uiOpen)
        {
            if (context.started)
            {
                //UIBack
            }
        }
        //if you want to change the time to do so go to the input manager
        if (context.interaction.ToString() == "UnityEngine.InputSystem.Interactions.TapInteraction")
        {
            if (context.performed)
            {
                if (mode == PlayerMode.DEFAULT && gc.grounded && currentStamina >= rollStaminaCost)
                {
                    actionTime = 0;
                    mode = PlayerMode.ROLLING;
                    currentStamina -= rollStaminaCost;
                    //stamina check, what do if not enough stamina?
                }
            }
        }
        if (context.interaction.ToString() == "UnityEngine.InputSystem.Interactions.HoldInteraction")
        {
            if (context.performed)
            {
                if (mode == PlayerMode.DEFAULT)
                {
                    mode = PlayerMode.RUNNING;
                }
            }
            if (context.canceled)
            {
                if (mode == PlayerMode.RUNNING)
                {
                    mode = PlayerMode.DEFAULT;
                }
            }
        }


        /*
        else if (context.performed)
        {
            if (context.duration > dodgeTime)
            {
                if (mode == PlayerMode.DEFAULT)
                {
                    mode = PlayerMode.RUNNING;
                }
            }
        }
        if (context.canceled)
        {
            if (context.duration <= dodgeTime)
            {
                if (mode == PlayerMode.DEFAULT)
                {
                    actionTime = 0;
                    mode = PlayerMode.ROLLING;
                    currentStamina -= rollStaminaCost;
                    //stamina check, what do if not enough stamina?
                }
            }
            if (context.duration > dodgeTime)
            {
                if (mode == PlayerMode.RUNNING)
                {
                    mode = PlayerMode.DEFAULT;
                }
            }
        }*/
    }

    void IAvatarActions.OnUseItem(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.started)
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
        Vector2 heldVelocity = myRb.velocity;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        layerSwapping = true;
        float time = 0;
        gameObject.layer = 9;
        foreach (Collider2D c in colliders)
        {
            c.gameObject.layer = gameObject.layer;
        }
        while (time < t)
        {
            yield return null;
            time += Time.deltaTime;
        }
        gameObject.layer = layerManager.GetLayer().gameObject.layer;
        foreach (Collider2D c in colliders)
        {
            c.gameObject.layer = gameObject.layer;
        }
        layerSwapping = false;
        myRb.velocity = new Vector2(heldVelocity.x, heldVelocity.y);
    }

}
