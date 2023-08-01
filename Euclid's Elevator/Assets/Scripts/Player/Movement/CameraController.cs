using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] new Transform camera;
    
    float sensitivity;

    Vector2 rotation;
    bool canLook = true;

    private void Start()
    {
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.LoadSettings() != null)
            {
                sensitivity = 0.1f * SaveSystem.Instance.LoadSettings().Sensitivity;
            }
            else
            {
                sensitivity = 0.1f * 0.1f;
            }
            SaveSystem.Instance.OnSettingsChanged += (Settings settings) =>
            {
                sensitivity = 0.1f * settings.Sensitivity;
            };

            if (SaveSystem.Instance.currentSaveData != null && SaveSystem.Instance.currentSaveData.CameraRotation.Length != 0)
            {
                rotation = new Vector2(SaveSystem.Instance.currentSaveData.CameraRotation[0],
                                       SaveSystem.Instance.currentSaveData.CameraRotation[1]);

            }
            else
            {
                ResetAngle();
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                data.CameraRotation = new float[]
                {
                    rotation.x,
                    rotation.y
                };
            };
        }
        else
        {
            Debug.Log("No save system.");
            sensitivity = 0.1f * 0.1f;
            ResetAngle();
        }
    }

    private void Update()
    {
        if (canLook)
        {
            Vector2 inputVec;
            if (Input.InputActions != null)
            {
               inputVec = Input.InputActions.General.Look.ReadValue<Vector2>() * Time.timeScale;
            }
            else
            {
                Debug.Log("Input class absent.");
                return;
            }

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