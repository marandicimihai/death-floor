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

        if (PauseGame.Instance != null)
        {
            PauseGame.Instance.OnTogglePause += (bool value) => OpenMenu(value);
        }
        else
        {
            Debug.Log("No pause game.");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWin += (object caller, System.EventArgs args) =>
            {
                SaveSystem.Instance.ClearData(0);
                SceneManager.LoadScene("Menu");
            };

            GameManager.Instance.OnGameOver += (object caller, System.EventArgs args) =>
            {
                isDeathMenu = true;
                OpenMenu(true);
            };
        }
        else
        {
            Debug.Log("No game manager.");
        }
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
            if (MenuSettings.Instance != null)
            {
                MenuSettings.Instance.OpenSettings(value);
            }
            else
            {
                Debug.Log("No settings menu.");
            }
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
        if (MenuSettings.Instance == null)
        {
            Debug.Log("No settings menu.");
            return;
        }

        OpenMenu(!value);
        MenuSettings.Instance.OpenSettings(value);
    }

    public void Resume()
    {
        OpenMenu(false);

        if (PauseGame.Instance == null)
        {
            Debug.Log("No pause class.");
        }
        else
        {
            PauseGame.Instance.Unpause();
        }
    }

    public void Restart()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.ClearData(0);
        }
        else
        {
            Debug.Log("No save system");
        }
        SceneManager.LoadScene("Main");
    }

    public void LoadMainMenu()
    {
        if (GameManager.Instance != null && SaveSystem.Instance != null)
        {
            if (GameManager.Instance.GameStage == GameStage.End)
            {
                SaveSystem.Instance.ClearData(0);
            }
            else
            {
                SaveSystem.Instance.SaveGame();
            }
        }
        else
        {
            Debug.Log("No game manager or save system.");
        }
        SceneManager.LoadScene("Menu");
    }
}
