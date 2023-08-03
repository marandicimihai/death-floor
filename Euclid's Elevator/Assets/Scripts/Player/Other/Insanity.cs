using UnityEngine;

public class Insanity : MonoBehaviour
{
    public delegate void InsanityDel(InsanityArgs args);
    public InsanityDel OnInsanityChanged;

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) => insanity = 0;
            GameManager.Instance.OnDeath += (object caller, System.EventArgs args) => insanity = 0;
        }
        else
        {
            Debug.Log("No game manager.");
        }

        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.currentSaveData != null)
            {
                insanity = SaveSystem.Instance.currentSaveData.insanity;
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                data.insanity = insanity;
            };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }

    private void Update()
    {
        float insanityold = insanity;
        if (enemy != null)
        {
            if (enemy.Visible)
            {
                player.VisualContact(AnimationAction.FadeAppear, insanityEffectFadeTime);
                insanity += Time.deltaTime / insanityTime;
            }
            else
            {
                player.VisualContact(AnimationAction.FadeDisappear, insanityEffectFadeTime);
            }
        }
        else
        {
            Debug.Log("No enemy!");
        }

        #region Effects

        if (insanity >= lowInsanityEffectAppearPercentage)
        {
            player.LowInsanity(AnimationAction.FadeAppear, lowInsanityEffectFadeTime);
        }
        else
        {
            player.LowInsanity(AnimationAction.FadeDisappear, lowInsanityEffectFadeTime);
        }
        if (insanity >= lowInsanitySoundAppearPercentage && verylowsanity == null)
        {
            verylowsanity = AudioManager.Instance.PlayClip(lowInsanitySound);
        }
        else if (insanity < lowInsanitySoundAppearPercentage)
        {
            AudioManager.Instance.StopClip(verylowsanity);
        }

        #endregion

        if (insanity >= 1 && !player.Dead)
        {
            player.Die(false);
            sanityDeath.SetBool("Dead", true);
            Invoke(nameof(ResetAnimation), 1);
        }

        if (insanity != insanityold)
        {
            OnInsanityChanged?.Invoke(new InsanityArgs(insanity));
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

public class InsanityArgs : System.EventArgs
{
    public float insanity;

    public InsanityArgs(float insanity)
    {
        this.insanity = insanity;
    }
}