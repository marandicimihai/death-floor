using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Inventory)), RequireComponent(typeof(CameraController))]
public class FpsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CameraController cameraController;
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
    [SerializeField] float distancePerFootstep;
    [SerializeField] AudioSource footstepSource;
    [Header("")]
    [SerializeField] AudioSource jumpScareSource;
    [Header("")]
    [SerializeField] LayerMask interactionMask;
    [SerializeField] int interactionDistance;
    [Header("")]
    [SerializeField] float spawnYRot;


    public PlayerInputActions PlayerInputActions { get { return playerInputActions; } }
    public bool Paralized { get; private set; }

    PlayerInputActions playerInputActions;
    Camera cam;

    Vector3 verticalVelocity;
    Vector3 velocity;

    float walked;
    uint steps;

    bool grounded;
    bool sneaking;

    private void Awake()
    {
        cam = cameraController.Camera.GetComponent<Camera>();

        #region Input

        playerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();

        PlayerInputActions.General.Interact.performed += Interact;
        PlayerInputActions.General.Drop.performed += DropItem;

        PlayerInputActions.General.Sneak.started += (InputAction.CallbackContext context) => { sneaking = true; };
        PlayerInputActions.General.Sneak.canceled += (InputAction.CallbackContext context) => { sneaking = false; };

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

        if (steps > prev && !sneaking && input != Vector2.zero && controller.velocity.magnitude >= 0.1f)
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
        Vector3 wishVel = inputDirection.normalized * (sneaking ? sneakSpeed : walkSpeed);

        Vector3 addVel = (sneaking ? sneakAccelStep : accelStep) * Time.deltaTime * (wishVel - velocity).normalized;

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

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance, interactionMask))
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

                else if (!door.Toggle())
                {
                    bool gate = false;
                    for(int i = 0; i < inventory.Items.Length - 1; i++)
                    {
                        if (door.Toggle(inventory.Items[i].itemObj))
                        {
                            inventory.UseItem(i);
                            gate = true;
                            break;
                        }
                    }
                    if (!gate)
                        LineManager.instance.SayLine("Door locked");
                }
            }
        }
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        if (Paralized)
            return;

        inventory.DropItem(cam.transform.position, cam.transform.forward);
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

        if (inventory != null)
            inventory.Die();

        if (GameManager.instance != null)
            GameManager.instance.Die();
    }

    public void Lock()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, spawnYRot, transform.eulerAngles.z);
        Paralized = true;
    }

    public void Unlock()
    {
        cameraController.ResetAngle(spawnYRot);
        Paralized = false;
    }
}