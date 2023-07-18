using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    public GameData currentSaveData;

    public bool CanSave
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
    bool canSave = true;

    [SerializeField] int saveSlots;
    int? currentSave;

    public delegate void SaveTheGame(ref GameData data);
    public SaveTheGame OnSaveGame;

    public delegate void SettingsChanged(Settings settings);
    public SettingsChanged OnSettingsChanged;

    bool startedupdate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        /////////TESTING
        currentSave = 0;
        currentSaveData = LoadGame(0);
        /////////TESTING
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += (Scene first, Scene second) =>
        {
            OnSettingsChanged?.Invoke(LoadSettings());
        };
    }

    private void Update()
    {
        if (!startedupdate)
        {
            startedupdate = true;
            OnSettingsChanged?.Invoke(LoadSettings());
        }
    }

    [MenuItem("Dev/Save Game")]
    public static void SaveData()
    {
        Instance.SaveGame();
    }

    [MenuItem("Dev/Clear Save")]
    public static void Clear()
    {
        Instance.ClearData(0);
    }

    public void LoadGameData(int index)
    {
        SceneManager.LoadScene("Main");
        currentSaveData = LoadGame(index);
        currentSave = index;
    }

    public bool SaveGame()
    {
        if (currentSave == null)
        {
            return false;
        }

        GameData data = new();
        OnSaveGame?.Invoke(ref data);

        if (!canSave)
        {
            return false;
        }

        if (CanSave)
        {
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{currentSave}.owo");
            using (FileStream stream = new(path, FileMode.Create))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
                stream.Write(info, 0, info.Length);
            }
            return true;
        }
        return false;
    }

    public void ClearData(int index)
    {
        GameData data = new();
        string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
        using (FileStream stream = new(path, FileMode.Create))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
            stream.Write(info, 0, info.Length);
        }
    }

    GameData LoadGame(int index)
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

    #region Settings

    public void SaveSettings(Settings settings)
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

    public Settings LoadSettings()
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
        foreach (FieldInfo field in typeof(Settings).GetFields())
        {
            Debug.Log(field.Name + ": " + field.GetValue(Instance.LoadSettings()));
        }
        Instance.SaveSettings(new Settings());
    }

    #endregion

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SaveGame();
        }
    }
}