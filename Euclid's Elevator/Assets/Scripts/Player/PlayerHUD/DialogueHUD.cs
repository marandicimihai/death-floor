using UnityEngine.UI;
using UnityEngine;

public class DialogueHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Text dialogue;
    [SerializeField] float timeInBetweenLetters;

    Line line;
    float letterTimeElapsed;
    float lastingElapsed;
    int currentLetter;
    bool typing;

    private void Awake()
    {
        dialogueManager.OnSayLine += StartSaying;
    }

    private void Update()
    {
        dialogue.enabled = !hideHUD;
        if (typing)
        {
            letterTimeElapsed += Time.deltaTime;
            if (letterTimeElapsed >= timeInBetweenLetters)
            {
                try
                {
                    dialogue.text += line.text[currentLetter];
                }
                catch
                {
                    dialogueManager.FinishedLine(line);
                    currentLetter = 0;
                    typing = false;
                }
                currentLetter++;
                letterTimeElapsed = 0;
            }
        }
        else
        {
            if (line != null)
            {
                lastingElapsed += Time.deltaTime;
                if (lastingElapsed >= line.timeLastingAfterTyping)
                {
                    dialogue.text = string.Empty;
                    letterTimeElapsed = 0;
                    lastingElapsed = 0;
                    currentLetter = 0;

                    line = null;
                }
            }
        }
    }

    public void StartSaying(Line line)
    {
        this.line = line;

        dialogue.text = string.Empty;
        letterTimeElapsed = 0;
        lastingElapsed = 0;
        currentLetter = 0;
        
        typing = true;
    }
}