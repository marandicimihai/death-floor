[System.Serializable]
public class Settings
{
    //Main scene settings
    public float effectsVolume;
    public float ambianceVolume;
    public bool bloom;
    public bool blur;
    public float sensitivity;

    //Menu scene settings
    public int resIndex;
    public int qualityIndex;
    public bool vSync;
    public bool fullScreen;

    public Settings(float effectsVolume, float ambianceVolume, bool bloom, bool blur, float sensitivity, int resIndex, int qualityIndex, bool vSync, bool fullScreen)
    {
        this.effectsVolume = effectsVolume;
        this.ambianceVolume = ambianceVolume;
        this.bloom = bloom;
        this.blur = blur;
        this.sensitivity = sensitivity;
        this.resIndex = resIndex;
        this.qualityIndex = qualityIndex;
        this.vSync = vSync;
        this.fullScreen = fullScreen;
    }

    public Settings(int resIndex, int qualityIndex, bool vSync, bool fullScreen)
    {
        this.resIndex = resIndex;
        this.qualityIndex = qualityIndex;
        this.vSync = vSync;
        this.fullScreen = fullScreen;
    }
}
