using UnityEngine.SceneManagement;
using System;

public class Input : Singleton<Input>
{
    public PlayerInputActions InputActions 
    {
        get
        {
            if (actions == null)
            {
                actions = new PlayerInputActions();
                actions.Enable();
            }
            return actions;
        }
    }

    PlayerInputActions actions;

    bool eventsDone;

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
            GameManager.Instance.OnGameWin += (object caller, EventArgs args) =>
            {
                InputActions.Disable();
            };
            eventsDone = true;
        }
        catch { }
    }
}