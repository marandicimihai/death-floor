using UnityEngine;

public class Input : MonoBehaviour
{
    public static PlayerInputActions InputActions { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputActions();

        InputActions.Enable();
    }
}