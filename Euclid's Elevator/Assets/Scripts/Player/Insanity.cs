using UnityEngine;
using UnityEngine.UI;
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
            t = insanity / maxInsanity * insanityTime;
        }
    }

    [SerializeField] FpsController controller;
    [SerializeField] CameraController camCon;

    [SerializeField] float insanityTime;
    
    [SerializeField] float minInsanity;
    [SerializeField] float maxInsanity;

    [SerializeField] Volume volume;
    [SerializeField] float timeForFullWeight;
    [SerializeField] float timeForNoWeight;

    [SerializeField] Image slider;

    float insanity;
    float t;

    private void Start()
    {
        GameManager.MakePausable(this);

        GameManager.Instance.OnStageStart += (object caller, StageArgs args) => Die();
    }

    private void Update()
    {
        slider.fillAmount = insanity / 100f;
        if (GameManager.Instance.enemyController.Visible)
        {
            if (t < insanityTime)
                t += Time.deltaTime;

            InsanityMeter = t / insanityTime * maxInsanity;

            if (InsanityMeter >= maxInsanity && !controller.Dead)
            {
                controller.InsanityDie();
            }

            volume.weight = Mathf.Clamp01(t / timeForFullWeight);
        }
        else
        {
            volume.weight = Mathf.Clamp01(volume.weight - timeForNoWeight * Time.deltaTime);
        }
    }

    public void Die()
    {
        InsanityMeter = minInsanity;
    }
}
