using UnityEngine;

public class AudioOcclusionTarget : MonoBehaviour
{
    public static Transform target;

    private void Awake()
    {
        target = transform;
    }
}
