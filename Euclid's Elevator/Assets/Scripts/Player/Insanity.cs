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

    LayerMask visionMask;
    float insanity;
    float t;

    private void Awake()
    {
        visionMask = controller.settings.visionMask;
    }

    private void Update()
    {
        if (!Physics.Raycast(camCon.Camera.position, GameManager.instance.enemy.position - camCon.Camera.position, 
            Vector3.Distance(GameManager.instance.enemy.position, transform.position), visionMask) &&
            !controller.Paralized && camCon.Camera.TryGetComponent(out Camera camera) && GameManager.instance.enemy.TryGetComponent(out Collider col) && 
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds))
        {
            GameManager.instance.enemyController.Stop();
            
            if (t < insanityTime)
                t += Time.deltaTime;

            InsanityMeter = GetInsanity(t);

            if (InsanityMeter >= maxInsanity)
            {
                controller.Die();
            }
        }
        else if (!controller.Paralized)
        {
            GameManager.instance.enemyController.Continue();
            t = 0;
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
