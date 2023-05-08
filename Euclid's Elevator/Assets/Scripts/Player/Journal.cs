using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Journal : MonoBehaviour
{
    [Header("Journal")]
    [SerializeField] AnimationClip openJournal;
    [SerializeField] AnimationClip closeJournal;
    [SerializeField] float flipPageTime;
    [SerializeField] float holdTime;
    [SerializeField] Animator journal;
    [SerializeField] UIManager UIManager;
    [Header("Pages")]
    [SerializeField] Transform leftPage;
    [SerializeField] Transform rightPage;
    [SerializeField] Text leftPageNumber;
    [SerializeField] Text rightPageNumber;
    [Header("Sliders")]
    [SerializeField] GameObject sliderobjleft;
    [SerializeField] GameObject sliderobjright;
    [SerializeField] Image leftSlider;
    [SerializeField] Image rightSlider;
    [Header("Pages")]
    [SerializeField] List<JournalPage> pages;

    int currentPage;
    public bool InJournalView { get; private set; }
    bool journalUIOn;
    bool canFlip;
    bool exitQueued;

    string inputname;
    float startTime;
    bool holding;

    private void Start()
    {
        GameManager.Instance.playerController.PlayerInputActions.General.Journal.performed += ToggleJournal;
        GameManager.Instance.playerController.PlayerInputActions.General.PageRight.started += (InputAction.CallbackContext context) => 
        {
            inputname = context.action.name;
            startTime = Time.unscaledTime;
            StartCoroutine(HoldSlider(inputname));
        };
        GameManager.Instance.playerController.PlayerInputActions.General.PageLeft.started += (InputAction.CallbackContext context) =>
        {
            inputname = context.action.name;
            startTime = Time.unscaledTime;
            StartCoroutine(HoldSlider(inputname));
        };
        GameManager.Instance.playerController.PlayerInputActions.General.PageRight.canceled += NextPage;
        GameManager.Instance.playerController.PlayerInputActions.General.PageLeft.canceled += PreviousPage;
        canFlip = true;
        currentPage = 1;
    }

    #region JournalView

    void ToggleJournal(InputAction.CallbackContext context)
    {
        if (InJournalView)
        {
            ExitJournalView();
        }
        else
        {
            EnterJournalView();
        }
    }

    void EnterJournalView()
    {
        if (GameManager.Paused || InJournalView || GameManager.Instance.playerController.Dead)
            return;

        GameManager.Instance.Pause();
        InJournalView = true;

        journal.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound("JournalOpen", true);
        journal.SetBool("Open", true);
        UIManager.EnterJournalView();

        StartCoroutine(WaitAndExecRT(openJournal.length, () =>
        {
            journalUIOn = true;

            if (exitQueued)
                return;

            UIManager.OpenJournalUI();
            RefreshView();
        }));
    }

    public void ExitJournalView()
    {
        if (!InJournalView || !journalUIOn)
            return;

        journalUIOn = false;

        StartCoroutine(WaitAndExecRT(closeJournal.length, () =>
        {
            journal.gameObject.SetActive(false);
            InJournalView = false;
        }));
        SoundManager.Instance.PlaySound("JournalClose");
        journal.SetBool("Open", false);
        UIManager.ExitJournalView();

        GameManager.Instance.Unpause();
    }

    void RefreshView()
    {
        for (int i = 0; i < leftPage.childCount; i++)
        {
            Destroy(leftPage.GetChild(i).gameObject);
        }
        for (int i = 0; i < rightPage.childCount; i++)
        {
            Destroy(rightPage.GetChild(i).gameObject);
        }
        if (currentPage - 1 < pages.Count && pages[currentPage - 1] != null)
            Instantiate(pages[currentPage - 1].pageUIPrefab, leftPage.position, leftPage.rotation, leftPage);
        if (currentPage < pages.Count && pages[currentPage] != null)
            Instantiate(pages[currentPage].pageUIPrefab, rightPage.position, rightPage.rotation, rightPage);

        leftPageNumber.text = currentPage.ToString();
        rightPageNumber.text = (currentPage + 1).ToString();
    }

    public IEnumerator CallExitWhenAvailable()
    {
        if (exitQueued)
            yield break;

        exitQueued = true;

        yield return new WaitUntil(() =>
        {
            return !(!InJournalView || !journalUIOn);
        });

        if (!exitQueued)
            yield break;

        exitQueued = false;

        ExitJournalView();
    }

    public void CancelExitCall()
    {
        exitQueued = false;
    }

    #endregion

    public void AddPage(JournalPage page)
    {
        foreach (JournalPage p in pages)
        {
            if (p.name == page.name)
                return;
        }

        pages.Add(page);
        SoundManager.Instance.PlaySound("JournalScribble");

        RefreshView();
    }

    void NextPage(InputAction.CallbackContext context)
    {
        holding = false;

        if (!InJournalView || !canFlip)
            return;

        if (Time.unscaledTime - startTime > holdTime && inputname == context.action.name)
        {
            if (pages.Count % 2 == 0)
            {
                currentPage = pages.Count - 1;
            }
            else
            {
                currentPage = pages.Count;
            }
            SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3" });
        }
        else
        {
            if (currentPage + 2 <= pages.Count)
            {
                currentPage += 2;
                SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3"});
            }
        }

        canFlip = false;
        StartCoroutine(WaitAndExecRT(flipPageTime, () =>
        {
            canFlip = true;
        }));

        RefreshView();
    }

    void PreviousPage(InputAction.CallbackContext context)
    {
        holding = false;

        if (!InJournalView || !canFlip)
            return;

        if (Time.unscaledTime - startTime > holdTime && inputname == context.action.name)
        {
            currentPage = 1;
            SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3" });
        }
        else
        {
            if (currentPage - 2 >= 1)
            {
                currentPage -= 2;
                SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3" });
            }
        }

        canFlip = false;
        StartCoroutine(WaitAndExecRT(flipPageTime, () =>
        {
            canFlip = true;
        }));

        RefreshView();
    }

    IEnumerator HoldSlider(string inputName)
    {
        if (inputName == "PageRight")
        {
            sliderobjright.SetActive(true);
            sliderobjleft.SetActive(false);
        }
        else if (inputName == "PageLeft")
        {
            sliderobjright.SetActive(false);
            sliderobjleft.SetActive(true);
        }
        holding = true;
        yield return new WaitUntil(() =>
        {
            if (inputName == "PageRight")
            {
                rightSlider.fillAmount = (Time.unscaledTime - startTime) / holdTime;
            }
            else if (inputName == "PageLeft")
            {
                leftSlider.fillAmount = (Time.unscaledTime - startTime) / holdTime;
            }
            return !holding;
        });

        sliderobjright.SetActive(false);
        sliderobjleft.SetActive(false);
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }

    IEnumerator WaitAndExecRT(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSecondsRealtime(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}
