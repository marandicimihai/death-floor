using UnityEngine;

public class VFXAnimation : MonoBehaviour
{
    [SerializeField] float blackScreenFadeTime;

    public void BlackScreenAppear()
    {
        GameManager.Instance.player.vfxmanager.BlackScreen(AnimationAction.FadeAppear, blackScreenFadeTime);
    }

    public void BlackScreenDisappear()
    {
        GameManager.Instance.player.vfxmanager.BlackScreen(AnimationAction.FadeDisappear, blackScreenFadeTime);
    }
}
