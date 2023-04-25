using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(FpsController))]
public class Insanity : MonoBehaviour
{
    public float InsanityMeter 
    {
        get
        {
            return insanity;
        }
        set
        {
            insanity = Mathf.Clamp(value, minInsanity, maxInsanity);
        }
    }

    [SerializeField] FpsController controller;
    [SerializeField] CameraController camCon;

    [SerializeField] float insanityTime;
    [SerializeField] AnimationCurve insanityCurve;
    
    [SerializeField] float minInsanity;
    [SerializeField] float maxInsanity;

    [SerializeField] Volume volume;
    [SerializeField] float timeForFullWeight;
    [SerializeField] float timeForNoWeight;

    float insanity;
    float t;

    private void Start()
    {
        GameManager.MakePausable(this);
    }

    private void Update()
    {
        if (GameManager.Instance.enemyController.visibleToPlayer)
        {
            if (t < insanityTime)
                t += Time.deltaTime;

            InsanityMeter = GetInsanity(t);

            if (InsanityMeter >= maxInsanity)
            {
                controller.InsanityDie();
            }

            volume.weight = Mathf.Clamp01(t / timeForFullWeight);
        }
        else
        {
            t = 0;
            volume.weight = Mathf.Clamp01(volume.weight - timeForNoWeight * Time.deltaTime);
        }
    }

    public void Die()
    {
        InsanityMeter = minInsanity;
    }

    public float GetInsanity(float n)
    {
        n /= insanityTime;
        return insanityCurve.Evaluate(n) * maxInsanity;
    }
}
