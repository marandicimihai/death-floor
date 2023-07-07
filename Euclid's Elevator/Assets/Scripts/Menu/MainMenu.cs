using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainTab;
    [SerializeField] Canvas creditsTab;
    [SerializeField] Animator menuCameraAnim;
    [SerializeField] float ambienceFadeTime;
    [SerializeField] string click;
    [SerializeField] string airhum;
    [SerializeField] string lightbuzz;

    AudioJob humJob;
    AudioJob buzzJob;

    MenuSettings settingsMenu;
    void Start()
    {
        Time.timeScale = 1;

        settingsMenu = FindObjectOfType<MenuSettings>();
        mainTab.enabled = true;
        creditsTab.enabled = false;
        settingsMenu.SetPagesOff();
        settingsMenu.SetCanvasOff();

        humJob = AudioManager.Instance.PlayClip(airhum);
        humJob.DDOL();
        buzzJob = AudioManager.Instance.PlayClip(lightbuzz);
        buzzJob.DDOL();
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
        AudioManager.Instance.FadeAwayClip(humJob, ambienceFadeTime);
        AudioManager.Instance.FadeAwayClip(buzzJob, ambienceFadeTime);
        SceneManager.LoadScene("Main");
    }
    public void DoClick()
    {
        AudioManager.Instance.PlayClip(click);
    }
}
