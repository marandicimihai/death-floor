using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] new Transform camera;

    float sensitivity;

    Vector2 rotation;
    bool canLook = true;

    private void Start()
    {
        sensitivity = 0.1f * SaveSystem.LoadSettings().Sensitivity;

        SaveSystem.OnSettingsChanged += (Settings settings) =>
        {
            sensitivity = 0.1f * settings.Sensitivity;
        };

        if (SaveSystem.CurrentSaveData != null && SaveSystem.CurrentSaveData.CameraRotation.Length != 0)
        {
            rotation = new Vector2(SaveSystem.CurrentSaveData.CameraRotation[0],
                                   SaveSystem.CurrentSaveData.CameraRotation[1]);

        }
        else
        {
            ResetAngle();
        }
        SaveSystem.OnSaveGame += (ref GameData data) =>
        {
            data.CameraRotation = new float[]
            {
                    rotation.x,
                    rotation.y
            };
        };
    }

    private void Update()
    {
        if (canLook)
        {
            Vector2 inputVec;
            inputVec = Input.Instance.InputActions.General.Look.ReadValue<Vector2>() * Time.timeScale;

            rotation.x -= inputVec.y * sensitivity;
            rotation.y += inputVec.x * sensitivity;

            rotation.x = Mathf.Clamp(rotation.x, -90, 90);

            camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
            transform.localEulerAngles = new Vector3(0, rotation.y, 0);
        }
    }

    public void ResetAngle()
    {
        rotation.x = 0;
        rotation.y = 90;
        camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.y, 0);
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