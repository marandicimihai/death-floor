using UnityEngine;

public class BehaviourService : MonoBehaviour, IBehaviourService
{
    [SerializeField] MonoBehaviour[] behaviours;

    public bool RequestComponentOfType<T>(out T t)
    {
        t = default;
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i] is T)
            {
                t = (T)System.Convert.ChangeType(behaviours[i], typeof(T));
            }
        }

        Debug.Log($"Didn't find behaviour of type {typeof(T)}");
        return false;
    }
}
