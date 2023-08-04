using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] PopUpHUD hud;
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

        if (hud != null)
        {
            hud.PopUp(popUp);
        }
        else
        {
            Debug.Log("No hud.");
        }

        if (popUp.oneTime)
        {
            used.Add(popUp.name);
        }
    }
}