using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuSettings : MonoBehaviour
{
    public static MenuSettings Instance;

    [Header("Page Paramaters")]
    [SerializeField] bool displayPages;
    [SerializeField] [Range(0, 2)] int pageIndex;
    [SerializeField] string[] pagesNames = new string[3];
    [SerializeField] Sprite[] pageIcons = new Sprite[3];
    [SerializeField] Sprite[] pageIconsHL = new Sprite[3]; //HL stands for highlighted
    [SerializeField] GameObject[] pages = new GameObject[3];
    [SerializeField] Text pageDisplayText;
    //[SerializeField] Image pageDisplayIcon;
    [SerializeField] Button pageDisplayButton;
    [Header("Audio")]
    [SerializeField] [Range(0f, 1.0f)] float effectsVolume = 1;
    [SerializeField] [Range(0f, 1.0f)] float ambianceVolume = 1;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider ambianceSlider;
    [Header("Graphics")]
    [SerializeField] string[] qualityNames;
    [SerializeField] int qualityIndex;
    [SerializeField] Res[] resType;
    [SerializeField] int resIndex;
    [SerializeField] bool isfullscreen = true;
    [SerializeField] bool vSync;
    [SerializeField] bool bloom; 
    [SerializeField] bool blur; //i bet a lot of people will turn this one off
    [SerializeField] Text textQuality;
    [SerializeField] Text textRes;
    [SerializeField] Image imageBoxVsync;
    [SerializeField] Image imageBoxFullscreen;
    [SerializeField] Image imageBoxBloom;
    [SerializeField] Image imageBoxBlur;
    [Header("Input")]
    [SerializeField] string[] textDisplay; //how should the text be displayed for the input buttons
    [SerializeField] Text[] inputText; //inputText's length shall be the same as textDisplay's length
    [SerializeField] [Range(0.1f, 2.0f)] float mouseSensitivity = 1;
    [SerializeField] Slider mouseSensSlider;
    [Header("Other")]
    [SerializeField] string click;
    [SerializeField] bool canvasOn;
    public bool refreshTest = false; //use this "trigger" to update the settings UI;
    bool valuesLoaded;
    [SerializeField] Animator thisAnim;
    [SerializeField] Canvas thisCanvas;
    //Settings set; 

    [Serializable] 
    public class Res
    {
        public int width = 1600;
        public int height = 900;
    }
    private void OnValidate()
    {
        if(inputText.Length != textDisplay.Length)
        {
            Debug.LogError("inputText is not matching the length of textDisplay!!!");
        }
    }

    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        Refresh();
        valuesLoaded = true;
    }
    void Update()
    {
        if (refreshTest)
        {
            refreshTest = false;
            Refresh();
        }
    }
    //UI MANAGMENT
    public void OpenFatherMenu()
    {
        if (FindObjectOfType<MainMenu>())
        {
            MainMenu mm = FindObjectOfType<MainMenu>();
            mm.OpenMainTab(true);
            mm.OpenSettings(false);
        }
        else if(FindObjectOfType<SideMenu>())
        {
            SideMenu sm = FindObjectOfType<SideMenu>();
            sm.OpenSettings(false);
        }
        else
        {
            Debug.LogError("Hey dumb ass, you forgot to add the main menu");
        }
    }
    void ChangePageTo(int index)
    {
        if(index >= pages.Length || index < 0)
        {
            index = 0;
            Debug.LogWarning("page index was set beyond the available size!");
        }
        if (displayPages)
        {
            pages[pageIndex].SetActive(false);
            pageIndex = index;
            pages[pageIndex].SetActive(true);
        }
        else
        {
            SetPagesOff(); //just to make sure
        }
        pageDisplayText.text = pagesNames[pageIndex];
        pageDisplayButton.GetComponent<Image>().sprite = pageIcons[pageIndex];
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = pageIconsHL[pageIndex];
        pageDisplayButton.spriteState = ss;
    }
    public void ChangePageNext()
    {
        pages[pageIndex].SetActive(false);
        pageIndex++;
        if(pageIndex >= pages.Length)
        {
            pageIndex = 0;
        }
        pages[pageIndex].SetActive(true);
        pageDisplayText.text = pagesNames[pageIndex];
        pageDisplayButton.GetComponent<Image>().sprite = pageIcons[pageIndex];
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = pageIconsHL[pageIndex];
        pageDisplayButton.spriteState = ss;
        UnselectButton();
    }
    public void SetPagesOff()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }
    public void OpenSettings(bool value)
    {
        thisAnim.SetBool("isOpen", value);
        displayPages = value;
        thisAnim.SetTrigger("fuckThis");
        Refresh();
    }
    public void SetCanvasOn() //the fucking animator gotta act special and not accept boolians, kids these days! 
    {
        canvasOn = true;
        thisCanvas.enabled = canvasOn;
    }
    public void SetCanvasOff()
    {
        canvasOn = false;
        thisCanvas.enabled = canvasOn;
    }
    //AUDIO
    void UpdateAudio()
    {
        effectsVolume = SaveSystem.Instance.LoadSettings().EffectsVolume;
        ambianceVolume = SaveSystem.Instance.LoadSettings().AmbienceVolume;
        effectsSlider.value = effectsVolume;
        ambianceSlider.value = ambianceVolume;
    }
    public void SyncVolume() //Syncs the stored values of the sound volume with the slider values
    {
        effectsVolume = effectsSlider.value;
        ambianceVolume = ambianceSlider.value;
        SaveAudio();
    }
    public void SaveAudio()
    {
        if (!valuesLoaded)
            return;

        Settings old = SaveSystem.Instance.LoadSettings();
        Settings newTop = new Settings(effectsVolume, ambianceVolume); //goes on top

        SaveSystem.Instance.SaveSettings(old + newTop);
    }
    //GRAPHICS
    void UpdateGraphics()
    {
        qualityIndex = SaveSystem.Instance.LoadSettings().QualityIndex;
        resIndex = SaveSystem.Instance.LoadSettings().ResIndex;
        vSync = SaveSystem.Instance.LoadSettings().VSync;
        isfullscreen = SaveSystem.Instance.LoadSettings().Fullscreen;
        bloom = SaveSystem.Instance.LoadSettings().Bloom;
        blur = SaveSystem.Instance.LoadSettings().Blur;


        textQuality.text = qualityNames[qualityIndex];
        QualitySettings.SetQualityLevel(qualityIndex);
        string width = resType[resIndex].width.ToString();
        string height = resType[resIndex].height.ToString();
        textRes.text = width + "x" + height;
        Screen.SetResolution(resType[resIndex].width, resType[resIndex].height, isfullscreen);
        QualitySettings.vSyncCount = vSync.GetHashCode();
        Screen.fullScreen = isfullscreen;
        
        imageBoxVsync.enabled = vSync;
        imageBoxFullscreen.enabled = isfullscreen;
        imageBoxBloom.enabled = bloom;
        imageBoxBlur.enabled = blur;
    }
    public void NextQualityIndex()
    {
        qualityIndex++;
        if(qualityIndex >= qualityNames.Length)
        {
            qualityIndex = 0;
        }
        textQuality.text = qualityNames[qualityIndex];
        UnselectButton();
    }
    public void NextResIndex()
    {
        resIndex++;
        if (resIndex >= resType.Length)
        {
            resIndex = 0;
        }
        string width = resType[resIndex].width.ToString();
        string height = resType[resIndex].height.ToString();
        textRes.text = width + "x" + height;
        UnselectButton();
    }
    public void SwitchVsync()
    {
        vSync = !vSync;
        imageBoxVsync.enabled = vSync;
    }
    public void SwitchFullscreen()
    {
        isfullscreen = !isfullscreen;
        imageBoxFullscreen.enabled = isfullscreen;
    }
    public void SwitchBloom()
    {
        bloom = !bloom;
        imageBoxBloom.enabled = bloom;
    }
    public void SwitchBlur()
    {
        blur = !blur;
        imageBoxBlur.enabled = blur;
    }
    public void ApplyAndSaveGraphics()
    {
        if (!valuesLoaded)
            return;

        Settings old = SaveSystem.Instance.LoadSettings();
        Settings newTop = new Settings(bloom, blur, resIndex, resType[resIndex].width, resType[resIndex].height, qualityIndex, vSync, isfullscreen); //goes on top
        SaveSystem.Instance.SaveSettings(old + newTop);
    }
    //INPUT
    void UpdateInput()
    {
        mouseSensitivity = SaveSystem.Instance.LoadSettings().Sensitivity;
        //also the same for other INPUTS (that currently can not be changed >:\ ) !!!!

        for(int i = 0; i < inputText.Length; i++)
        {
            inputText[i].text = textDisplay[i];
        }
        mouseSensSlider.value = mouseSensitivity;
    }
    public void SyncSens() //whenever the slider gets input; 
    {
        mouseSensitivity = mouseSensSlider.value;
        SaveInput();
    }
    //BRO DO THE INPUT BROOOOOOO!!!! pls!
    public void SaveInput()
    {
        if (!valuesLoaded)
            return;

        Settings old = SaveSystem.Instance.LoadSettings();
        Settings newTop = new Settings(mouseSensitivity);
        SaveSystem.Instance.SaveSettings(old + newTop);
    }

    //OTHER
    public void DoClick() //make a clicking sound
    {
        AudioManager.Instance.PlayClip(click);
    }
    void UnselectButton() //the buttons stay selected after being clicked thus the highlights no longer work, so yeah unselecting is the only way
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    //Will recheck everything and apply changes (such as should the menu be open or what text should the UI display)
    //it also sets the values of every parameter to the saved ones
    //but if the saved settings are NULL, no where to be found, gone... it will save at the end thus creating new saved settings
    void Refresh() 
    {
        SetPagesOff();
        UpdateAudio();
        UpdateGraphics();
        UpdateInput();
        ChangePageTo(pageIndex);
        thisCanvas.enabled = canvasOn;
    }
}
