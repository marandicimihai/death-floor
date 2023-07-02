using UnityEngine;

public class Insanity : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyNavigation enemy;
    [SerializeField] float insanityTime;
    [SerializeField] float insanityEffectFadeTime;

    [Header("Very Low Sanity")]
    [SerializeField] float lowInsanitySoundAppearPercentage;
    [SerializeField] string lowInsanitySound;
    [Header("Low Sanity")]
    [SerializeField] float lowInsanityEffectAppearPercentage;
    [SerializeField] float lowInsanityEffectFadeTime;

    [Header("Animation")]
    [SerializeField] Animator sanityDeath;

    AudioJob verylowsanity;
    float insanity;

    private void Start()
    {
        GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) => insanity = 0;
        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) => insanity = 0;
    }

    private void Update()
    {
        if (enemy.Visible)
        {
            player.vfxmanager.VisualContact(AnimationAction.FadeAppear, insanityEffectFadeTime);
            insanity += Time.deltaTime / insanityTime;
        }
        else
        {
            player.vfxmanager.VisualContact(AnimationAction.FadeDisappear, insanityEffectFadeTime);
        }
        if (insanity >= 1 - lowInsanityEffectAppearPercentage)
        {
            player.vfxmanager.LowInsanity(AnimationAction.FadeAppear, lowInsanityEffectFadeTime);
        }
        else
        {
            player.vfxmanager.LowInsanity(AnimationAction.FadeDisappear, lowInsanityEffectFadeTime);
        }
        if (insanity >= 1 - lowInsanitySoundAppearPercentage && verylowsanity == null)
        {
            verylowsanity = AudioManager.Instance.PlayClip(lowInsanitySound);
        }
        else if (insanity < 1 - lowInsanitySoundAppearPercentage)
        {
            AudioManager.Instance.StopClip(verylowsanity);
        }
        if (insanity >= 1)
        {
            player.Die(false);
            sanityDeath.SetBool("Dead", true);
            Invoke(nameof(ResetAnimation), 1);
        }
    }

    public void ReduceSanity(float delta)
    {
        insanity -= delta;
        insanity = Mathf.Clamp01(insanity);
    }

    void ResetAnimation()
    {
        sanityDeath.SetBool("Dead", false);
    }
}
