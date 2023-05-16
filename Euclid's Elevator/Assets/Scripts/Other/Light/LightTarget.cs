using UnityEngine;

public class LightTarget : MonoBehaviour
{
    [SerializeField] float renderDistance;

    private void Update()
    {
        foreach (LightGroup group in LightGroup.groups)
        {
            if (Vector3.Distance(group.transform.position, transform.position) <= renderDistance)
            {
                group.Switch(true);
            }
            else
            {
                group.Switch(false);
            }
        }
    }
}
