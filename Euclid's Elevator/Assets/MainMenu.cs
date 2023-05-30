using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainTab;
    [SerializeField] Canvas creditsTab;
    [SerializeField] Animator menuCameraAnim;
    MenuSettings settingsTab;
    // Start is called before the first frame update
    void Start()
    {
        settingsTab = FindObjectOfType<MenuSettings>();
        mainTab.enabled = true;
        creditsTab.enabled = false;
        settingsTab.SetPagesOff();
        settingsTab.SetCanvasOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openMainTab(bool open)
    {
        mainTab.enabled = open;
    }
    public void openCreditsTab(bool open)
    {
        creditsTab.enabled = open;
        menuCameraAnim.SetBool("Credits", open);
    }
    public void openSettings(bool open)
    {
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
        Debug.Log("bzzt loading vrrrr");
    }

}
