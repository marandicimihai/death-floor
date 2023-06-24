using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    public ActionInfoHUD actionInfo;
    public InventoryHUD inventoryHUD;
    [SerializeField] GameObject crosshair;
    [SerializeField] JournalHUD journalHUD;
    [SerializeField] DialogueHUD dialogueHUD;

    float journalHUDTime;
    float journalTimeElapsed;
    bool inJournalView;

    private void Update()
    {
        if (inJournalView)
        {
            journalTimeElapsed += Time.unscaledDeltaTime;
            if (journalTimeElapsed >= journalHUDTime)
            {
                journalHUD.hideHUD = false;
            }
        }
    }

    private void Awake()
    {
        DefaultHUD();
    }

    private void Start()
    {
        PauseGame.Instance.OnPause += (object caller, System.EventArgs args) =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        };
        PauseGame.Instance.OnUnPause += (object caller, System.EventArgs args) =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        };
    }

    public void ToggleJournalView(bool value, float delay)
    {
        journalTimeElapsed = 0;
        if (value)
        {
            actionInfo.hideHUD = true;
            inventoryHUD.hideHUD = true;
            dialogueHUD.hideHUD = true;
            crosshair.SetActive(false);
            journalHUDTime = delay;
        }
        else
        {
            DefaultHUD();
        }
        inJournalView = value;
    }

    public void DefaultHUD()
    {
        actionInfo.hideHUD = false;
        inventoryHUD.hideHUD = false;
        journalHUD.hideHUD = true;
        dialogueHUD.hideHUD = false;
        crosshair.SetActive(true);
    }

    public void HideAllHUD()
    {
        actionInfo.hideHUD = true;
        inventoryHUD.hideHUD = true;
        journalHUD.hideHUD = true;
        dialogueHUD.hideHUD = true;
        crosshair.SetActive(false);
    }
}
