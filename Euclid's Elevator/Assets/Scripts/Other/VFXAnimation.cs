using UnityEngine;

public class VFXAnimation : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float blackScreenFadeTime;

    public void BlackScreenAppear()
    {
        player.vfxmanager.BlackScreen(AnimationAction.FadeAppear, blackScreenFadeTime);
    }

    public void BlackScreenDisappear()
    {
        player.vfxmanager.BlackScreen(AnimationAction.FadeDisappear, blackScreenFadeTime);
    }
}
