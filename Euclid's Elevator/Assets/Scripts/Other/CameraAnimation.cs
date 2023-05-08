using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Transform cameraParent;
    [SerializeField] bool callDeath;

    public void EnterAnimation()
    {
        GameManager.Instance.playerController.EnterAnimation(cameraParent);
    }

    public void ExitAnimation()
    {
        GameManager.Instance.playerController.ExitAnimation(callDeath);
    }

    public void BlackScreen(float value)
    {
        GameManager.Instance.playerController.cameraController.BlackScreen(value);
    }
}