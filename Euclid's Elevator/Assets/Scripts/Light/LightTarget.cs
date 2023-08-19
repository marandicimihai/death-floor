using UnityEngine;

public class LightTarget : MonoBehaviour
{
    [Header("THERE CAN ONLY BE ONE INSTANCE OF THIS CLASS")]
    public static LightTarget target;
    public float maxDistance;

    private void Awake()
    {
        target = this;
    }
}
