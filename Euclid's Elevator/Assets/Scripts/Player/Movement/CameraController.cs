using UnityEngine;

public class CameraController : MonoBehaviour
{
    public new Transform camera;
    [SerializeField] float sensitivity;

    Vector2 rotation;
    bool canLook;

    private void Awake()
    {
        ResetAngle();
        Enable();
    }

    private void Update()
    {
        if (canLook)
        {
            Vector2 inputVec = Input.InputActions.General.Look.ReadValue<Vector2>() * Time.timeScale;

            rotation.x -= inputVec.y * sensitivity;
            rotation.y += inputVec.x * sensitivity;

            rotation.x = Mathf.Clamp(rotation.x, -90, 90);

            camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
            transform.localEulerAngles = new Vector3(0, rotation.y, 0);
        }    
    }
    
    public void ResetAngle()
    {
        camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.y, 0);
        rotation.x = 0;
        rotation.y = 90;
    }

    public void Spawn(float freezeTime)
    {
        canLook = false;
        ResetAngle();

        Invoke(nameof(Enable), freezeTime);
    }

    public void Disable()
    {
        canLook = false;
    }

    public void Enable()
    {
        canLook = true;
    }
}