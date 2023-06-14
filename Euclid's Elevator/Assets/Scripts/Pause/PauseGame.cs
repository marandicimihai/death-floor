using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        AudioManager.Instance.Pause();
    }


    public void Unpause()
    {
        Time.timeScale = 1;
        AudioManager.Instance.Unpause();
    }
}
