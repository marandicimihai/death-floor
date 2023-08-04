using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueHUD hud;

    static Line currentLine;
    static List<string> used;

    private void Awake()
    {
        used = new();
    }

    private void Start()
    {
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.currentSaveData != null &&
                SaveSystem.Instance.currentSaveData.usedLines.Length > 0)
            {
                used = SaveSystem.Instance.currentSaveData.usedLines.ToList();
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                data.usedLines = used.ToArray();
            };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }

    /// <summary>
    /// Handles requests. The event is fired and the HUD class displays the line. When it is finished it will call FinishedLine().
    /// </summary>
    /// <param name="line"></param>
    public void SayLine(Line line)
    {
        if (line.oneTime && used.Contains(line.name))
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
        used.Add(line.name);
        currentLine = null;
    }
}