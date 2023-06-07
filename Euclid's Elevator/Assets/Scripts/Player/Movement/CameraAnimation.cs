using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Player player;
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
        positionOnEnter = defaultPosition;

        defaultRotation = camera.transform.localRotation;
        rotationOnEnter = defaultRotation;
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
        if (inAnimation)
            return;

        player.inventoryHUD.hideHUD = hideHUD;

        player.controller.Disable();
        player.cameraController.Disable();

        defaultRotation = camera.transform.localRotation;
        defaultPosition = camera.transform.localPosition;

        camera.transform.parent = reference;

        positionOnEnter = camera.transform.localPosition;
        rotationOnEnter = camera.transform.localRotation;
        
        inAnimation = true;
    }

    public void ExitAnimation(bool instant = false)
    {
        if (!inAnimation)
            return;

        player.inventoryHUD.hideHUD = false;

        player.controller.Enable();
        player.cameraController.Enable();

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
