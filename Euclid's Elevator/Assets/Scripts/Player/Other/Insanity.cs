using DeathFloor.SaveSystem;
using UnityEngine;

public class Insanity : MonoBehaviour, ISaveData<InsanityData>
{
    public float InsanityValue { get => insanity; }

    public bool CanSave => true;

    [SerializeField] Player player;
    [SerializeField] VFXManager vfx;
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

    EnemyNavigation enemy;
    AudioJob verylowsanity;
    float insanity;

    private void Start()
    {
        enemy = FindObjectOfType<EnemyNavigation>();
    }

    public void OnFirstTimeLoaded()
    {

    }

    public InsanityData OnSaveData()
    {
        return new InsanityData(insanity);
    }

    public void LoadData(InsanityData data)
    {
        insanity = data.Insanity;
    }

    private void Update()
    {
        if (enemy != null)
        {
            if (enemy.Visible)
            {
                if (vfx != null)
                {
                    vfx.VisualContact(AnimationAction.FadeAppear, insanityEffectFadeTime);
                }
                else
                {
                    Debug.Log("No vfx class.");
                }
                insanity += Time.deltaTime / insanityTime;
            }
            else
            {
                if (vfx != null)
                {
                    vfx.VisualContact(AnimationAction.FadeDisappear, insanityEffectFadeTime);
                }
                else
                {
                    Debug.Log("No vfx class.");
                }
            }
        }
        else
        {
            Debug.Log("No enemy!");
        }

        #region Effects

        if (insanity >= lowInsanityEffectAppearPercentage)
        {
            if (vfx != null)
            {
                vfx.LowInsanity(AnimationAction.FadeAppear, lowInsanityEffectFadeTime);
            }
            else
            {
                Debug.Log("No vfx class.");
            }
        }
        else
        {
            if (vfx != null)
            {
                vfx.LowInsanity(AnimationAction.FadeDisappear, lowInsanityEffectFadeTime);
            }
            else
            {
                Debug.Log("No vfx class.");
            };
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

        if (player != null)
        {
            if (insanity >= 1 && !player.Dead)
            {
                player.Die(false);
                sanityDeath.SetBool("Dead", true);
                Invoke(nameof(ResetAnimation), 1);
            }
        }
        else
        {
            Debug.Log("No player class.");
        }
    }

    /// <summary>
    /// Use 1 to reset it.
    /// </summary>
    /// <param name="delta"></param>
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