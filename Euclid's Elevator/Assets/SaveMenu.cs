using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveMenu : MonoBehaviour
{
    [SerializeField] bool isOpen;
    [SerializeField] Canvas thisCanvas;
    [SerializeField] Text[] saveTitles = new Text[2]; //the save title in the top right corner
    [SerializeField] Text[] saveSimpleTitles; //aka. the cabinet text;
    [SerializeField] GameObject scoreAll; //to disable and enable them visually
    [SerializeField] Text[] scores; 
    [SerializeField] Button DeleteBut; //to be able to disable it;
    [SerializeField] string unusedSaveName = "Empty Save #"; // + number added after
    [SerializeField] string unusedSaveNameSimple = "Empty";
    [SerializeField] string usedSaveName = "Save #";  // + number added after
    [SerializeField] int saveIndex = -1; //which save is currently selected; -1 = none
    [SerializeField] SaveExample[] saves;
    public class SaveExample //this is an example of what values/parameters i want out of a save;
    {
        public bool used; //is this save real? or used?
        public float timeScore; //time spent playing in said save, store it in seconds;
        public float deathScore; //nr. of deaths
        public float winScore; //how many times has the player beaten the game in the said slot
        public float sanityScore; //the amount of total loss of sanity. so you add up every percent of lost sanity to this score;
    }
    void Start()
    {
        if(thisCanvas != null)
        {
            thisCanvas = GetComponent<Canvas>();
        }
        thisCanvas.enabled = isOpen;
    }
    public void OpenMenu()
    {
        isOpen = true;
        thisCanvas.enabled = isOpen;
    }
    public void CloseMenu() //called by the "go back" button
    {
        isOpen = false;
        thisCanvas.enabled = isOpen;
        //open the main menu here!!!
    }
    public void ChangeSaveIndex(int index)
    {
        saveIndex = index;
        //do stuff!!!
    }
}
