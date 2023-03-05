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

    [SerializeField] float insanityPerSecondWhenLooking;

    [SerializeField] LayerMask visionMask;

    [SerializeField] float minInsanity;
    [SerializeField] float maxInsanity;

    float insanity;

    private void Update()
    {
        GameManager.instance.enemy.TryGetComponent(out Enemy enemyC);

        if (!Physics.Raycast(camCon.Camera.position, GameManager.instance.enemy.position - camCon.Camera.position, 
            Vector3.Distance(GameManager.instance.enemy.position, transform.position), visionMask) &&
            !controller.Paralized && camCon.Camera.TryGetComponent(out Camera camera) && GameManager.instance.enemy.TryGetComponent(out Collider col) && 
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds))
        {
            enemyC.Stop();
            
            insanity += insanityPerSecondWhenLooking * Time.deltaTime;
            insanity = Mathf.Clamp(insanity, minInsanity, maxInsanity);

            if (insanity >= maxInsanity)
            {
                controller.Die();
            }
        }
        else if (!controller.Paralized)
        {
            enemyC.Continue();
        }
    }

    public void Die()
    {
        insanity = minInsanity;
    }
}
