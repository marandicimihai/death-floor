using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    static Line currentLine;
    static List<string> used;

    public delegate void SayLineHUD(Line line);
    public SayLineHUD OnSayLine;

    private void Awake()
    {
        Instance = this;
        used = new();
    }

    public void SayLine(Line line)
    {
        if (line.oneTime && used.Contains(line.name))
            return;

        if (currentLine == null || line.name != currentLine.name)
        {
            OnSayLine?.Invoke(line);
            currentLine = line;
        }
    }

    public void FinishedLine(Line line)
    {
        used.Add(line.name);
        currentLine = null;
    }
}