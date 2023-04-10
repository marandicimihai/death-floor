using UnityEngine;

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

    float insanity;
    float t;

    private void Update()
    {
        if (GameManager.instance.enemyController.visibleToPlayer)
        {
            if (t < insanityTime)
                t += Time.deltaTime;

            InsanityMeter = GetInsanity(t);

            if (InsanityMeter >= maxInsanity)
            {
                controller.Die();
            }
        }
        else
        {
            t = 0;
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
