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
            camCon.TryGetComponent(out Camera camera))
        {
            GeomUtil.CalculateFrustumPlanes(planes, camera.cameraToWorldMatrix);
            if (!GameManager.instance.enemy.TryGetComponent(out Collider col) && !GeometryUtility.TestPlanesAABB(planes, col.bounds))
                return;

            insanity += insanityPerSecondWhenLooking * Time.deltaTime;
            insanity = Mathf.Clamp(insanity, minInsanity, maxInsanity);
        }
    }
}

public static class GeomUtil
{

    private static System.Action<Plane[], Matrix4x4> _calculateFrustumPlanes_Imp;
    public static void CalculateFrustumPlanes(Plane[] planes, Matrix4x4 worldToProjectMatrix)
    {
        if (planes == null) throw new System.ArgumentNullException("planes");
        if (planes.Length < 6) throw new System.ArgumentException("Output array must be at least 6 in length.", "planes");

        if (_calculateFrustumPlanes_Imp == null)
        {
            var meth = typeof(GeometryUtility).GetMethod("Internal_ExtractPlanes", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new System.Type[] { typeof(Plane[]), typeof(Matrix4x4) }, null);
            if (meth == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");

            _calculateFrustumPlanes_Imp = System.Delegate.CreateDelegate(typeof(System.Action<Plane[], Matrix4x4>), meth) as System.Action<Plane[], Matrix4x4>;
            if (_calculateFrustumPlanes_Imp == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
        }

        _calculateFrustumPlanes_Imp(planes, worldToProjectMatrix);
    }

}
