using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

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
            OnSettingsChanged?.Invoke(LoadSettings());
            startedupdate = true;
        }
    }

    public void SaveSettings(Settings settings)
    {
        BinaryFormatter formatter = new();

        string path = Application.persistentDataPath + "/settings.uwu";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, settings);
        stream.Close();

        OnSettingsChanged?.Invoke(settings);
    }

    public Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.uwu";
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
            return null;
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
}