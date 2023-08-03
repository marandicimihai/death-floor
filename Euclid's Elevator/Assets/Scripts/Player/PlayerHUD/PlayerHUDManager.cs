using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] ActionInfoHUD actionInfo;
    [SerializeField] InventoryHUD inventoryHUD;
    [SerializeField] DialogueHUD dialogueHUD;
    [SerializeField] PopUpHUD popupHUD;
    [SerializeField] GameObject crosshair;
    [SerializeField] JournalHUD journalHUD;

    float journalHUDTime;
    float journalTimeElapsed;
    bool inJournalView;

    private void Awake()
    {
        DefaultHUD();
        HideCursor();
    }

    private void Start()
    {
        if (PauseGame.Instance != null)
        {
            PauseGame.Instance.OnPause += (object caller, System.EventArgs args) => ShowCursor();
            PauseGame.Instance.OnUnPause += (object caller, System.EventArgs args) => HideCursor();
        }
        else
        {
            Debug.Log("No pause class.");
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += (object caller, System.EventArgs args) =>
            {
                ShowCursor();
                HideAllHUD();
            };
            GameManager.Instance.OnGameEnd += (object caller, System.EventArgs args) =>
            {
                Cursor.lockState = CursorLockMode.None;
                ShowCursor();
                HideAllHUD();
            };
        }
        else
        {
            Debug.Log("No game manager.");
        }
    }

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

    public void ToggleJournalView(bool value, float delay)
    {
        journalTimeElapsed = 0;
        if (value)
        {
            actionInfo.HideHUD(true);
            inventoryHUD.HideHUD(true);
            dialogueHUD.HideHUD(true);
            popupHUD.HideHUD(true);
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
        actionInfo.HideHUD(false);
        inventoryHUD.HideHUD(false);
        dialogueHUD.HideHUD(false);
        journalHUD.HideHUD(true);
        popupHUD.HideHUD(false);
        crosshair.SetActive(true);
    }

    public void HideAllHUD()
    {
        actionInfo.HideHUD(true);
        inventoryHUD.HideHUD(true);
        journalHUD.HideHUD(true);
        dialogueHUD.HideHUD(true);
        popupHUD.HideHUD(true);
        crosshair.SetActive(false);
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
