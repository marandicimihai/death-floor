using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    public Transform enemy;

    private void Awake()
    {
        instance = this;
    }
}
