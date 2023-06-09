using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum SliderType
{
    Circle,
    Lock,
    Unlock
}

public class ActionInfoHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject circleSliderObject;
    [SerializeField] Image circleSlider;
    [SerializeField] GameObject lockSliderObject;
    [SerializeField] Image lockSlider;
    [SerializeField] GameObject unlockSliderObject;
    [SerializeField] Image unlockSlider;

    object currentUsing;
    bool active;
    GameObject activeSliderObject;
    Image activeSlider;

    private void Update()
    {
        if (hideHUD)
        {
            SetChildrenActive(false);
        }
        else
        {
            SetChildrenActive(true);
        }

        if (active)
        {
            text.text = string.Empty;
        }
    }

    public void SetActionText(string text)
    {
        if (!active)
        {
            this.text.text = text;
        }
    }

    public void StartAction(SliderType type, object caller)
    {
        if (active)
            return;

        if (type == SliderType.Circle)
        {
            activeSliderObject = circleSliderObject;
            activeSlider = circleSlider;
        }
        else if (type == SliderType.Lock)
        {
            activeSliderObject = lockSliderObject;
            activeSlider = lockSlider;
        }
        else if (type == SliderType.Unlock)
        {
            activeSliderObject = unlockSliderObject;
            activeSlider = unlockSlider;
        }

        currentUsing = caller;
        activeSliderObject.SetActive(true);
        activeSlider.fillAmount = 0;

        active = true;
    }

    public void SetSliderValue(float value, object caller)
    {
        if (!active || currentUsing != caller)
            return;

        activeSlider.fillAmount = value;
    }

    public void StopAction(object caller)
    {
        if (active && currentUsing == caller)
        {
            active = false;
            activeSliderObject.SetActive(false);
        
            activeSliderObject = null;
            activeSlider = null;

            currentUsing = null;
        }
    }

    void SetChildrenActive(bool active)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
