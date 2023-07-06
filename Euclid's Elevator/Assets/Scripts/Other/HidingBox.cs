using UnityEngine.InputSystem;
using UnityEngine;

public class HidingBox : MonoBehaviour
{
    public bool hasPlayer;
    [SerializeField] Animator animator;
    [SerializeField] Transform cameraRoot;

    bool playerIn;

    private void Start()
    {
        Input.InputActions.Box.ExitBox.performed += (InputAction.CallbackContext context) => ExitBox();
        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
        {
            animator.SetTrigger("Reset");
        };
    }

    public void EnterBox(Player player)
    {
        if (hasPlayer || playerIn)
            return;

        hasPlayer = true;
        cameraRoot.localRotation = Quaternion.LookRotation(Vector3.Scale(player.transform.forward, new Vector3(1, 0, 1)));
        animator.SetTrigger("GetIn");
    }

    public void ExitBox()
    {
        if (!hasPlayer || !playerIn)
            return;

        hasPlayer = false;
        animator.SetTrigger("GetOut");
    }

    public void TriggerDeath(Vector3 enemyPos)
    {
        if (!hasPlayer)
            return;

        cameraRoot.localRotation = Quaternion.LookRotation(Vector3.Scale(enemyPos - cameraRoot.position, new Vector3(1, 0, 1)));
        hasPlayer = false;

        animator.SetTrigger("Death");
    }

    public void PlayerIn()
    {
        playerIn = true;
    }

    public void PlayerOut()
    {
        playerIn = false;
    }
}
