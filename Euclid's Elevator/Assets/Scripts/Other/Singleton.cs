using UnityEngine;

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
    }
}