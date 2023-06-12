using UnityEngine;

public class PlayerCameraAnimation : MonoBehaviour
{
    [SerializeField] Transform cameraReference;
    [SerializeField] bool hideHUD;
    [SerializeField] bool instantExit;

    public void EnterAnimation()
    {
        GameManager.Instance.player.cameraAnimation.EnterAnimation(cameraReference, hideHUD);
    }

    public void ExitAnimation()
    {
        GameManager.Instance.player.cameraAnimation.ExitAnimation(instantExit);
    }
}
