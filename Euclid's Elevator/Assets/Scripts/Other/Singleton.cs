using UnityEngine;

/// <summary>
/// Instantiates an object and adds component T to it when it is required to (not on awake). The class doesn't have to be in the hierarchy.
/// </summary>
/// <typeparam name="T">The type of the singleton</typeparam>
public class Singleton<T> : MonoBehaviour 
    where T : Component
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var newobj = new GameObject
                {
                    name = "Input"
                };
                instance = newobj.AddComponent<T>();
                Debug.Log($"Created instance of {instance.GetType()}.");
            }
            return instance;
        }
    }

    static T instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
}