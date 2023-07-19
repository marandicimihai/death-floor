using System.Reflection;
using System;

[System.Serializable]
public class Settings
{
    //Update immediately
    public float EffectsVolume 
    { 
        get
        {
            if (effectsVolume == null)
            {
                return 0;
            }
            else
            {
                return (float)effectsVolume;
            }
        }
        set
        {
            effectsVolume = value;
        }
    }
    public float? effectsVolume;

    public float AmbienceVolume
    {
        get
        {
            if (ambienceVolume == null)
            {
                return 0;
            }
            else
            {
                return (float)ambienceVolume;
            }
        }
        set
        {
            ambienceVolume = value;
        }
    }
    public float? ambienceVolume;

    public bool Bloom
    {
        get
        {
            if (bloom == null)
            {
                return false;
            }
            else
            {
                return (bool)bloom;
            }
        }
        set
        {
            bloom = value;
        }
    }
    public bool? bloom;

    public bool Blur
    {
        get
        {
            if (blur == null)
            {
                return false;
            }
            else
            {
                return (bool)blur;
            }
        }
        set
        {
            blur = value;
        }
    }
    public bool? blur;

    public float Sensitivity
    {
        get
        {
            if (sensitivity == null)
            {
                return 0.1f;
            }
            else
            {
                return (float)sensitivity;
            }
        }
        set
        {
            sensitivity = value;
        }
    }
    public float? sensitivity;

    //Update on apply

    public int ResIndex
    {
        get
        {
            if (resIndex == null)
            {
                return 0;
            }
            else
            {
                return (int)resIndex;
            }
        }
        set
        {
            resIndex = value;
        }
    }
    public int? resIndex;

    public int Width
    {
        get
        {
            if (width == null)
            {
                return 0;
            }
            else
            {
                return (int)width;
            }
        }
        set
        {
            width = value;
        }
    }
    public int? width;

    public int Height
    {
        get
        {
            if (height == null)
            {
                return 0;
            }
            else
            {
                return (int)height;
            }
        }
        set
        {
            height = value;
        }
    }
    public int? height;

    public int QualityIndex
    {
        get
        {
            if (qualityIndex == null)
            {
                return 0;
            }
            else
            {
                return (int)qualityIndex;
            }
        }
        set
        {
            qualityIndex = value;
        }
    }
    public int? qualityIndex;

    public bool VSync
    {
        get
        {
            if (vSync == null)
            {
                return false;
            }
            else
            {
                return (bool)vSync;
            }
        }
        set
        {
            vSync = value;
        }
    }
    public bool? vSync;

    public bool Fullscreen
    {
        get
        {
            if (fullscreen == null)
            {
                return false;
            }
            else
            {
                return (bool)fullscreen;
            }
        }
        set
        {
            fullscreen = value;
        }
    }
    public bool? fullscreen;

    public string[] InputPaths;

    public Settings()
    {

    }

    //ALL SETTINGS
    public Settings(float effectsVolume, float ambianceVolume, bool bloom, bool blur, float sensitivity, int resIndex, int width, int height, int qualityIndex, bool vSync, bool fullScreen, string[] inputPaths)
    {
        this.EffectsVolume = effectsVolume;
        this.AmbienceVolume = ambianceVolume;
        this.Bloom = bloom;
        this.Blur = blur;
        this.Sensitivity = sensitivity;
        this.ResIndex = resIndex;
        this.Width = width;
        this.Height = height;
        this.QualityIndex = qualityIndex;
        this.VSync = vSync;
        this.Fullscreen = fullScreen;
        this.InputPaths = inputPaths;
    }

    //GRAPHICS
    public Settings(bool bloom, bool blur, int resIndex, int width, int height, int qualityIndex, bool vSync, bool fullScreen)
    {
        this.Bloom = bloom;
        this.Blur = blur;
        this.ResIndex = resIndex;
        this.Width = width;
        this.Height = height;
        this.QualityIndex = qualityIndex;
        this.VSync = vSync;
        this.Fullscreen = fullScreen;
    }

    //INPUT
    public Settings(float sensitivity)
    {
        this.Sensitivity = sensitivity;
    }

    public Settings(string[] inputPaths)
    {
        InputPaths = inputPaths;
    }

    //AUDIO
    public Settings(float effectsVolume, float ambianceVolume)
    {
        this.EffectsVolume = effectsVolume;
        this.AmbienceVolume = ambianceVolume;
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