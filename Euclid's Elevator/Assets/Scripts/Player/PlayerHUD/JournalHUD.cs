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
        if (journal != null)
        {
            journal.OnPagesChanged += (object caller, System.EventArgs args) => RefreshPages();
        }
        else
        {
            Debug.Log("No journal.");
        }
    }

    private void Update()
    {
        UpdatePageHUD();
    }

    void UpdatePageHUD()
    {
        if (damageLevels.Length == 0)
        {
            return;
        }

        JournalDamageLevel highestAvailableLevel = damageLevels[0];

        foreach (JournalDamageLevel level in damageLevels)
        {
            if (level.insanityPercentage > highestAvailableLevel.insanityPercentage)
            {
                if (insanity != null)
                {
                    if (level.insanityPercentage <= insanity.InsanityValue)
                    {
                        highestAvailableLevel = level;
                    }
                }
                else
                {
                    Debug.Log("No insanity.");
                }
            }
        }

        journalBook.sprite = highestAvailableLevel.sprite;
    }

    void RefreshPages()
    {
        if (journal == null)
        {
            Debug.Log("No journal.");
            return;
        }

        DestroyChildren(leftPage);
        DestroyChildren(rightPage);

        leftPageNumber.text = journal.Page.ToString();
        rightPageNumber.text = (journal.Page + 1).ToString();

        if (journal.Pages.Count >= 1)
        {
            Instantiate(journal.Pages[journal.Page - 1].pageHUDPrefab, leftPage);
        }
        if (journal.Pages.Count > journal.Page)
        {
            Instantiate(journal.Pages[journal.Page].pageHUDPrefab, rightPage);
        }
    }

    void DestroyChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    public void HideHUD(bool value) => book.SetActive(!value);
}
