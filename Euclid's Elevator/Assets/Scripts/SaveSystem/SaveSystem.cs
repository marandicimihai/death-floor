using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public static class SaveSystem
{
    public static bool CanSave
    {
        get
        {
            return canSave;
        }
        set
        {
            if (!value)
            {
                canSave = false;
            }
        }
    }
    public delegate void SaveTheGame(ref GameData data);
    public static SaveTheGame OnSaveGame;

    public delegate void SettingsChanged(Settings settings);
    public static SettingsChanged OnSettingsChanged;
    public static GameData CurrentSaveData
    {
        get
        {
            if (!hasLoadedData)
            {
                currentSaveData = LoadGame(CurrentSave);
                hasLoadedData = true;
            }
            return currentSaveData;
        }
    }

    static GameData currentSaveData;
    static bool canSave = true;

    static readonly int saveSlots = 4;
    static int CurrentSave
    {
        get
        {
            return currentSave;
        }
        set
        {
            currentSave = value;
            hasLoadedData = false;
        }
    }
    static int currentSave;
    static bool hasLoadedData;

    #region Save Slots

    [MenuItem("Dev/Save System/Save Slot")]
    public static void SaveData()
    {
        SaveGame(CurrentSave);
    }

    [MenuItem("Dev/Save System/Clear Save Slot")]
    static void Clear()
    {
        ClearSlotData(CurrentSave);
    }

    [MenuItem("Dev/Save System/Save Up")]
    public static void SaveUp()
    {
        CurrentSave += 1;
        if (CurrentSave >= saveSlots)
        {
            CurrentSave = 0;
        }
        Debug.Log($"Save slot set to {CurrentSave}.");
    }

    [MenuItem("Dev/Save System/Save Down")]
    public static void SaveDown()
    {
        CurrentSave -= 1;
        if (CurrentSave <= -1)
        {
            CurrentSave = 3;
        }
        Debug.Log($"Save slot set to {CurrentSave}.");
    }

    public static bool SaveGame(int index)
    {
        GameData data = new();
        OnSaveGame?.Invoke(ref data);

        if (!CanSave)
        {
            return false;
        }

        if (CanSave)
        {
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            using (FileStream stream = new(path, FileMode.Create))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
                stream.Write(info, 0, info.Length);
            }
            return true;
        }
        return false;
    }

    public static void ClearSlotData(int index)
    {
        GameData data = new();
        string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
        using (FileStream stream = new(path, FileMode.Create))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
            stream.Write(info, 0, info.Length);
        }
    }

    static GameData LoadGame(int index)
    {
        string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
        if (File.Exists(path))
        {
            using (FileStream stream = new(path, FileMode.Open))
            {
                StreamReader reader = new(stream);
                GameData data = new();
                JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), data);
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SetSlot(int index)
    {
        CurrentSave = index;
    }

    #endregion

    #region Settings

    public static void SaveSettings(Settings settings)
    {
        BinaryFormatter formatter = new();

        string path = Path.Combine(Application.persistentDataPath, "settings.uwu");
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, settings);
        stream.Close();

        if (settings != null)
        {
            OnSettingsChanged?.Invoke(settings);
        }
    }

    public static Settings LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, "settings.uwu");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new FileStream(path, FileMode.Open);

            Settings settings = formatter.Deserialize(stream) as Settings;
            stream.Close();
            return settings;
        }
        else
        {
            Settings settings = new();

            SaveSettings(settings);

            return settings;
        }
    }

    [MenuItem("Dev/ClearSettings")]
    public static void ClearSettings()
    {
        SaveSettings(new Settings());
    }

    #endregion
}