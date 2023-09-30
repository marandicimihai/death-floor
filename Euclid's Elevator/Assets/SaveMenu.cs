using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveMenu : MonoBehaviour
{
    [SerializeField] bool doesTheSaveExist;
    [SerializeField] bool debugTrigger; // use this to test the menu!!!
    [SerializeField] bool isOpen;
    [SerializeField] Canvas thisCanvas;
    [SerializeField] Text[] saveTitles = new Text[2]; //the save title in the top right corner
    [SerializeField] Text[] saveSubTitles; //aka. the cabinet text;
    [SerializeField] GameObject scoreParent; //to disable and enable them visually
    [SerializeField] Text[] textScores; 
    [SerializeField] Button DeleteBut;
    [SerializeField] Button SelectBut;
    [SerializeField] Button[] SaveSelectionButtons;
    [SerializeField] string unusedSaveName = "Empty Save #"; // + number added after
    [SerializeField] string usedSaveName = "Save #";  // + number added after (it's also used by the "sub titles")
    [SerializeField] string noSaveSelectedName = "Select a save!";
    [SerializeField] string unusedSaveSubName = "Empty";
    [SerializeField] SaveExample defaultValueSave; //this value exists because because of fucking ghost NULLS, since in some cases unity CAN'T FUCKING DECIDE IF IT'S NULL OR NOT
    [SerializeField] int saveIndex = -1; //which save is currently selected; -1 = none
    [SerializeField] SaveExample[] saves = new SaveExample[4]; //this is used as place holder, after the save system is fully done replace this
    [System.Serializable]
    public class SaveExample //this is an example of what values/parameters i want out of a save;
    {
        public ScoreDataType[] scores;
        /*so far the scores will be time (stored in seconds), nr. of deaths,
         times you have beaten the game in said save and total loss of sanity
        (aka. all lost percentage of sanity will add up into one big score)
        */
    }
    [System.Serializable]
    public class ScoreDataType //idk man, don't yell at me
    {
        public string scoreTitle;
        public int scoreValue;
        public bool useTimerFormat;
    }
    void Start()
    {
        if (thisCanvas != null)
        {
            thisCanvas = GetComponent<Canvas>();
        }
        CheckSaveAndRefresh();
    }
    private void Update()
    {
        if (debugTrigger)
        {
            debugTrigger = false;
            CheckSaveAndRefresh();
        }
        if(saveIndex >= 0)
        {
            SaveSelectionButtons[saveIndex].Select(); //to make buttons stay selected
        }
    }
    private void CheckSaveAndRefresh() //refreshes the displayed data based on what save is selected and so on.
    {
        if(saveIndex >= saves.Length)
        {
            saveIndex = -1;
            Debug.LogError("You set the save index too high!");
        }
        thisCanvas.enabled = isOpen;
        if (saveIndex >= 0)
        {
            CheckIfSaveExists(saveIndex);
            SelectBut.interactable = true;
            DeleteBut.interactable = doesTheSaveExist; //if there is no save, there is no point in clicking the delete button
            scoreParent.SetActive(doesTheSaveExist);
            for (int i = 0; i < saveTitles.Length; i++)
            {
                if (doesTheSaveExist)
                {
                    saveTitles[i].text = usedSaveName + (saveIndex + 1);
                }
                else
                {
                    saveTitles[i].text = unusedSaveName + (saveIndex + 1);
                }
            }
            if (doesTheSaveExist)
            {
                for (int i = 0; i < saves[saveIndex].scores.Length; i++)
                {
                    CheckIfSaveExists(saveIndex);
                    if (saves[saveIndex].scores[i].useTimerFormat)
                    {
                        int timeAll = saves[saveIndex].scores[i].scoreValue;
                        int timeSeconds = timeAll % 60;
                        int timeMinutes = ((timeAll - timeSeconds) / 60) % 60;
                        int timeHours = (timeAll - timeMinutes * 60 - timeSeconds) / 3600;
                        string timeToString = timeHours.ToString("00") + ":" + timeMinutes.ToString("00") + ":" + timeSeconds.ToString("00");
                        textScores[i].text = timeToString;
                    }
                    else
                    {
                        textScores[i].text = saves[saveIndex].scores[i].scoreValue.ToString();
                    }
                }
            }
        }
        else
        {
            DeleteBut.interactable = false;
            SelectBut.interactable = false;
            scoreParent.SetActive(false);
            for (int i = 0; i < saveTitles.Length; i++)
            {
                saveTitles[i].text = noSaveSelectedName;
            }
        }
        for (int i = 0; i < saves.Length; i++)
        {
            CheckIfSaveExists(i);
            if (doesTheSaveExist)
            {
                saveSubTitles[i].text = usedSaveName + (i + 1);
            }
            else
            {
                saveSubTitles[i].text = unusedSaveSubName;
            }
        }
    }

    private void CheckIfSaveExists(int index)
    {
        doesTheSaveExist = saves[index] != null;
        if (doesTheSaveExist)
        {
            doesTheSaveExist = saves[index].scores != null; //you set this shit to null then for some reason it later becomes non null WTF
        }
    }

    public void OpenMenu()
    {
        isOpen = true;
        saveIndex = -1;
        thisCanvas.enabled = isOpen;
    }
    public void CloseMenu() //called by the "go back" button
    {
        isOpen = false;
        saveIndex = -1;
        thisCanvas.enabled = isOpen;
        //open the main menu here [!!!]
    }
    public void ChangeSaveIndex(int index)
    {
        saveIndex = index;
        CheckSaveAndRefresh();
    }
    public void SelectSave()
    {
        if(saveIndex >= 0)
        {
            CheckIfSaveExists(saveIndex);
            if (doesTheSaveExist)
            {
                Debug.Log("Placeholder event: Continue suffering from where you left!");
                //Load the save and level !!!
            }
            else
            {
                //create save !!!
                saves[saveIndex] = defaultValueSave;
                CheckSaveAndRefresh();
                Debug.Log("Placeholder event: no save! so a new one shall be created! woohoo!!!");
                //load level !!!
            }
        }
        else
        {
            Debug.Log("There is no save currently selected!");
        }
    }
    public void DeleteSave()
    {
        CheckIfSaveExists(saveIndex);
        if (saveIndex >= 0 && doesTheSaveExist)
        {
            Debug.Log("Placeholder event: Sadge!");
            CheckSaveAndRefresh();
            saves[saveIndex] = null;
        }
        else
        {
            Debug.Log("There is no save currently selected! (or it doesn't even exist)");
        }
        CheckSaveAndRefresh();
    }
}
