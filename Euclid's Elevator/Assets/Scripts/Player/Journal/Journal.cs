using System.Collections.Generic;
using UnityEngine.InputSystem;
using DeathFloor.SaveSystem;
using UnityEngine;
using System;

public class Journal : MonoBehaviour, ISaveData<JournalData>
{
    public EventHandler OnPagesChanged;
    public int Page { get => page; }
    public List<JournalPage> Pages { get => pages; private set => pages = value; }
    public bool CanSave => true;

    [SerializeField] PlayerHUDManager hud;
    [SerializeField] Animator journalAnimator;
    [SerializeField] float HUDDelay;

    [Header("Sounds")]
    [SerializeField] string openJournal;
    [SerializeField] string closeJournal;
    [SerializeField] string scribble;
    [SerializeField] string[] pageFlip;

    List<JournalPage> pages;
    int page;
    bool open;

    private void Awake()
    {
        Pages = new();
        page = 1;
    }

    private void Start()
    {
        Input.Instance.InputActions.Realtime.Journal.performed += ToggleJournal;
        Input.Instance.InputActions.Realtime.PageLeft.performed += PageLeft;
        Input.Instance.InputActions.Realtime.PageRight.performed += PageRight;

        journalAnimator.gameObject.SetActive(open);
    }

    public void OnFirstTimeLoaded()
    {

    }

    public JournalData OnSaveData()
    {
        return new JournalData(Pages);
    }

    public void LoadData(JournalData data)
    {
        Pages = data.Pages;
    }

    void ToggleJournal(InputAction.CallbackContext context)
    {
        open = !open;
        journalAnimator.gameObject.SetActive(true);
        journalAnimator.SetBool("Open", open);
        OnPagesChanged?.Invoke(this, new EventArgs());

        if (hud != null)
        {
            hud.ToggleJournalView(open, HUDDelay);
        }
        else
        {
            Debug.Log("No hud class.");
        }

        if (open)
        {
            AudioManager.Instance.PlayClip(openJournal);
        }
        else
        {
            AudioManager.Instance.PlayClip(closeJournal);
        }
    }

    public void CloseJournal()
    {
        if (open)
        {
            ToggleJournal(new InputAction.CallbackContext());
        }
    }

    void PageRight(InputAction.CallbackContext context)
    {
        if (open)
        {
            if (Pages.Count >= page + 2)
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
        if (newPage == null)
            return;

        if (!Pages.Contains(newPage))
        {
            Pages.Add(newPage);
            AudioManager.Instance.PlayClip(scribble);
        }
    }
}
