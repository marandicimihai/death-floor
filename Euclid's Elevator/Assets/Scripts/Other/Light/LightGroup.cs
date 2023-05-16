using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour
{
    public static List<LightGroup> groups;
    static bool listInit;

    Light[] lights;

    bool on = true;

    private void Awake()
    {
        if (!listInit)
        {
            groups = new List<LightGroup>();
            listInit = true;
        }

        lights = GetComponentsInChildren<Light>();
        groups.Add(this);
    }

    /*private void OnDrawGizmos()
    {
        if (on)
        {
            Gizmos.DrawSphere(transform.position, 2.5f);
        }
    }*/

    public void Switch(bool on)
    {
        if (this.on == on)
            return;

        if (on)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }

            this.on = on;
        }
        else
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }

            this.on = on;
        }
    }
}
