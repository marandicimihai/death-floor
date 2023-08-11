using System.Collections.Generic;
using DeathFloor.SaveSystem;
using UnityEngine;

public class PopUpManager : MonoBehaviour, ISaveData<PopUpData>
{
    public bool CanSave => true;
    [SerializeField] PopUpHUD hud;
    List<PopUpProperties> used;

    private void Awake()
    {
        used = new();
    }

    public void OnFirstTimeLoaded()
    {

    }

    public PopUpData OnSaveData()
    {
        return new PopUpData(used);
    }

    public void LoadData(PopUpData data)
    {
        used = data.UsedPopUps;
    }

    public void PopUp(PopUpProperties popUp)
    {
        if ((used.Contains(popUp) && popUp.oneTime) || popUp == null)
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
            used.Add(popUp);
        }
    }
}