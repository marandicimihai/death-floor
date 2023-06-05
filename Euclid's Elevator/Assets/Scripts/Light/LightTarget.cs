using UnityEngine;

public class LightTarget : MonoBehaviour
{
    public static LightTarget target;
    public float maxDistance;

    private void Awake()
    {
        target = this;
    }
}
