using UnityEngine;

public class Insanity : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyNavigation enemy;
    [SerializeField] float insanityTime;
    [SerializeField] float insanityEffectFadeTime;

    [Header("Animation")]
    [SerializeField] Animator sanityDeath;

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
            player.vfxmanager.Insanity(AnimationAction.FadeAppear, insanityEffectFadeTime);
            insanity += Time.deltaTime / insanityTime;
        }
        else
        {
            player.vfxmanager.Insanity(AnimationAction.FadeDisappear, insanityEffectFadeTime);
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
