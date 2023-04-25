using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public void EnterAnimation(Transform cameraParent)
    {
        GameManager.Instance.playerController.EnterAnimation(cameraParent);
    }

    public void ExitAnimation()
    {
        GameManager.Instance.playerController.ExitAnimation();
    }
}