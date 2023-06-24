using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SideMenu : MonoBehaviour
{
    Animator thisAnim;
    Canvas thisCanvas;
    [SerializeField] bool isDeathMenu;
    [SerializeField] GameObject resumeBut;
    [SerializeField] GameObject restartBut;
    [SerializeField] Image pauseSprite;
    [SerializeField] Image gameoverSprite;
    [SerializeField] bool open;
    [SerializeField] bool test;
    // "Start is called before the first frame update" WOAH REALLY!?
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            test = false;
            TEST();
        }

    }
    void TEST() //to test the sidemenu
    {
        OpenMenu(open);
    }
    public void SetMenuType(bool type) //0 - pause menu// 1 - death menu;
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
            open = true;
        }
        else
        {
            thisAnim.SetTrigger("SlideOut");
            open = false;
        }
    }
    public void SetCanvas(int mode)
    { //0 - false/1 - true
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
        FindObjectOfType<MenuSettings>().OpenSettings(value);
    }
    public void Resume()
    {
        OpenMenu(false);
        //do the resume !
    }
    public void Restart()
    {
        //do restart !
    }
    public void LoadMainMenu()
    {
        //do that !
    }
}
