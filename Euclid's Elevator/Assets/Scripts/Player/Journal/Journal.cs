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

    [Header("Sounds")]
    [SerializeField] string openJournal;
    [SerializeField] string closeJournal;
    [SerializeField] string scribble;
    [SerializeField] string[] pageFlip;

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
            AudioManager.Instance.PlayClip(openJournal);
        }
        else
        {
            PauseGame.Instance.Unpause();
            AudioManager.Instance.PlayClip(closeJournal);
        }
    }

    void PageRight(InputAction.CallbackContext context)
    {
        if (open)
        {
            if (pages.Count >= page + 2)
            {
                page += 2;
                AudioManager.Instance.PlayRandomClip(pageFlip);
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
                AudioManager.Instance.PlayRandomClip(pageFlip);
                OnPagesChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public void AddPage(JournalPage newPage)
    {
        if (!pages.Contains(newPage))
        {
            pages.Add(newPage);
            AudioManager.Instance.PlayClip(scribble);
        }
    }
}