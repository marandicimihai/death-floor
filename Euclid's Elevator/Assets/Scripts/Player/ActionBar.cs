using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionBar : MonoBehaviour
{
    [SerializeField] GameObject sliderObject;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text action;

    bool isActive;

    private void Awake()
    {
        StopAction();
    }

    public void StartAction()
    {
        if (isActive)
            return;

        slider.value = 0;
        sliderObject.SetActive(true);
        isActive = true;
    }

    public void SetSliderValue(float percentage)
    {
        if (!isActive)
            return;

        slider.value = percentage;
    }

    public void StopAction()
    {
        sliderObject.SetActive(false);
        isActive = false;
    }

    public void SetActionText(string text)
    {
        action.text = text;
    }
}