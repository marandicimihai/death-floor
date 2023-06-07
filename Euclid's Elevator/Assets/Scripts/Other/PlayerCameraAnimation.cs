using UnityEngine;

public class PlayerCameraAnimation : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Transform cameraReference;
    [SerializeField] bool hideHUD;
    [SerializeField] bool instantExit;

    public void EnterAnimation()
    {
        player.cameraAnimation.EnterAnimation(cameraReference, hideHUD);
    }

    public void ExitAnimation()
    {
        player.cameraAnimation.ExitAnimation(instantExit);
    }
}
