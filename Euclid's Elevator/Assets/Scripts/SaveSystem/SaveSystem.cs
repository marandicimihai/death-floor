using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Reflection;
using System.Linq;
using System;

namespace DeathFloor.SaveSystem
{
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

        public delegate void SettingsChanged(Settings settings);
        public static SettingsChanged OnSettingsChanged;
        static SaveData CurrentSaveData
        {
            get
            {
                if (!hasLoadedData)
                {
                    currentSaveData = LoadGame(CurrentSaveIndex);
                    hasLoadedData = true;
                }
                return currentSaveData;
            }
        }

        static SaveData currentSaveData;
        static bool canSave = true;

        static readonly int saveSlots = 4;
        static int CurrentSaveIndex
        {
            get
            {
                return currentSaveIndex;
            }
            set
            {
                currentSaveIndex = value;
                hasLoadedData = false;
            }
        }
        static int currentSaveIndex;
        static bool hasLoadedData;

        #region Save Slots

        [MenuItem("Dev/Save System/Save Slot")]
        public static void SaveData()
        {
            SaveGame(CurrentSaveIndex);
        }

        [MenuItem("Dev/Save System/Clear Save Slot")]
        static void Clear()
        {
            ClearSlotData(CurrentSaveIndex);
        }

        [MenuItem("Dev/Save System/Save Up")]
        public static void SaveUp()
        {
            CurrentSaveIndex += 1;
            if (CurrentSaveIndex >= saveSlots)
            {
                CurrentSaveIndex = 0;
            }
            Debug.Log($"Save slot set to {CurrentSaveIndex}.");
        }

        [MenuItem("Dev/Save System/Save Down")]
        public static void SaveDown()
        {
            CurrentSaveIndex -= 1;
            if (CurrentSaveIndex <= -1)
            {
                CurrentSaveIndex = 3;
            }
            Debug.Log($"Save slot set to {CurrentSaveIndex}.");
        }

        public static bool SaveGame(int index)
        {
            SaveData data = new();

            var saveType = typeof(ISaveData<>);

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(saveType)))
            {
                var objs = GameObject.FindObjectsOfType(type);
                if (objs.Length > 1)
                {
                    Debug.Log("sum weird");
                }
                else if (objs.Length == 1)
                {
                    type.GetMethod("OnSaveData").Invoke(objs[0], null);
                }
            }

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
            SaveData data = new();
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            using (FileStream stream = new(path, FileMode.Create))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
                stream.Write(info, 0, info.Length);
            }
        }

        static SaveData LoadGame(int index)
        {
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            if (File.Exists(path))
            {
                using (FileStream stream = new(path, FileMode.Open))
                {
                    StreamReader reader = new(stream);
                    SaveData data = new();
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
            CurrentSaveIndex = index;
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
}