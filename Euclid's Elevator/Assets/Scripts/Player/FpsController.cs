using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
public struct InteractionSettings
{
    public Camera cam;
    public LayerMask interactionMask;
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
    [SerializeField] Insanity insanity;
    [SerializeField] CameraController cameraController;
    [SerializeField] CharacterController controller;
    [SerializeField] Inventory inventory;
    [SerializeField] Lockpick lockpick;

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
    [SerializeField] AudioSource footstepSource;
    [Header("")]
    [SerializeField] AudioSource jumpScareSource;
    [Header("")]
    [SerializeField] InteractionSettings settings;
    [Header("")]
    [SerializeField] float spawnYRot;


    public PlayerInputActions PlayerInputActions { get { return playerInputActions; } }
    public bool Paralized { get; private set; }

    PlayerInputActions playerInputActions;
    Camera cam;

    Vector3 verticalVelocity;
    Vector3 velocity;

    float speedMultiplier;
    float walked;
    uint steps;

    bool grounded;
    bool sneaking;

    private void Awake()
    {
        cam = cameraController.Camera.GetComponent<Camera>();
        settings.cam = cam;

        speedMultiplier = 1;

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
        if (Paralized)
            return;

        Move();
    }

    private void Move()
    {
        grounded = Physics.CheckSphere(transform.position + controller.center - new Vector3(0, controller.height / 2 - controller.radius + groundCheckExtension, 0), controller.radius - 0.1f, whatIsGround);
        
        Vector2 input = PlayerInputActions.General.Movement.ReadValue<Vector2>();
        input.x *= strafeScale;
        input.y *= forwardScale;
        input.Normalize();

        if (grounded)
            Accelerate(input, ref velocity);

        HandleSlope(ref velocity);

        controller.Move(velocity * Time.deltaTime);

        walked += (velocity * Time.deltaTime).magnitude;

        float prev = steps;
        steps = (uint)Mathf.RoundToInt(walked / distancePerFootstep);
        
        if (steps > prev && !sneaking && input != Vector2.zero && controller.velocity.magnitude >= 0.2f)
        {
            footstepSource.Play();
            if(GameManager.instance.enemy.TryGetComponent(out Enemy enemy))
            {
                enemy.NoiseHeardNav(transform.position);
            }
        }

        #region Gravity
        if (!grounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        else
            verticalVelocity.y = -1f;

        controller.Move(verticalVelocity * Time.deltaTime);
        #endregion
    }

    private void Accelerate(Vector2 input, ref Vector3 velocity)
    {
        Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;
        Vector3 wishVel = (sneaking ? sneakSpeed : walkSpeed) * speedMultiplier * inputDirection.normalized;

        Vector3 addVel = (sneaking ? sneakAccelStep : accelStep) * speedMultiplier * Time.deltaTime * (wishVel - velocity).normalized;

        if (addVel.magnitude > velocity.magnitude && input == Vector2.zero)
        {
            velocity = Vector3.zero;
        }
        else if ((velocity + addVel).magnitude > wishVel.magnitude && input != Vector2.zero)
        {
            velocity = wishVel;
        }
        else
            velocity += addVel;
    }

    private void HandleSlope(ref Vector3 direction)
    {
        if (Physics.Raycast(new Ray(transform.position + controller.center, Vector3.down), out RaycastHit slopeHit, controller.height / 2 + slopeCheckExtension, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < maxSlopeAngle && angle != 0)
            {
                direction = Vector3.ProjectOnPlane(direction, slopeHit.normal);
                return;
            }
        }
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        return;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (Paralized)
            return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask))
        {
            if (hit.transform.TryGetComponent(out Item item))
            {
                inventory.PickUpItem(item);
            }
            else if (hit.transform.CompareTag("Door"))
            {
                Door door = hit.transform.GetComponentInParent<Door>();

                if (door == null)
                    return;

                if (!door.Toggle())
                {
                    bool gate = false;
                    for(int i = 0; i < inventory.Items.Length - 1; i++)
                    {
                        if (inventory.Items[i] != null && inventory.Items[i].itemObj != null && door.Toggle(inventory.Items[i].itemObj))
                        {
                            inventory.UseItem(i);
                            gate = true;
                            break;
                        }
                    }
                    if (!gate)
                        LineManager.instance.SayLine("Door locked");
                    else
                        return;
                }

                lockpick.PickLock(door, settings);
            }
        }
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        if (Paralized)
            return;

        inventory.DropItem(cam.transform.position, cam.transform.forward);
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if (inventory.Items[inventory.ActiveSlot] == null)
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

    public void Die(Vector3? enemyPosition = null)
    {
        if (Paralized)
            return;

        Paralized = true;

        velocity = Vector3.zero;

        if (enemyPosition != null)
        {
            jumpScareSource.Play();
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
        Paralized = true;
    }

    public void SpawnUnlock()
    {
        cameraController.ResetAngle(spawnYRot);
        Paralized = false;
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
        if (((1 << hit.collider.gameObject.layer) & walls.value) <= 0)
            return;

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle > maxSlopeAngle)
        {
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
    }
}