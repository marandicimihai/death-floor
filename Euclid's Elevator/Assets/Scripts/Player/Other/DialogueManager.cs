using System.Collections.Generic;
using DeathFloor.SaveSystem;
using UnityEngine;
using System.Linq;

public class DialogueManager : MonoBehaviour, ISaveData<DialogueData>
{
    public bool CanSave => true;

    [SerializeField] DialogueHUD hud;

    static Line currentLine;
    static List<Line> used;

    private void Start()
    {
        used = new();
    }

    public void OnFirstTimeLoaded()
    {
        
    }

    public DialogueData OnSaveData()
    {
        return new DialogueData(used);
    }

    public void LoadData(DialogueData data)
    {
        used = data.UsedLines;
    }

    /// <summary>
    /// Handles requests. The event is fired and the HUD class displays the line. When it is finished it will call FinishedLine().
    /// </summary>
    /// <param name="line"></param>
    public void SayLine(Line line)
    {
        if (line.oneTime && used.Contains(line))
            return;

        if (currentLine == null || line.name != currentLine.name)
        {
            if (hud != null)
            {
                hud.StartSaying(line);
            }
            currentLine = line;
        }
    }

    public void FinishedLine(Line line)
    {
        used.Add(line);
        currentLine = null;
    }
}