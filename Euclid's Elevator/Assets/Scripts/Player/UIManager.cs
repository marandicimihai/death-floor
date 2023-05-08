using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIParent;
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject journalUI;
    [SerializeField] ActionBar bar;

    private void Start()
    {
        GameManager.Instance.OnPause += (sender, args) =>
        {
            if (args.UI)
            {
                TurnOffUI();
            }
        };
        GameManager.Instance.OnUnpause += (sender, args) => TurnOnUI();
    }

    public void EnterJournalView()
    {
        crosshairUI.SetActive(false);
        inventoryUI.SetActive(false);
        dialogueUI.SetActive(false);
        bar.hideInfo = true;
    }   
    
    public void OpenJournalUI()
    {
        journalUI.SetActive(true);
    }

    public void ExitJournalView()
    {
        crosshairUI.SetActive(true);
        inventoryUI.SetActive(true);
        dialogueUI.SetActive(true);
        journalUI.SetActive(false);
        bar.hideInfo = false;
    }

    void TurnOffUI()
    {
        UIParent.SetActive(false);
    }
    
    void TurnOnUI()
    {
        UIParent.SetActive(true);
    }
}