using UnityEngine.Rendering;
using DeathFloor.SaveSystem;
using UnityEngine;

public enum AnimationAction
{
    FadeAppear,
    FadeDisappear
}

public class VFXManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Volume blackScreen;
    [SerializeField] Volume visualContact;
    [SerializeField] Volume lowInsanity;
    [SerializeField] Volume bloom;
    [SerializeField] Volume blur;
    [SerializeField] new Camera camera;
    [SerializeField] float shakeMagnitude;

    float blackScreenInterpolation;
    float blackScreenTime;
    bool isBlackScreen;

    Vector3 initialPosition;
    float shakeInterpolation;
    float shakeTime;
    bool shaking;

    float visualContactInterpolation;
    float insanityTime;
    bool hasVisualContact;

    float lowInsanityInterpolation;
    float lowInsanityTime;
    bool isLowInsanity;

    private void Start()
    {
        blackScreenTime = 0.001f;
        insanityTime = 0.001f;
        shakeTime = 0.001f;
        lowInsanityTime = 0.001f;

        SaveSystem.OnSettingsChanged += (Settings settings) =>
        {
            bloom.weight = settings.Bloom ? 1 : 0;
            blur.weight = settings.Blur ? 1 : 0;
        };
        initialPosition = camera.transform.localPosition;
    }

    private void Update()
    {
        if (isBlackScreen)
        {
            blackScreenInterpolation += Time.deltaTime / blackScreenTime;
        }
        else
        {
            blackScreenInterpolation -= Time.deltaTime / blackScreenTime;
        }

        blackScreenInterpolation = Mathf.Clamp01(blackScreenInterpolation);
        blackScreen.weight = blackScreenInterpolation;

        if (hasVisualContact)
        {
            visualContactInterpolation += Time.deltaTime / insanityTime;
        }
        else
        {
            visualContactInterpolation -= Time.deltaTime / insanityTime;
        }

        visualContactInterpolation = Mathf.Clamp01(visualContactInterpolation);
        visualContact.weight = visualContactInterpolation;

        if (isLowInsanity)
        {
            lowInsanityInterpolation += Time.deltaTime / lowInsanityTime;
        }
        else
        {
            lowInsanityInterpolation -= Time.deltaTime / lowInsanityTime;
        }

        lowInsanityInterpolation = Mathf.Clamp01(lowInsanityInterpolation);
        lowInsanity.weight = lowInsanityInterpolation;

        if (shaking)
        {
            shakeInterpolation += Time.deltaTime / shakeTime;
            shakeInterpolation = Mathf.Clamp01(shakeInterpolation);
            camera.transform.localPosition = initialPosition + shakeInterpolation * shakeMagnitude * Time.timeScale * Random.insideUnitSphere;
        }
        else if (!shaking && shakeInterpolation != 0)
        {
            shakeInterpolation -= Time.deltaTime / shakeTime;
            shakeInterpolation = Mathf.Clamp01(shakeInterpolation);
            camera.transform.localPosition = initialPosition + shakeInterpolation * shakeMagnitude * Time.timeScale * Random.insideUnitSphere;
        }
    }

    public void CameraShake(AnimationAction action, float fadeTime)
    {
        if (action == AnimationAction.FadeAppear)
        {
            initialPosition = camera.transform.localPosition;
            shaking = true;
        }
        else if (action == AnimationAction.FadeDisappear)
        {
            shaking = false;
        }

        shakeTime = fadeTime;
    }

    public void BlackScreen(AnimationAction action, float time)
    {
        if (action == AnimationAction.FadeAppear)
        {
            isBlackScreen = true;
        }
        else if (action == AnimationAction.FadeDisappear)
        {
            isBlackScreen = false;
        }

        blackScreenTime = time;
    }

    public void VisualContact(AnimationAction action, float time)
    {
        if (action == AnimationAction.FadeAppear)
        {
            hasVisualContact = true;
        }
        else if (action == AnimationAction.FadeDisappear)
        {
            hasVisualContact = false;
        }

        insanityTime = time;
    }

    public void LowInsanity(AnimationAction action, float time)
    {
        if (action == AnimationAction.FadeAppear)
        {
            isLowInsanity = true;
        }
        else if (action == AnimationAction.FadeDisappear)
        {
            isLowInsanity = false;
        }

        lowInsanityTime = time;
    }

    public void ResetEffects()
    {
        isBlackScreen = false;
        hasVisualContact = false;
        isLowInsanity = false;
    }
}
