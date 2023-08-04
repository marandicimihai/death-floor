using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PlayerHUDManager hudManager;
    [SerializeField] new Camera camera;
    [SerializeField] Transform defaultCameraParent;
    [SerializeField] float animationTransitionTime;

    Vector3 positionOnEnter;
    Quaternion rotationOnEnter;
    
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    bool inAnimation;
    float interpolation;

    private void Awake()
    {
        defaultPosition = camera.transform.localPosition;
        defaultRotation = camera.transform.localRotation;

        positionOnEnter = defaultPosition;
        rotationOnEnter = defaultRotation;
    }

    private void Start()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                SaveSystem.Instance.CanSave = !inAnimation;
            };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }

    private void Update()
    {
        interpolation = Mathf.Clamp01(interpolation);

        if (inAnimation)
        {
            interpolation += Time.deltaTime / animationTransitionTime;
            camera.transform.localPosition = Vector3.Lerp(positionOnEnter, Vector3.zero, interpolation);
            camera.transform.localRotation = Quaternion.Lerp(rotationOnEnter, Quaternion.identity, interpolation);
        }
        else if (!inAnimation && interpolation != 0)
        {
            interpolation -= Time.deltaTime / animationTransitionTime;
            camera.transform.localPosition = Vector3.Lerp(defaultPosition, positionOnEnter, interpolation);
            camera.transform.localRotation = Quaternion.Lerp(defaultRotation, rotationOnEnter, interpolation);
        }
    }

    public void EnterAnimation(Transform reference, bool hideHUD = false)
    {
        if (!inAnimation)
        {
            defaultPosition = camera.transform.localPosition;
            defaultRotation = camera.transform.localRotation;
        }

        if (hideHUD)
        {
            if (hudManager != null)
            {
                hudManager.HideAllHUD();
            }
            else
            {
                Debug.Log("No hud manager.");
            }
        }

        if (player != null)
        {
            player.UnFreeze();
        }
        else
        {
            Debug.Log("No player class.");
        }

        if (Input.InputActions != null)
        {
            Input.InputActions.Disable();
        }
        else
        {
            Debug.Log("Input class absent.");
        }

        camera.transform.parent = reference;

        positionOnEnter = camera.transform.localPosition;
        rotationOnEnter = camera.transform.localRotation;

        interpolation = 0;
        inAnimation = true;
    }

    public void ExitAnimation(bool instant = false)
    {
        if (!inAnimation)
            return;

        if (hudManager != null)
        {
            hudManager.DefaultHUD();
        }
        else
        {
            Debug.Log("No hud manager.");
        }

        if (player != null)
        {
            player.Freeze();
        }
        else
        {
            Debug.Log("No player class.");
        }

        if (Input.InputActions != null)
        {
            Input.InputActions.Enable();
        }
        else
        {
            Debug.Log("Input class absent.");
        }

        camera.transform.parent = defaultCameraParent;

        if (instant)
        {
            interpolation = 0;
            camera.transform.localPosition = defaultPosition;
            camera.transform.localRotation = defaultRotation;
        }

        positionOnEnter = camera.transform.localPosition;
        rotationOnEnter = camera.transform.localRotation;

        inAnimation = false;
    }
}
