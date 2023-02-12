using UnityEngine;

[RequireComponent(typeof(FpsController))]
public class CameraController : MonoBehaviour
{
    [SerializeField] new Transform camera;
    [SerializeField] Vector2 sensitivities;

    FpsController controller;

    Vector2 rotation;

    private void Awake()
    {
        controller = GetComponent<FpsController>();
    }

    private void Update()
    {
        Vector2 input = controller.PlayerInputActions.General.Look.ReadValue<Vector2>();

        rotation.x -= input.y * sensitivities.y;
        rotation.y += input.x * sensitivities.x;

        rotation.x = Mathf.Clamp(rotation.x, -80, 80);

        camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.y, 0);
    }
}