using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Journal : MonoBehaviour
{
    public EventHandler OnPagesChanged;
    public int Page { get => page; }
    public List<JournalPage> Pages { get => pages; }

    [SerializeField] JournalPage[] scriptableObjects;
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
        pages = new();
        page = 1;
    }

    private void Start()
    {
        if (Input.InputActions != null)
        {
            Input.InputActions.Realtime.Journal.performed += ToggleJournal;
            Input.InputActions.Realtime.PageLeft.performed += PageLeft;
            Input.InputActions.Realtime.PageRight.performed += PageRight;
        }
        else
        {
            Debug.Log("Input class absent");
        }

        journalAnimator.gameObject.SetActive(open);

        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.currentSaveData != null &&
                SaveSystem.Instance.currentSaveData.pages.Length > 0)
            {
                foreach(string name in SaveSystem.Instance.currentSaveData.pages)
                {
                    pages.Add(GetPage(name));
                }
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            List<string> names = new();

            foreach (JournalPage page in pages)
            {
                names.Add(page.name);
            }

            data.pages = names.ToArray();
        };
        }
        else
        {
            Debug.Log("No save system.");
        }
    }

    JournalPage GetPage(string name)
    {
        foreach (JournalPage page in scriptableObjects)
        {
            if (page.name == name)
            {
                return page;
            }
        }
        return null;
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
        if (newPage == null)
            return;

        if (!pages.Contains(newPage))
        {
            pages.Add(newPage);
            AudioManager.Instance.PlayClip(scribble);
        }
    }
}
