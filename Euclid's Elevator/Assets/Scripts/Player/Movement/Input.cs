using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Input : MonoBehaviour
{
    public static PlayerInputActions InputActions { get; private set; }

    bool eventsDone;

    public static void Init()
    {
        InputActions = new PlayerInputActions();

        InputActions.Enable();
        InputActions.Box.Disable();
    }

    private void Start()
    {
        TryEvents();
        SceneManager.activeSceneChanged += (Scene first, Scene second) =>
        {
            TryEvents();
        };
    }

    void TryEvents()
    {
        if (eventsDone)
        {
            return;
        }

        try
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
            eventsDone = true;
        }
        catch { }
    }
}