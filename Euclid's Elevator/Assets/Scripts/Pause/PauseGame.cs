using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PauseGame : MonoBehaviour
{
    public bool Paused { get; private set; }
    public static PauseGame Instance;
    public EventHandler OnPause;
    public EventHandler OnUnPause;

    bool init;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Input.InputActions.Realtime.Pause.performed += (InputAction.CallbackContext context) => 
        {
            TogglePause();
        };
    }

    private void Update()
    {
        if (!init)
        {
            Unpause();
            init = true;
        }
    }
    
    void TogglePause()
    {
        if (Paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        OnPause?.Invoke(this, new EventArgs());
        Paused = true;
    }


    public void Unpause()
    {
        Time.timeScale = 1;
        OnUnPause?.Invoke(this, new EventArgs());
        Paused = false;
    }
}
