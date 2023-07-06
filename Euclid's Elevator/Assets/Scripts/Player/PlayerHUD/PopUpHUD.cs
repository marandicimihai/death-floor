using System.Collections.Generic;
using UnityEngine;

public class PopUpHUD : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] GameObject popUp;
    [SerializeField] float timePerPopUp;
    [SerializeField] float slideOutTime;

    List<RectTransform> popups;
    List<PopUpProperties> used;
    float slideTimeElapsed;
    float timeElapsed;

    private void Awake()
    {
        popups = new();
        used = new();
    }

    private void Update()
    {
        if (transform.childCount > 0)
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
                    popups[i].localPosition = new Vector3(-rect.rect.width / 2, -rect.rect.height * i + rect.rect.height / 2, 0);
                }
            }
        }
    }

    void PerformSlide()
    {
        slideTimeElapsed += Time.deltaTime;
        popups[0].localPosition = Vector3.Lerp(new Vector3(-rect.rect.width / 2, rect.rect.height / 2, 0), new Vector3(rect.rect.width, rect.rect.height / 2, 0), slideTimeElapsed / slideOutTime);
        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                popups[i].localPosition = Vector3.Lerp(new Vector3(-rect.rect.width / 2, -rect.rect.height * i, 0), new Vector3(-rect.rect.width / 2, -rect.rect.height * (i - 1) + rect.rect.height / 2, 0), slideTimeElapsed / slideOutTime);
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

    public void PopUp(PopUpProperties popUp)
    {
        if (used.Contains(popUp) && popUp.oneTime)
        {
            return;
        }

        GameObject newpop = Instantiate(this.popUp, this.transform);
        popups.Add(newpop.GetComponent<RectTransform>());
        
        if (newpop.TryGetComponent(out PopUp popUpScript))
        {
            if (popUp.oneTime)
            {
                used.Add(popUp);
            }
            popUpScript.AssignProperties(popUp);
        }
    }
}
