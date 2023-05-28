using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
public struct InteractionSettings
{
    public Camera cam;
    public LayerMask interactionMask;
    public LayerMask visionMask;
    public int interactionDistance;
}

[RequireComponent(typeof(CharacterController)), 
    RequireComponent(typeof(Inventory)), 
    RequireComponent(typeof(CameraController)), 
    RequireComponent(typeof(Insanity)), 
    RequireComponent(typeof(Lockpick))]
public class FpsController : MonoBehaviour
{
    public bool invincible;

    [Header("Components")]
    public Insanity insanity;
    public CameraController cameraController;
    public Lockpick lockpick;
    public Journal journal;
    [SerializeField] CharacterController controller;
    [SerializeField] Inventory inventory;
    [SerializeField] ActionBar bar;
 
    [Header("Movement")]
    [SerializeField] float forwardScale;
    [SerializeField] float strafeScale;
    [SerializeField] float walkSpeed;
    [SerializeField] float accelStep;

    [Header("Sneaking")]
    [SerializeField] float sneakSpeed;
    [SerializeField] float sneakAccelStep;

    [Header("Gravity")]
    [SerializeField] float gravity;

    [Header("Ground Check")]
    [Range(0.1f, 10)] [SerializeField] float groundCheckExtension;
    public LayerMask whatIsGround;

    [Header("Slope Handling")]
    [SerializeField] float slopeCheckExtension;
    [SerializeField] float maxSlopeAngle;

    [Header("Other Settings")]
    [SerializeField] LayerMask walls;
    [Header("")]
    [SerializeField] float distancePerFootstep;
    [Header("")]
    public InteractionSettings settings;
    [Header("")]
    [SerializeField] float spawnYRot;

    public PlayerInputActions PlayerInputActions { get { return playerInputActions; } }
    public bool Dead { get; private set; }

    PlayerInputActions playerInputActions;

    Vector3 lastPosition;

    Vector3 moveDir;
    Vector3 forces;
    Vector3 verticalVelocity;
    Vector3 velocity;

    float speedMultiplier;
    float walked;
    uint steps;

    bool Grounded
    {
        get
        {
            return Physics.CheckSphere(transform.position + controller.center - new Vector3(0, controller.height / 2 - controller.radius + groundCheckExtension, 0), controller.radius - 0.1f, whatIsGround);
        }
    }
    bool sneaking;

    private void Awake()
    {
        speedMultiplier = 1;
        lastPosition = transform.position;

        #region Input

        playerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();

        PlayerInputActions.General.Interact.performed += Interact;
        PlayerInputActions.General.Interact.canceled += context => lockpick.StopPicking();
        PlayerInputActions.General.Interact.canceled += context => lockpick.StopLocking();
        playerInputActions.General.Use.performed += UseItem;
        PlayerInputActions.General.Drop.performed += DropItem;

        PlayerInputActions.General.Sneak.started += context => sneaking = true;
        PlayerInputActions.General.Sneak.canceled += context => sneaking = false;

        #endregion

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        GameManager.MakePausable(this);
    }

    private void Update()
    {
        GetLooking();
        if (Dead)
        {
            forces = Vector3.zero;
            return;
        }
        ComputeVelocity();
        moveDir = forces + velocity + verticalVelocity;
        controller.Move(moveDir * Time.deltaTime);
        forces = Vector3.zero;
    }

    #region Movement

