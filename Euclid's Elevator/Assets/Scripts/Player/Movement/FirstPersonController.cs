using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    #region Private Serialized

    [Header("Input Movement Speed")]
    [SerializeField] float forwardScale;
    [SerializeField] float strafeScale;
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float walkSpeed;
    [SerializeField] float walkAccel;
    [SerializeField] float sneakSpeed;
    [SerializeField] float sneakAccel;

    [Header("Wall and ground check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask walls;
    [SerializeField] LayerMask ground;
    [SerializeField] float checkRadius;
    [SerializeField] float checkExtension;

    [Header("Gravity")]
    [SerializeField] float gravityForce;

    #endregion

    #region Private

    CharacterController controller;

    Vector3 moveDirection, forces, velocity, gravity;

    float speed, speedMultiplier;
    float accel;

    bool canMove, sneaking;
    bool Grounded
    {
        get
        {
            return Physics.CheckSphere(groundCheck.position, checkRadius, ground);
        }
    }

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        Input.InputActions.General.Sneak.started += (InputAction.CallbackContext context) => sneaking = true;
        Input.InputActions.General.Sneak.canceled += (InputAction.CallbackContext context) => sneaking = false;

        speedMultiplier = 1;
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            ComputeVelocity();
            moveDirection = forces + velocity;
            controller.Move(moveDirection * Time.deltaTime);
        }
        ComputeGravity();
        controller.Move(gravity * Time.deltaTime);
        forces = Vector3.zero;
    }

    private void ComputeGravity()
    {
        if (Grounded)
        {
            gravity.y = -1f;
        }
        else
        {
            gravity.y += gravityForce * Time.deltaTime;
        }
    }

    private void ComputeVelocity()
    {
        Vector2 inputVec = Input.InputActions.General.Movement.ReadValue<Vector2>();
        inputVec.x *= strafeScale;
        inputVec.y *= forwardScale;
        inputVec.Normalize();

        if (sneaking)
        {
            speed = sneakSpeed;
            accel = sneakAccel;
        }
        else
        {
            speed = walkSpeed;
            accel = walkAccel;
        }

        if (Grounded)
        {
            Accelerate(inputVec, ref velocity);
            HandleSlope(ref velocity);
        }
    }

    /// <summary>
    /// Smoothly interpolates from current velocity to desired velocity
    /// </summary>
    /// <param name="input"></param>
    /// <param name="velocity"></param>
    private void Accelerate(Vector2 input, ref Vector3 velocity)
    {
        Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;
        Vector3 wishVel = speed * speedMultiplier * inputDirection.normalized;

        Vector3 addVel = accel * speedMultiplier * (wishVel - velocity).normalized;

        //prevent velocity from having oscillating values when stopping
        if (addVel.magnitude > velocity.magnitude && input == Vector2.zero)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity += addVel;
        }

        //cap velocity to wish velocity length
        if (velocity.magnitude > wishVel.magnitude && input != Vector2.zero)
        {
            velocity = velocity.normalized * wishVel.magnitude;
        }
    }

    /// <summary>
    /// Projects velocity on the ground plane
    /// </summary>
    /// <param name="velocity"></param>
    private void HandleSlope(ref Vector3 velocity)
    {
        if (Physics.Raycast(new Ray(transform.position + controller.center, Vector3.down), out RaycastHit slopeHit, controller.height / 2 + checkExtension, ground))
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

    /// <summary>
    /// Projects the velocity on the walls to prevent clipping
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //check if the hit is a wall and prevent wall bumping when a force is applied
        if (((1 << hit.collider.gameObject.layer) & walls.value) <= 0 || forces != Vector3.zero)
            return;

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle > maxSlopeAngle)
        {
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
            forces = Vector3.ProjectOnPlane(forces, hit.normal);
        }
    }

    /// <summary>
    /// Adds a force to the player, must be applied every frame.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="forceStrength"></param>
    public void AddForce(Vector3 direction, float forceStrength)
    {
        forces += Vector3.ProjectOnPlane(direction, Vector3.up).normalized * forceStrength;
    }
}
