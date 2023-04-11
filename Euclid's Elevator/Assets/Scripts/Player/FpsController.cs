using UnityEngine.InputSystem;
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
    [Header("Components")]
    public Insanity insanity;
    public CameraController cameraController;
    public Lockpick lockpick;
    [SerializeField] CharacterController controller;
    [SerializeField] Inventory inventory;

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
    public bool Freezed { get; private set; }

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
        playerInputActions.General.Use.performed += UseItem;
        PlayerInputActions.General.Drop.performed += DropItem;

        PlayerInputActions.General.Sneak.started += context => sneaking = true;
        PlayerInputActions.General.Sneak.canceled += context => sneaking = false;

        #endregion

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Freezed)
        {
            forces = Vector3.zero;
            return;
        }
        ComputeVelocity();
        moveDir = forces + velocity + verticalVelocity;
        controller.Move(moveDir * Time.deltaTime);
        forces = Vector3.zero;
    }

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
            SoundManager.instance.PlaySound($"Footstep{(steps % 4) + 1}");;
            GameManager.instance.enemyController.NoiseHeardNav(transform.position);
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

    private void Interact(InputAction.CallbackContext context)
    {
        if (Dead || Freezed)
            return;

        if (Physics.Raycast(settings.cam.transform.position, settings.cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask))
        {
            if (hit.transform.TryGetComponent(out Item item))
            {
                inventory.PickUpItem(item);
            }
            else if (hit.transform.CompareTag("Door"))
            {
                Door door = hit.transform.GetComponentInParent<Door>();

                if (door == null || door.StageLocked)
                    return;

                if (!door.Toggle())
                {
                    for(int i = 0; i < inventory.Items.Length - 1; i++)
                    {
                        if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && door.Toggle(inventory.Items[i].itemObj))
                        {
                            inventory.UseItem(i);
                            GameManager.instance.enemyController.NoiseHeardNav(door.middle.position);
                            return;
                        }
                    }
                    LineManager.instance.SayLine("Door locked");
                }
                else
                {
                    GameManager.instance.enemyController.NoiseHeardNav(door.middle.position);
                }

                lockpick.PickLock(door, settings);
            }
            else if (hit.transform.CompareTag("ItemHole"))
            {
                for (int i = 0; i < inventory.Items.Length - 1; i++)
                {
                    if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && GameManager.instance.InsertItem(inventory.Items[i].itemObj))
                    {
                        inventory.UseItem(i);
                        return;
                    }
                }
                if (!GameManager.instance.elevator.Broken)
                {
                    LineManager.instance.SayLine("Missing Keycard");
                }
            }
        }
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        if (Dead || Freezed)
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
        forces += forceStrength * Vector3.Scale(direction, new Vector3(1, 0, 1)).normalized;
    }

    public void Die(Vector3? enemyPosition = null)
    {
        if (Dead)
            return;

        Dead = true;
        Freezed = true;
        cameraController.canLook = false;

        velocity = Vector3.zero;

        if (enemyPosition != null)
        {
            SoundManager.instance.PlaySound("Jumpscare");
            cameraController.JumpscareTurn((Vector3)enemyPosition);
        }

        insanity.Die();
        inventory.Die();

        if (GameManager.instance != null)
            GameManager.instance.Die();
    }

    public void SpawnFreeze()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, spawnYRot, transform.eulerAngles.z);
        Dead = false;
        Freezed = true;
        cameraController.canLook = false;
    }

    public void SpawnUnlock()
    {
        cameraController.ResetAngle(spawnYRot);
        lastPosition = transform.position;
        walked = 0;
        steps = 0;
        Freezed = false;
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