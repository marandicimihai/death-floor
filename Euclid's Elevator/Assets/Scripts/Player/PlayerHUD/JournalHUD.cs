using UnityEngine.UI;
using UnityEngine;

public class JournalHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [SerializeField] Journal journal;
    [SerializeField] GameObject book;
    [SerializeField] Transform leftPage;
    [SerializeField] Transform rightPage;
    [SerializeField] Text leftPageNumber;
    [SerializeField] Text rightPageNumber;

    private void Awake()
    {
        journal.OnPagesChanged += (object caller, System.EventArgs args) => RefreshPages();
    }

    private void Update()
    {
        book.SetActive(!hideHUD);
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
