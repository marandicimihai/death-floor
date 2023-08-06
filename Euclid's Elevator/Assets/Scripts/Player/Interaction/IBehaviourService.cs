using UnityEngine;

public interface IBehaviourService
{
    public static IBehaviourService GetAvailableService()
    {
        Object @object = Object.FindObjectOfType(typeof(IBehaviourService));
        if (@object != null)
        {
            return @object as IBehaviourService;
        }
        Debug.Log("Did not find a service");
        return null;
    }

    public bool RequestComponentOfType<T>(out T t);
}