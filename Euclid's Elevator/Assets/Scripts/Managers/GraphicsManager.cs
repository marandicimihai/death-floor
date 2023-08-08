using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    private void Start()
    {
        SaveSystem.OnSettingsChanged += (Settings settings) =>
        {
            QualitySettings.vSyncCount = settings.VSync ? 1 : 0;
            QualitySettings.SetQualityLevel(settings.QualityIndex);
            Screen.SetResolution(settings.Width, settings.Height, settings.Fullscreen);
        };
    }
}
