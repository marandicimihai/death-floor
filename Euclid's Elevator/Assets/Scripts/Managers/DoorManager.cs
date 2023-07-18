using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Header("THE FOLLOWING ARRAY IS USED FOR THE SAVE. DONT CHANGE!")]
    [SerializeField] Door[] doors;

    private void Start()
    {
        if (SaveSystem.Instance.currentSaveData != null)
        {
            if (SaveSystem.Instance.currentSaveData.locked.Length != 0 &&
                SaveSystem.Instance.currentSaveData.open.Length != 0)
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    doors[i].Init(SaveSystem.Instance.currentSaveData.locked[i], 
                                  SaveSystem.Instance.currentSaveData.open[i], 
                                  SaveSystem.Instance.currentSaveData.stage);
                }
            }
            else
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    doors[i].StartedGame();
                }
            }
        }
        else
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].StartedGame();
            }
        }

        SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            List<bool> locked = new();
            List<bool> open = new();

            for (int i = 0; i < doors.Length; i++)
            {
                locked.Add(doors[i].Locked);
                open.Add(doors[i].Open);
            }

            data.locked = locked.ToArray();
            data.open = open.ToArray();
        };
    }
}
