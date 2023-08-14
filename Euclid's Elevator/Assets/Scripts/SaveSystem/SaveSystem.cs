using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Reflection;
using System;

namespace DeathFloor.SaveSystem
{
    public static class SaveSystem
    {
        public delegate void SettingsChanged(Settings settings);
        public static SettingsChanged OnSettingsChanged;

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
            }
        }
        static int currentSaveIndex;

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

        public static void SaveGame(int index)
        {
            SaveData data = new();

            foreach (Type behaviourType in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (Type interfaceType in behaviourType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ISaveData<>))
                    {
                        UnityEngine.Object found = GameObject.FindObjectOfType(behaviourType, true);
                        if (found != null)
                        {
                            if ((bool)behaviourType.GetProperty("CanSave").GetValue(found) == false)
                            {
                                Debug.Log($"Can't save because of object of type {behaviourType}.");
                                return;
                            }
                            data += (SaveData)behaviourType.GetMethod("OnSaveData").Invoke(found, null);
                        }
                        else
                        {
                            Debug.Log($"Couldn't find object of type {behaviourType}.");
                        }
                    }
                }
            }

            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            using FileStream stream = new(path, FileMode.Create);
            byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(data));
            stream.Write(info, 0, info.Length);
        }

        public static void ClearSlotData(int index)
        {
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            File.Delete(path);
        }

        public static void LoadGame(int index)
        {
            string path = Path.Combine(Application.persistentDataPath, $"saveddata{index}.owo");
            if (File.Exists(path))
            {
                SaveData data = new();
                using (FileStream stream = new(path, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), data);
                }

                foreach (Type behaviourType in Assembly.GetExecutingAssembly().GetTypes())
                {
                    foreach (Type interfaceType in behaviourType.GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ISaveData<>))
                        {
                            UnityEngine.Object found = GameObject.FindObjectOfType(behaviourType, true);
                            if (found != null)
                            {
                                MethodInfo info = behaviourType.GetMethod("LoadData");
                                Type paramType = info.GetParameters()[0].ParameterType;
                                object param = Activator.CreateInstance(paramType);
                                paramType.GetMethod("CopyData").Invoke(param, new object[] { data });
                                info.Invoke(found, new object[] { param });
                            }
                            else
                            {
                                Debug.Log($"Couldn't find object of type {behaviourType}.");
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No save found.");
                foreach (Type behaviourType in Assembly.GetExecutingAssembly().GetTypes())
                {
                    foreach (Type interfaceType in behaviourType.GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ISaveData<>))
                        {
                            UnityEngine.Object found = GameObject.FindObjectOfType(behaviourType, true);
                            if (found != null)
                            {
                                behaviourType.GetMethod("OnFirstTimeLoaded").Invoke(found, null);
                            }
                            else
                            {
                                Debug.Log($"Couldn't find object of type {behaviourType}.");
                            }
                        }
                    }
                }
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