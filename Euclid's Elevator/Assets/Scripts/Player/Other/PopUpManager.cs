using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public delegate void ShowPopUp(PopUpProperties properties);
    public ShowPopUp PopUpShow;

    List<string> used;

    private void Awake()
    {
        used = new();
    }

    private void Start()
    {
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.currentSaveData != null &&
                SaveSystem.Instance.currentSaveData.usedPopUps.Length > 0)
            {
                used = SaveSystem.Instance.currentSaveData.usedPopUps.ToList();
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                data.usedPopUps = used.ToArray();
            };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }

    public void PopUp(PopUpProperties popUp)
    {
        if ((used.Contains(popUp.name) && popUp.oneTime) || popUp == null)
        {
            return;
        }

        PopUpShow?.Invoke(popUp);

        if (popUp.oneTime)
        {
            used.Add(popUp.name);
        }
    }
}