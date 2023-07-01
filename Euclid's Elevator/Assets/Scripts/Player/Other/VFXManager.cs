using System.Collections;
using UnityEngine.Rendering;
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
    [SerializeField] Volume insanity;
    [SerializeField] new Camera camera;
    [SerializeField] float shakeMagnitude;

    float blackScreenInterpolation;
    float blackScreenTime;
    bool isBlackScreen;

    Vector3 initialPosition;
    float shakeInterpolation;
    float shakeTime;
    bool shaking;

    float insanityInterpolation;
    float insanityTime;
    bool insane;

    private void Start()
    {
        blackScreenTime = 0.001f;
        insanityTime = 0.001f;
        shakeTime = 0.001f;
        player.OnSpawn += (SpawnArgs args) => ResetEffects(); // delayed
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

        if (insane)
        {
            insanityInterpolation += Time.deltaTime / insanityTime;
        }
        else
        {
            insanityInterpolation -= Time.deltaTime / insanityTime;
        }

        insanityInterpolation = Mathf.Clamp01(insanityInterpolation);
        insanity.weight = insanityInterpolation;

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

    public void Insanity(AnimationAction action, float time)
    {
        if (action == AnimationAction.FadeAppear)
        {
            insane = true;
        }
        else if (action == AnimationAction.FadeDisappear)
        {
            insane = false;
        }

        insanityTime = time;
    }

    void ResetEffects()
    {
        isBlackScreen = false;
        insane = false;
    }
}
