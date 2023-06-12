using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Journal : MonoBehaviour
{
    public EventHandler OnPagesChanged;
    public List<JournalPage> pages;
    public int page;

    [SerializeField] Player player;
    [SerializeField] Animator journalAnimator;
    [SerializeField] float HUDDelay;

    bool open;

    private void Awake()
    {
        pages = new();
        page = 1;
    }

    private void Start()
    {
        Input.InputActions.General.Journal.performed += ToggleJournal;
        Input.InputActions.General.PageLeft.performed += PageLeft;
        Input.InputActions.General.PageRight.performed += PageRight;
    }

    void ToggleJournal(InputAction.CallbackContext context)
    {
        if (player.Dead)
        {
            return;
        }

        open = !open;
        journalAnimator.SetBool("Open", open);
        OnPagesChanged?.Invoke(this, new EventArgs());
        player.HUDManager.ToggleJournalView(open, HUDDelay);

        if (open)
        {
            PauseGame.Instance.Pause();
        }
        else
        {
            PauseGame.Instance.Unpause();
        }
    }

    void PageRight(InputAction.CallbackContext context)
    {
        if (open)
        {
            if (pages.Count >= page + 2)
            {
                page += 2;
                OnPagesChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    void PageLeft(InputAction.CallbackContext context)
    {
        if (open)
        {
            if (page >= 3)
            {
                page -= 2;
                OnPagesChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public void AddPage(JournalPage newPage)
    {
        if (!pages.Contains(newPage))
        {
            pages.Add(newPage);
        }
    }
}
