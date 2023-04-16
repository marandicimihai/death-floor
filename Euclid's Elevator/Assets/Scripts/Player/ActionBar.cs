using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionBar : MonoBehaviour
{
    public bool hideInfo;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject sliderObject;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text action;

    bool isActive;

    private void Awake()
    {
        StopAction();
    }

    private void Start()
    {
        GameManager.MakePausable(this);
    }

    private void Update()
    {
        if (hideInfo)
        {
            UI.SetActive(false);
        }
        else
        {
            UI.SetActive(true);
        }
    }

    public void StartAction()
    {
        if (isActive)
            return;

        if (hideInfo)
        {
            UI.SetActive(false);
            return;
        }

        slider.value = 0;
        sliderObject.SetActive(true);
        isActive = true;
    }

    public void SetSliderValue(float percentage)
    {
        if (!isActive)
            return;

        if (hideInfo)
        {
            UI.SetActive(false);
            return;
        }

        slider.value = percentage;
    }

    public void StopAction()
    {
        if (hideInfo)
        {
            UI.SetActive(false);
            return;
        }

        sliderObject.SetActive(false);
        isActive = false;
    }

    public void SetActionText(string text)
    {
        if (hideInfo)
        {
            UI.SetActive(false);
            return;
        }

        action.text = text;
    }
}