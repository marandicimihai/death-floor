using System.Collections;
using UnityEditor;
using UnityEngine;
using System;
using TMPro;

public class LineManager : MonoBehaviour
{
    static LineManager instance;

    [SerializeField] TMP_Text textBox;
    [SerializeField] float timeInBetweenLetters;
    [SerializeField] Line[] lines;
    
    Line currentLine;

    private void Awake()
    {
        instance = this;
    }

    public void SayLine (string name)
    {
        Line line = Array.Find(lines, line => line.name == name);

        if (line == null)
        {
            Debug.Log($"Couldn't find line named { name }");
            return;
        }
        else if (line == currentLine)
        {
            return;
        }
        else if (line != currentLine && currentLine != null)
        {
            StopAllCoroutines();
        }

        DisplayLine(line);
    }

    void DisplayLine (Line line)
    {
        textBox.text = string.Empty;

        currentLine = line;
        StartCoroutine(StartTyping(textBox, line, 0));
    }

    IEnumerator StartTyping (TMP_Text textBox, Line line, int index)
    {
        yield return new WaitForSeconds(timeInBetweenLetters);
        try
        {
            textBox.text += line.text[index];
            index += 1;
            StartCoroutine(StartTyping(textBox, line, index));
        }
        catch
        {
            StartCoroutine(LineTyped(textBox, line.timeLastingAfterTyping));
        }
    }

    IEnumerator LineTyped(TMP_Text textBox, float time)
    {
        yield return new WaitForSeconds(time);
        textBox.text = string.Empty;
        currentLine = null;
    }
}
