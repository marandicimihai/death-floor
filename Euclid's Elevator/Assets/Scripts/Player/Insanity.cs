using UnityEngine;

[RequireComponent(typeof(FpsController))]
public class Insanity : MonoBehaviour
{
    [SerializeField] CameraController camCon;

    [SerializeField] float insanityPerSecondWhenLooking;

    [SerializeField] LayerMask visionMask;

    [SerializeField] float minInsanity;
    [SerializeField] float maxInsanity;

    Plane[] planes;
    float insanity;

    private void Update()
    {
        if (!Physics.Raycast(camCon.Camera.position, GameManager.instance.enemy.position - camCon.Camera.position, 
            Vector3.Distance(GameManager.instance.enemy.position, transform.position), visionMask) &&
            camCon.Camera.TryGetComponent(out Camera camera))
        {
            planes = GeometryUtility.CalculateFrustumPlanes(camera);
            if (!GameManager.instance.enemy.TryGetComponent(out Collider col) && !GeometryUtility.TestPlanesAABB(planes, col.bounds))
                return;

            insanity += insanityPerSecondWhenLooking * Time.deltaTime;
            insanity = Mathf.Clamp(insanity, minInsanity, maxInsanity);
        }
    }
}
