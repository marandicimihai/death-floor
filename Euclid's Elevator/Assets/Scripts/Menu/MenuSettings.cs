using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuSettings : MonoBehaviour
{
    //"It's been a worthwile since i last scripted for a unity project" - Dev #2
    public bool refreshTest = false; //use this "trigger" to update the settings UI;
    Animator thisAnim;
    [Header("Page Paramaters")]
    Canvas thisCanvas;
    [SerializeField] bool displayPages;
    [SerializeField] bool canvasOn;
    [SerializeField] [Range(0, 2)] int pageIndex;
    [SerializeField] string[] pagesNames;
    [SerializeField] Sprite[] pageIcons;
    [SerializeField] GameObject[] pages;
    [SerializeField] Text pageDisplayText;
    [SerializeField] Image pageDisplayIcon;
    [Header("Audio")]
    [SerializeField] [Range(0f, 1.0f)] float effectsVolume = 1;
    [SerializeField] [Range(0f, 1.0f)] float ambianceVolume = 1;
    //these are referenced incase we want to change the sliders without the user;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider ambianceSlider;
    [Header("Graphics")]
    [SerializeField] string[] qualityNames;
    [SerializeField] int qualityIndex;
    [SerializeField] Res[] resType;
    [SerializeField] int resIndex;
    [SerializeField] bool isfullscreen = true;
    [SerializeField] string[] aliasNames;
    [SerializeField] int aliasIndex; //0 is false (AKA none)
    [SerializeField] bool vSync;
    [SerializeField] bool bloom; 
    [SerializeField] bool blur; //i bet a lot of people will turn this one off
    [SerializeField] Text textQuality;
    [SerializeField] Text textRes;
    [SerializeField] Text textAlias;
    //reference here "ticked boxes" imagies;
    [SerializeField] Image ImageBoxVsync;
    [SerializeField] Image ImageBoxFullscreen;
    [SerializeField] Image ImageBoxBloom;
    [SerializeField] Image ImageBoxBlur;
    [Header("Input")]
    [SerializeField] string[] textDisplay; //how should the text be displayed for the input buttons
    [SerializeField] Text[] inputText; //the length of textDisplay must match the length of inputText
    [SerializeField] [Range(0.1f, 2.0f)] float mouseSensitivity = 1;
    [SerializeField] Slider mouseSensSlider;

    Settings set;

    [Serializable] 
    public class Res
    {
        public int width = 1600;
        public int height = 900;
    }

    void Start()
    {
        thisAnim = gameObject.GetComponent<Animator>();
        SetPagesOff();
        UpdateAudio();
        UpdateGraphics();
        UpdateInput();
        ChangePageTo(pageIndex);
        thisCanvas = gameObject.GetComponent<Canvas>();
        thisCanvas.enabled = canvasOn;
    }

    public void ApplySettings(Settings set)
    {
        SetEffectsVolume(set.effectsVolume);
        SetAmbianceVolume(set.ambianceVolume);
        SetSens(set.sensitivity);
        SetBloom(set.bloom);
        SetBlur(set.blur);
        SetResIndex(set.resIndex);
        SetQualityIndex(set.qualityIndex);
        SetVsync(set.vSync);
        SetFullscreen(set.fullScreen);
    }

    void Update()
    {
        testRefresh();
    }
    void DoThatThing() //it's kinda annoying how the fucking buttons are just staying selected, what's this dumm ass feature
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    void testRefresh()
    {
        if (refreshTest)
        {
            SetPagesOff();
            refreshTest = false;
            UpdateAudio();
            UpdateGraphics();
            UpdateInput();
            ChangePageTo(pageIndex);
            thisCanvas.enabled = canvasOn;
        }
    }
    //OTHER
    public void OpenFatherMenu()
    {
        set = new Settings(effectsVolume, ambianceVolume, bloom, blur, mouseSensitivity, resIndex, qualityIndex, vSync, isfullscreen);
        SaveSystem.SaveSettings(set);
        if (FindObjectOfType<MainMenu>())
        {
            MainMenu mm = FindObjectOfType<MainMenu>();
            mm.OpenMainTab(true);
            mm.OpenSettings(false);
        }
        else
        {
            Debug.LogError("Hey dumb ass, you forgot to add the main menu");
        }
    }
    public void ChangePageTo(int index)
    {

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
        pageDisplayIcon.sprite = pageIcons[pageIndex];
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
        pageDisplayIcon.sprite = pageIcons[pageIndex];
        DoThatThing();
    }
    public void SetPagesOff()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }
    public void SetDisplayPages(bool value)
    {
        displayPages = true;
        ChangePageTo(pageIndex);
    }
    public void OpenSettings(bool value)
    {
        thisAnim.SetBool("isOpen", value);
        displayPages = value;
        thisAnim.SetTrigger("fuckThis");
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
        effectsSlider.value = effectsVolume;
        ambianceSlider.value = ambianceVolume;
    }
    public void SyncVolume()
    {
        effectsVolume = effectsSlider.value;
        ambianceVolume = ambianceSlider.value;
    }
    public void SetEffectsVolume(float vol)
    {
        effectsVolume = vol;
        effectsSlider.value = effectsVolume;
    }
    public void SetAmbianceVolume(float vol)
    {
        ambianceVolume = vol;
        ambianceSlider.value = ambianceVolume;
    }
    public float GetEffectsVolume()
    {
        return effectsVolume;
    }
    public float GetAmbianceVolume()
    {
        return ambianceVolume;
    }
    //GRAPHICS
    void UpdateGraphics()
    {
        textQuality.text = qualityNames[qualityIndex];
        string width = resType[resIndex].width.ToString();
        string height = resType[resIndex].height.ToString();
        textRes.text = width + "x" + height;
        textAlias.text = aliasNames[aliasIndex];
        ImageBoxVsync.enabled = vSync;
        ImageBoxFullscreen.enabled = isfullscreen;
        ImageBoxBloom.enabled = bloom;
        ImageBoxBlur.enabled = blur;
    }
    public void SetQualityIndex(int index)
    {
        qualityIndex = index;
        textQuality.text = qualityNames[qualityIndex];
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void NextQualityIndex()
    {
        qualityIndex++;
        if(qualityIndex >= qualityNames.Length)
        {
            qualityIndex = 0;
        }
        textQuality.text = qualityNames[qualityIndex];
        QualitySettings.SetQualityLevel(qualityIndex);
        DoThatThing();
    }
    public void SetResArray(Res[] newResArray) //idk if u need this...
    {
        resType = newResArray;
        string width = resType[resIndex].width.ToString();
        string height = resType[resIndex].height.ToString();
        textRes.text = width + "x" + height;
    }
    public void SetResIndex(int index)
    {
        resIndex = index;
        string width = resType[resIndex].width.ToString();
        string height = resType[resIndex].height.ToString();
        textRes.text = width + "x" + height;
        Screen.SetResolution(resType[resIndex].width, resType[resIndex].height, isfullscreen);
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
        Screen.SetResolution(resType[resIndex].width, resType[resIndex].height, isfullscreen);
        DoThatThing();
    }
    public void SetAliasIndex(int index)
    {
        aliasIndex = index;
        textAlias.text = aliasNames[aliasIndex];
    }
    public void NextAliasIndex()
    {
        aliasIndex++;
        if (aliasIndex >= aliasNames.Length)
        {
            aliasIndex = 0;
        }
        textAlias.text = aliasNames[aliasIndex];
        DoThatThing();
    }
    public void SetVsync(bool newVsync)
    {
        vSync = newVsync;
        ImageBoxVsync.enabled = vSync;
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    public void SwitchVsync()
    {
        vSync = !vSync;
        ImageBoxVsync.enabled = vSync;
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    public void SetFullscreen(bool newFullscreen)
    {
        isfullscreen = newFullscreen;
        ImageBoxFullscreen.enabled = isfullscreen;
        Screen.fullScreen = isfullscreen;
    }
    public void SwitchFullscreen()
    {
        isfullscreen = !isfullscreen;
        ImageBoxFullscreen.enabled = isfullscreen;
        Screen.fullScreen = isfullscreen;
    }
    public void SetBloom(bool newBloom)
    {
        bloom = newBloom;
        ImageBoxBloom.enabled = bloom;
    }
    public void SwitchBloom()
    {
        bloom = !bloom;
        ImageBoxBloom.enabled = bloom;
    }
    public void SetBlur(bool newBlur)
    {
        blur = newBlur;
        ImageBoxBlur.enabled = blur;
    }
    public void SwitchBlur()
    {
        blur = !blur;
        ImageBoxBlur.enabled = blur;
    }
    public int GetQualityIndex()
    {
        return qualityIndex;
    }
    public int GetResIndex()
    {
        return resIndex;
    }
    public int GetAliasIndex()
    {
        return aliasIndex;
    }
    public bool GetVsync()
    {
        return vSync;
    }
    public bool GetFullscreen()
    {
        return isfullscreen;
    }
    public bool GetBloom()
    {
        return bloom;
    }
    public bool GetBlur()
    {
        return blur;
    }

    //Input
    void UpdateInput()
    {
        for(int i = 0; i < inputText.Length; i++)
        {
            inputText[i].text = textDisplay[i];
        }
        mouseSensSlider.value = mouseSensitivity;
    }
    public void SetDisplayKey(int index, string newDisplayText)
    {
        textDisplay[index] = newDisplayText;
        inputText[index].text = textDisplay[index];
    }
    public void SetSens(float newSens) //0.1 to 2
    {
        mouseSensitivity = newSens;
        mouseSensSlider.value = mouseSensitivity;
    }
    public void SyncSens() //whenever the slider gets input;
    {
        mouseSensitivity = mouseSensSlider.value;
    }
    //idk what to do here with Input so yeah, you do you;
}
