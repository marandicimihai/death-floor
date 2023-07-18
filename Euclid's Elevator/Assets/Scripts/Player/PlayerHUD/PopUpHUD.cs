using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopUpHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [SerializeField] RectTransform rect;
    [SerializeField] GameObject popUp;
    [SerializeField] float timePerPopUp;
    [SerializeField] float slideOutTime;

    List<RectTransform> popups;
    List<string> used;
    float slideTimeElapsed;
    float timeElapsed;

    bool slideIn;

    Vector3 center;

    private void Awake()
    {
        center = new Vector3(-rect.rect.width / 2, rect.rect.height / 2, 0);
        popups = new();
        used = new();
    }

    private void Start()
    {
        if (SaveSystem.Instance.currentSaveData != null &&
            SaveSystem.Instance.currentSaveData.usedPopUps.Length > 0)
        {
            used = SaveSystem.Instance.currentSaveData.usedPopUps.ToList();
        }
        SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            data.usedPopUps = used.ToArray();
        };
    }

    private void Update()
    {
        if (hideHUD)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                popups[i].gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                popups[i].gameObject.SetActive(true);
            }
        }

        if (transform.childCount > 0)
        {
            if (slideIn)
            {
                PerformSlideIn();
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= timePerPopUp)
                {
                    PerformSlide();
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        popups[i].localPosition = new Vector3(center.x, -rect.rect.height * i + rect.rect.height / 2, 0);
                    }
                }
            }
        }
        else
        {
            slideTimeElapsed = 0;
            timeElapsed = 0;
            slideIn = false;
        }
    }

    void PerformSlide()
    {
        slideTimeElapsed += Time.deltaTime;
        popups[0].localPosition = Vector3.Lerp(center, new Vector3(rect.rect.width, center.y, 0), slideTimeElapsed / slideOutTime);
        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                popups[i].localPosition = Vector3.Lerp(new Vector3(center.x, -rect.rect.height * i, 0), new Vector3(center.x, -rect.rect.height * (i - 1) + rect.rect.height / 2, 0), slideTimeElapsed / slideOutTime);
            }
        }

        if (slideTimeElapsed >= slideOutTime)
        {
            Destroy(popups[0].gameObject);
            popups.Remove(popups[0]);
            slideTimeElapsed = 0;
            timeElapsed = 0;
        }
    }

    void PerformSlideIn()
    {
        slideIn = true;
        slideTimeElapsed += Time.deltaTime;
        popups[0].localPosition = Vector3.Lerp(new Vector3(center.x, -center.y, 0), new Vector3(center.x, center.y, 0), slideTimeElapsed / slideOutTime);
        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                popups[i].localPosition = Vector3.Lerp(new Vector3(center.x, -rect.rect.height * (i + 1), 0), new Vector3(center.x, -rect.rect.height * i + rect.rect.height / 2, 0), slideTimeElapsed / slideOutTime);
            }
        }

        if (slideTimeElapsed >= slideOutTime)
        {
            slideTimeElapsed = 0;
            timeElapsed = 0;
            slideIn = false;
        }
    }

    public void PopUp(PopUpProperties popUp)
    {
        if ((used.Contains(popUp.name) && popUp.oneTime) || popUp == null)
        {
            return;
        }

        GameObject newpop = Instantiate(this.popUp, this.transform);
        popups.Add(newpop.GetComponent<RectTransform>());
        
        if (popups.Count == 1)
        {
            slideTimeElapsed = 0;
            slideIn = true;
        }

        if (newpop.TryGetComponent(out PopUp popUpScript))
        {
            if (popUp.oneTime)
            {
                used.Add(popUp.name);
            }
            popUpScript.AssignProperties(popUp);
        }
    }
}
