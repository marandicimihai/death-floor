using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCameraAnimation : MonoBehaviour
{
    [SerializeField] Transform cameraReference;
    [SerializeField] bool hideHUD;
    [SerializeField] bool instantExit;

    CameraAnimation cameraAnimation;

    private void Start()
    {
        cameraAnimation = FindObjectOfType<CameraAnimation>();
    }
    public void EnterAnimation()
    {
        if (cameraAnimation == null)
        {
            Debug.Log("No camera animation.");
            return;
        }

        cameraAnimation.EnterAnimation(cameraReference, hideHUD);
    }

    public void ExitAnimation()
    {
        if (cameraAnimation == null)
        {
            Debug.Log("No camera animation.");
            return;
        }

        cameraAnimation.ExitAnimation(instantExit);
    }
}
