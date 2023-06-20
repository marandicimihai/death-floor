using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public bool initOnAwake;
    public float time;

    private void Awake()
    {
        if (initOnAwake)
        {
            Initiate(time);
        }
    }

    public void Initiate(float time)
    {
        this.time = time;

        Invoke(nameof(DestroyObject), time);
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
