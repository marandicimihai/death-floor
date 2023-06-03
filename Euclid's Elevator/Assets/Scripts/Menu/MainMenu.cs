using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainTab;
    [SerializeField] Canvas creditsTab;
    [SerializeField] Animator menuCameraAnim;
    [SerializeField] AudioClip clickingSound;
    AudioSource thisSource;
    MenuSettings settingsTab;
    // Start is called before the first frame update
    void Start()
    {
        thisSource = GetComponent<AudioSource>();
        settingsTab = FindObjectOfType<MenuSettings>();
        mainTab.enabled = true;
        creditsTab.enabled = false;
        settingsTab.SetPagesOff();
        settingsTab.SetCanvasOff();
        Settings set = SaveSystem.LoadSettings();
        if (set != null)
        {
            settingsTab.ApplySettings(set);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenMainTab(bool open)
    {
        mainTab.enabled = open;
    }
    public void OpenCreditsTab(bool open)
    {
        creditsTab.enabled = open;
        menuCameraAnim.SetBool("Credits", open);
    }
    public void OpenSettings(bool open)
    {
        Settings set = SaveSystem.LoadSettings();
        if (set != null)
        {
            settingsTab.ApplySettings(set);
        }
        settingsTab.OpenSettings(open);
        menuCameraAnim.SetBool("Settings", open);
    }
    public void QuitGame()
    {
        Debug.Log("this is it, my final message... [dies]");
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void DoClick()
    {
        thisSource.clip = clickingSound;
        thisSource.Play();
    }
}
