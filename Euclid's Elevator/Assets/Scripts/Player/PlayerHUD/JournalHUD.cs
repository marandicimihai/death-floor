using UnityEngine.UI;
using UnityEngine;
using System;

[System.Serializable]
struct JournalDamageLevel
{
    public Sprite sprite;
    public float insanityPercentage;
}

public class JournalHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [SerializeField] Journal journal;
    [SerializeField] Insanity insanity;
    [SerializeField] GameObject book;
    [SerializeField] Transform leftPage;
    [SerializeField] Transform rightPage;
    [SerializeField] Text leftPageNumber;
    [SerializeField] Text rightPageNumber;

    [SerializeField] Image journalBook;
    [SerializeField] JournalDamageLevel[] damageLevels;


    private void Awake()
    {
        journal.OnPagesChanged += (object caller, System.EventArgs args) => RefreshPages();
        insanity.OnInsanityChanged += UpdatePageHUD;
    }

    private void Update()
    {
        book.SetActive(!hideHUD);
    }

    void UpdatePageHUD(InsanityArgs args)
    {
        if (damageLevels.Length == 0)
        {
            return;
        }

        JournalDamageLevel highestAvailableLevel = damageLevels[0];

        foreach (JournalDamageLevel level in damageLevels)
        {
            if (level.insanityPercentage > highestAvailableLevel.insanityPercentage && level.insanityPercentage <= args.insanity)
            {
                highestAvailableLevel = level;
            }
        }

        journalBook.sprite = highestAvailableLevel.sprite;
    }

    void RefreshPages()
    {
        DestroyChildren(leftPage);
        DestroyChildren(rightPage);

        leftPageNumber.text = journal.page.ToString();
        rightPageNumber.text = (journal.page + 1).ToString();

        if (journal.pages.Count >= 1)
        {
            Instantiate(journal.pages[journal.page - 1].pageHUDPrefab, leftPage);
        }
        if (journal.pages.Count > journal.page)
        {
            Instantiate(journal.pages[journal.page].pageHUDPrefab, rightPage);
        }
    }

    void DestroyChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
