using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

[Serializable]
struct SliderStruct
{
    public string name;
    public GameObject obj;
    public Image fillImg;
}

public class ActionBar : MonoBehaviour
{
    public bool hideInfo;

    [SerializeField] GameObject UI;
    [SerializeField] TMP_Text action;
    [SerializeField] SliderStruct[] sliders;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        StopActions();
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

    public void StartAction(string name)
    {
        if (IsActive)
            return;

        SliderStruct slider = Array.Find(sliders, (SliderStruct current) => current.name == name);

        if (slider.name == string.Empty)
        {
            Debug.Log($"Couldn't find slider with name {name}");
        }

        slider.fillImg.fillAmount = 0;
        slider.obj.SetActive(true);
        IsActive = true;
    }

    public void SetSliderValue(string name, float amount)
    {
        if (!IsActive)
            return;

        SliderStruct slider = Array.Find(sliders, (SliderStruct current) => current.name == name);

        if (slider.name == string.Empty)
        {
            Debug.Log($"Couldn't find slider with name {name}");
        }

        slider.fillImg.fillAmount = amount;
    }

    public void StopAction(string name)
    {
        SliderStruct slider = Array.Find(sliders, (SliderStruct current) => current.name == name);

        if (slider.name == string.Empty)
        {
            Debug.Log($"Couldn't find slider with name {name}");
        }

        slider.obj.SetActive(false);
        IsActive = false;
    }

    public void StopActions()
    {
        foreach(SliderStruct slider in sliders)
        {
            slider.obj.SetActive(false);
        }
        IsActive = false;
    }

    public void SetActionText(string text)
    {
        action.text = text;
    }
}