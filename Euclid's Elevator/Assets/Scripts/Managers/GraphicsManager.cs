using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    private void Start()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnSettingsChanged += (Settings settings) =>
            {
                QualitySettings.vSyncCount = settings.VSync ? 1 : 0;
                QualitySettings.SetQualityLevel(settings.QualityIndex);
                Screen.SetResolution(settings.Width, settings.Height, settings.Fullscreen);
            };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }
}