    private void ComputeVelocity()
    {
        Vector2 input = PlayerInputActions.General.Movement.ReadValue<Vector2>();
        input.x *= strafeScale;
        input.y *= forwardScale;
        input.Normalize();

        if (Grounded)
        {
            Accelerate(input, ref velocity);
            HandleSlope(ref velocity);
        }

        if (input == Vector2.zero)
            walked = 0;

        #region Sound

        walked += Vector3.Scale(transform.position - lastPosition, new Vector3(1, 0, 1)).magnitude;
        lastPosition = transform.position;

        float prev = steps;
        steps = (uint)Mathf.RoundToInt(walked / distancePerFootstep);
        
        if (steps > prev && !sneaking && input != Vector2.zero && controller.velocity.magnitude >= 0.2f)
        {
            if (Physics.Raycast(new Ray(transform.position + controller.center, Vector3.down), out RaycastHit floor, controller.height / 2 + slopeCheckExtension, whatIsGround) && floor.collider.CompareTag("Elevator"))
            {
                SoundManager.Instance.PlaySound($"ElevStep{(steps % 3) + 1}");
            }
            else
            {
                SoundManager.Instance.PlaySound($"Footstep{(steps % 4) + 1}");
            }
            GameManager.Instance.enemyController.NoiseHeardNav(transform.position);
        }

        #endregion

        #region Gravity
        if (!Grounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        else
            verticalVelocity.y = -1f;
        #endregion
    }

    private void Accelerate(Vector2 input, ref Vector3 velocity)
    {
        Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;
        Vector3 wishVel = (sneaking ? sneakSpeed : walkSpeed) * speedMultiplier * inputDirection.normalized;

        Vector3 addVel = (sneaking ? sneakAccelStep : accelStep) * speedMultiplier * (wishVel - velocity).normalized;

        if (addVel.magnitude > velocity.magnitude && input == Vector2.zero)
        {
            velocity = Vector3.zero;
        }
        else
            velocity += addVel;

        if (velocity.magnitude > wishVel.magnitude && input != Vector2.zero)
        {
            velocity = velocity.normalized * wishVel.magnitude;
        }
    }

    private void HandleSlope(ref Vector3 velocity)
    {
        if (Physics.Raycast(new Ray(transform.position + controller.center, Vector3.down), out RaycastHit slopeHit, controller.height / 2 + slopeCheckExtension, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < maxSlopeAngle && angle != 0)
            {
                velocity = Vector3.ProjectOnPlane(velocity, slopeHit.normal);
                return;
            }
        }
        velocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
        return;
    }

    #endregion

    #region Interaction

    private void Interact(InputAction.CallbackContext context)
    {
        if (Dead)
            return;

        if (Physics.Raycast(settings.cam.transform.position, settings.cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask))
        {
            if (hit.transform.TryGetComponent(out Item item))
            {
                inventory.PickUpItem(item);
                journal.AddPage(item.itemObj.pickUpPage);
            }
            else if (hit.transform.CompareTag("Door"))
            {
                Door door = hit.transform.GetComponentInParent<Door>();

                if (door == null || door.StageLocked)
                    return;

                if (!door.locked && !door.Open)
                {
                    if (inventory.Items[inventory.ActiveSlot] != null && door.TryLockItem(inventory.Items[inventory.ActiveSlot].itemObj))
                    {
                        lockpick.LockLock(door, settings, inventory, inventory.Items[inventory.ActiveSlot].itemObj);
                        return;
                    }
                }

                if (!door.Toggle())
                {
                    for(int i = 0; i < inventory.Items.Length; i++)
                    {
                        if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && door.Toggle(inventory.Items[i].itemObj))
                        {
                            inventory.UseItem(i);
                            GameManager.Instance.enemyController.NoiseHeardNav(door.middle.position);
                            return;
                        }
                    }
                    lockpick.PickLock(door, settings);
                    LineManager.instance.SayLine("Door locked");
                }
                else
                {
                    GameManager.Instance.enemyController.NoiseHeardNav(door.middle.position);
                }
            }
            else if (hit.transform.CompareTag("ItemHole") && !GameManager.Instance.elevator.Broken)
            {
                for (int i = 0; i < inventory.Items.Length; i++)
                {
                    if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && GameManager.Instance.InsertItem(inventory.Items[i].itemObj))
                    {
                        inventory.UseItem(i);
                        return;
                    }
                }
                if (!GameManager.Instance.elevator.Broken)
                {
                    LineManager.instance.SayLine("Missing Keycard");
                }
            }
            else if (hit.transform.CompareTag("ItemHole") && GameManager.Instance.elevator.Broken)
            {
                for (int i = 0; i < inventory.Items.Length; i++)
                {
                    if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && GameManager.Instance.elevator.Repair(inventory.Items[i].itemObj))
                    {
                        inventory.UseItem(i);
                        return;
                    }
                }
                LineManager.instance.SayLine("Elevator Broken");
            }
        }
    }

    private void GetLooking()
    {
        if (Physics.Raycast(settings.cam.transform.position, settings.cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask))
        {
            if (hit.transform.TryGetComponent(out Item item))
            {
                bar.SetActionText("Pick up item (E)");
                return;
            }
            else if (hit.transform.CompareTag("Door"))
            {
                Door door = hit.transform.GetComponentInParent<Door>();
                if (!door.Open && door.locked)
                {
                    bar.SetActionText("Pick lock (Hold E)");
                }
                else if (!door.Open && !door.locked && inventory.Items[inventory.ActiveSlot] != null 
                    && door.TryLockItem(inventory.Items[inventory.ActiveSlot].itemObj))
                {
                    bar.SetActionText("Lock door (Hold E)");
                }
                else if (!door.Open && !door.locked)
                {
                    bar.SetActionText("Open door (E)");
                }
                else if (door.Open)
                {
                    bar.SetActionText("Close door (E)");
                }
                return;
            }
            else if (hit.transform.CompareTag("ItemHole") && !GameManager.Instance.elevator.Broken)
            {
                bar.SetActionText("Insert keycard (E)");
                return;
            }
            else if (hit.transform.CompareTag("ItemHole") && GameManager.Instance.elevator.Broken)
            {
                bar.SetActionText("Repair elevator (E)");
                return;
            }
        }
        bar.SetActionText("");
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        if (Dead)
            return;

        inventory.DropItem(settings.cam.transform.position, settings.cam.transform.forward);
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if (inventory.Items[inventory.ActiveSlot] == null || inventory.Items[inventory.ActiveSlot].GetType() == typeof(Item))
            return;

        inventory.Items[inventory.ActiveSlot].UseItem(this);
        inventory.UseItem(inventory.ActiveSlot);
    }

    #endregion

    #region Effects

    public void IncreaseSpeed(float multiplier, float time)
    {
        speedMultiplier = multiplier;
        StartCoroutine(WaitAndExec(time, () =>
        {
            speedMultiplier = 1;
        }));
    }

    public void AddForce(Vector3 direction, float forceStrength)
    {
        forces += Vector3.ProjectOnPlane(direction, Vector3.up).normalized * forceStrength;
    }

    #endregion

    public void EnterAnimation(Transform cameraParent)
    {
        PlayerInputActions.Disable();
        cameraController.EnterAnimation(cameraParent);
    }

    public void ExitAnimation(bool callDeath)
    {
        PlayerInputActions.Enable();
        cameraController.ExitAnimation();
        if (callDeath)
        {
            CallDeath();
        }
    }

    void CallDeath()
    {
        if (GameManager.Instance != null && !invincible)
            GameManager.Instance.PlayerDied();
    }

    void Death()
    {
        if (Dead || invincible)
            return;

        Dead = true;
        cameraController.canLook = false;
    }

    public void JumpscareDie()
    {
        Death();

        SoundManager.Instance.PlaySound("Jumpscare");

        //Death called from animation
    }

    public void InsanityDie()
    {
        Death();
        CallDeath();
    }

    public void TrapDie()
    {
        Death();
        //Death called from animation
    }


    public void SpawnFreeze()
    {
        cameraController.ClearBlackScreen(0.1f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, spawnYRot, transform.eulerAngles.z);
        cameraController.canLook = false;
        insanity.Die();
        inventory.Die();
    }

    public void SpawnUnlock()
    {
        Dead = false;
        cameraController.ResetAngle(spawnYRot);
        lastPosition = transform.position;
        walked = 0;
        steps = 0;
        cameraController.canLook = true;
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (((1 << hit.collider.gameObject.layer) & walls.value) <= 0 || forces != Vector3.zero)
            return;

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle > maxSlopeAngle)
        {
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
            forces = Vector3.ProjectOnPlane(forces, hit.normal);
        }
    }
}