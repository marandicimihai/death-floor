using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    static Line currentLine;
    static List<Line> used;

    public delegate void SayLineHUD(Line line);
    public SayLineHUD OnSayLine;

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
            OnSayLine?.Invoke(line);
            currentLine = line;
        }
    }

    public void FinishedLine(Line line)
    {
        used.Add(line);
        currentLine = null;
    }
}