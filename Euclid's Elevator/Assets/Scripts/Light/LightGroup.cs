using UnityEngine;

public class LightGroup : MonoBehaviour
{
    Light[] lights;

    private void Awake()
    {
        lights = GetComponentsInChildren<Light>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, LightTarget.target.transform.position) <= LightTarget.target.maxDistance)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }
        else
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }
    }
}
