using UnityEngine;
using System;

public class Input : MonoBehaviour
{
    public static PlayerInputActions InputActions { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputActions();

        InputActions.Enable();
        InputActions.Box.Disable();
    }

    private void Start()
    {
        PauseGame.Instance.OnPause += (object caller, EventArgs args) =>
        {
            InputActions.General.Disable();
            InputActions.Box.Disable();
        };
        PauseGame.Instance.OnUnPause += (object caller, EventArgs args) =>
        {
            InputActions.General.Enable();
            InputActions.Box.Enable();
        };
        GameManager.Instance.OnGameOver += (object caller, EventArgs args) =>
        {
            InputActions.Disable();
        };
        GameManager.Instance.OnGameEnd += (object caller, EventArgs args) =>
        {
            InputActions.Disable();
        };
    }
}