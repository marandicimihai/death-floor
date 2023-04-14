using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField] List<JournalPage> pages;

    int currentPage;
    bool inJournalView;

    private void Start()
    {
        GameManager.Instance.playerController.PlayerInputActions.General.Journal.performed += ToggleJournal;
        GameManager.Instance.playerController.PlayerInputActions.General.PageRight.performed += NextPage;
        GameManager.Instance.playerController.PlayerInputActions.General.PageLeft.performed += PreviousPage;
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

        GameManager.Instance.Pause();
    }

    public void ExitJournalView()
    {
        if (!inJournalView)
            return;

        inJournalView = false;

        GameManager.Instance.Unpause();
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
    }

    void NextPage(InputAction.CallbackContext context)
    {
        if (!inJournalView)
            return;

        if (currentPage + 2 <= pages.Count)
        {
            currentPage += 2;
        }
    }

    void PreviousPage(InputAction.CallbackContext context)
    {
        if (!inJournalView)
            return;

        if (currentPage - 2 >= 1)
        {
            currentPage -= 2;
        }
    }
}
