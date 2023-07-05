using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCameraAnimation : MonoBehaviour
{
    [SerializeField] Transform cameraReference;
    [SerializeField] bool hideHUD;
    [SerializeField] bool instantExit;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EnterAnimation()
    {
        GameManager.Instance.player.cameraAnimation.EnterAnimation(cameraReference, hideHUD);
    }

    public void ExitAnimation()
    {
        GameManager.Instance.player.cameraAnimation.ExitAnimation(instantExit);
    }
}
