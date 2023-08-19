using UnityEngine;

public class VFXAnimation : MonoBehaviour
{
    [SerializeField] float blackScreenFadeTime;

    VFXManager vfxmanager;

    private void Start()
    {
        vfxmanager = FindObjectOfType<VFXManager>();
    }

    public void BlackScreenAppear()
    {
        if (vfxmanager == null)
        {
            Debug.Log("No vfx manager.");
            return;
        }

        vfxmanager.BlackScreen(AnimationAction.FadeAppear, blackScreenFadeTime);
    }

    public void BlackScreenDisappear()
    {
        if (vfxmanager == null)
        {
            Debug.Log("No vfx manager.");
            return;
        }

        vfxmanager.BlackScreen(AnimationAction.FadeDisappear, blackScreenFadeTime);
    }
}
