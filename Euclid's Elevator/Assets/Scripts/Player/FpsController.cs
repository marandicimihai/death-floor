using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FpsController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float forwardScale;
    [SerializeField] float strafeScale;
    [SerializeField] float walkSpeed;
    [SerializeField] float accelStep;

    [Header("Gravity")]
    [SerializeField] float gravity;

    [Header("Ground Check")]
    [Range(0.1f, 10)] [SerializeField] float groundCheckExtension;
    public LayerMask whatIsGround;

    [Header("Slope Handling")]
    [SerializeField] float slopeCheckExtension;
    [SerializeField] float maxSlopeAngle;

    [Header("Other Settings")]
    [SerializeField] int interactionDistance;


    public PlayerInputActions PlayerInputActions { get { return playerInputActions; } }

    [System.NonSerialized] public static FpsController singleton;

    PlayerInputActions playerInputActions;
    CharacterController controller;

    Vector3 verticalVelocity;
    Vector3 velocity;

    bool grounded;

    private void OnEnable()
    {
        controller = GetComponent<CharacterController>();

        #region Input

        playerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();

        #endregion

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
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
        Vector3 wishVel = inputDirection.normalized * walkSpeed;

        Vector3 addVel = (wishVel - velocity).normalized * accelStep * Time.deltaTime;

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
        
    }
}