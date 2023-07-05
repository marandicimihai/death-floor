using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SideMenu : MonoBehaviour
{
    Animator thisAnim;
    Canvas thisCanvas;
    [SerializeField] bool isDeathMenu;
    [SerializeField] GameObject resumeBut;
    [SerializeField] GameObject restartBut;
    [SerializeField] Image pauseSprite;
    [SerializeField] Image gameoverSprite;
    
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.enabled = false;

        PauseGame.Instance.OnTogglePause += (bool value) => OpenMenu(value);
        GameManager.Instance.OnGameOver += (object caller, System.EventArgs args) =>
        {
            isDeathMenu = true;
            OpenMenu(true);
        };
    }

    public void SetMenuType(bool type)
    {
        isDeathMenu = type;
        Refresh();
    }

    void Refresh()
    {
        restartBut.SetActive(isDeathMenu);
        resumeBut.SetActive(!isDeathMenu);
        gameoverSprite.enabled = isDeathMenu;
        pauseSprite.enabled = !isDeathMenu;
    }

    public void OpenMenu(bool value)
    {
        Refresh();
        if (value)
        {
            thisAnim.SetTrigger("SlideIn");
        }
        else
        {
            MenuSettings.Instance.OpenSettings(value);
            thisAnim.SetTrigger("SlideOut");
        }
    }

    public void SetCanvas(int mode)
    {
        if(mode > 0)
        {
            thisCanvas.enabled = true;
        }
        else
        {
            thisCanvas.enabled = false;
        }
    }

    public void OpenSettings(bool value)
    {
        OpenMenu(!value);
        MenuSettings.Instance.OpenSettings(value);
    }

    public void Resume()
    {
        OpenMenu(false);
        PauseGame.Instance.Unpause();
    }

    public void Restart()
    {
        //do restart !
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
