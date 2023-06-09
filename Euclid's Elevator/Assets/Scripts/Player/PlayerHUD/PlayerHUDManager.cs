using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    public bool hideHUD;
    public ActionInfoHUD actionInfo;
    [SerializeField] InventoryHUD inventoryHUD;
    [SerializeField] GameObject crosshair;

    private void Update()
    {
        actionInfo.hideHUD = hideHUD;
        inventoryHUD.hideHUD = hideHUD;
        crosshair.SetActive(!hideHUD);
    }
}
