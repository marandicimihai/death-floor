using System.Reflection;

[System.Serializable]
public class Settings
{
    //Update immediately
    public float? effectsVolume;
    public float? ambianceVolume;
    public bool? bloom;
    public bool? blur;
    public float? sensitivity;

    //Update on apply
    public int? resIndex;
    public int? qualityIndex;
    public bool? vSync;
    public bool? fullScreen;

    //ALL SETTINGS
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

    //GRAPHICS
    public Settings(bool bloom, bool blur, int resIndex, int qualityIndex, bool vSync, bool fullScreen)
    {
        this.bloom = bloom;
        this.blur = blur;
        this.resIndex = resIndex;
        this.qualityIndex = qualityIndex;
        this.vSync = vSync;
        this.fullScreen = fullScreen;
    }

    //INPUT
    public Settings(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }

    //AUDIO
    public Settings(float effectsVolume, float ambianceVolume)
    {
        this.effectsVolume = effectsVolume;
        this.ambianceVolume = ambianceVolume;
    }

    public static Settings operator +(Settings left, Settings right)
    {
        foreach (FieldInfo field in right.GetType().GetFields())
        {
            if (field.GetValue(right) != null)
            {
                field.SetValue(left, field.GetValue(right));
            }
        }
        return left;
    }
}