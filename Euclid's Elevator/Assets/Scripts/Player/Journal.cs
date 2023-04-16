using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Journal : MonoBehaviour
{
    [SerializeField] float openJournalUITime;
    [SerializeField] float flipPageTime;
    [SerializeField] Animator journal;
    [SerializeField] UIManager UIManager;
    [SerializeField] Transform leftPage;
    [SerializeField] Transform rightPage;
    [SerializeField] Text leftPageNumber;
    [SerializeField] Text rightPageNumber;
    [SerializeField] List<JournalPage> pages;

    int currentPage;
    bool inJournalView;
    bool journalUIOn;
    bool canFlip;

    private void Start()
    {
        GameManager.Instance.playerController.PlayerInputActions.General.Journal.performed += ToggleJournal;
        GameManager.Instance.playerController.PlayerInputActions.General.PageRight.performed += NextPage;
        GameManager.Instance.playerController.PlayerInputActions.General.PageLeft.performed += PreviousPage;
        canFlip = true;
        currentPage = 1;
    }

    #region JournalView

    void ToggleJournal(InputAction.CallbackContext context)
    {
        if (inJournalView)
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
        if (GameManager.Paused || inJournalView)
            return;

        inJournalView = true;

        SoundManager.Instance.PlaySound("JournalOpen");
        journal.SetBool("Open", true);
        UIManager.EnterJournalView();

        StartCoroutine(WaitAndExec(openJournalUITime, () =>
        {
            UIManager.OpenJournalUI();
            RefreshView();
            journalUIOn = true;
            GameManager.Instance.Pause();
        }));
    }

    public void ExitJournalView()
    {
        if (!inJournalView || !journalUIOn)
            return;

        journalUIOn = false;
        inJournalView = false;

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
        if (!inJournalView || !canFlip)
            return;

        if (currentPage + 2 <= pages.Count)
        {
            currentPage += 2;
            SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3"});
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
        if (!inJournalView || !canFlip)
            return;

        if (currentPage - 2 >= 1)
        {
            currentPage -= 2;
            SoundManager.Instance.PlaySound(new string[3] { "JournalFlip1", "JournalFlip2", "JournalFlip3" });
        }

        canFlip = false;
        StartCoroutine(WaitAndExecRT(flipPageTime, () =>
        {
            canFlip = true;
        }));

        RefreshView();
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
