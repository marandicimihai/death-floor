using UnityEngine;

[RequireComponent(typeof(FpsController))]
public class CameraController : MonoBehaviour
{
    public Transform Camera;
    [SerializeField] Vector2 sensitivities;
    [SerializeField] float jumpscareTurnTime;

    FpsController controller;

    Vector2 rotation;

    Quaternion from;
    Quaternion to;

    float t;

    bool rotating;

    private void Awake()
    {
        controller = GetComponent<FpsController>();
    }

    private void Update()
    {
        if (rotating)
        {
            t += 1 / jumpscareTurnTime * Time.deltaTime;

            t = Mathf.Clamp01(t);

            Vector3 step = Quaternion.Slerp(from, to, t).eulerAngles;

            Camera.localEulerAngles = new Vector3(step.x, 0, 0);
            transform.localEulerAngles = new Vector3(0, step.y, 0);

            if (new Vector3(Camera.localEulerAngles.x, transform.localEulerAngles.y, 0) == Vector3.Scale(to.eulerAngles, new Vector3(1, 1, 0)))
            {
                rotation = new Vector2(Camera.localEulerAngles.x, transform.localEulerAngles.y);
                rotating = false;
            }
            return;
        }

        if (controller.Dead)
            return;

        Vector2 input = controller.PlayerInputActions.General.Look.ReadValue<Vector2>();

        rotation.x -= input.y * sensitivities.y;
        rotation.y += input.x * sensitivities.x;

        rotation.x = Mathf.Clamp(rotation.x, -80, 80);

        Camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.y, 0);
    }

    public void JumpscareTurn(Vector3 enemyPosition)
    {
        to = Quaternion.LookRotation((enemyPosition - Camera.position).normalized);
        from = Quaternion.Euler(new Vector3(Camera.localEulerAngles.x, transform.localEulerAngles.y, 0));
        rotating = true;
        t = 0;
    }
}