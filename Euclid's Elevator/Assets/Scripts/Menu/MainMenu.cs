using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainTab;
    [SerializeField] Canvas creditsTab;
    [SerializeField] Animator menuCameraAnim;
    [SerializeField] string click;
    MenuSettings settingsMenu;
    void Start()
    {
        settingsMenu = FindObjectOfType<MenuSettings>();
        mainTab.enabled = true;
        creditsTab.enabled = false;
        settingsMenu.SetPagesOff();
        settingsMenu.SetCanvasOff();
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
        /*
        Settings set = SaveSystem.LoadSettings();
        if (set != null)
        {
            settingsMenu.ApplySettings(set);
        }
        */
        settingsMenu.OpenSettings(open);
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
        AudioManager.Instance.PlayClip(click);
    }
}
