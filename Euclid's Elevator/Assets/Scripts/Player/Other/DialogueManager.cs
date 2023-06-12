using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueHUD hud;

    Line currentLine;
    List<Line> used;

    private void Awake()
    {
        Instance = this;
        used = new();
    }

    public void SayLine(Line line)
    {
        if (line.oneTime && used.Contains(line))
            return;

        if (currentLine == null || line.name != currentLine.name)
        {
            hud.StartSaying(line);
            currentLine = line;
        }
    }

    public void FinishedLine(Line line)
    {
        used.Add(line);
        currentLine = null;
    }
}