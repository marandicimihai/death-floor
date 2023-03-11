using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked;

    [SerializeField] bool open;
    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;

    [SerializeField] ItemObject requiredItem;

    float t;

    private void Update()
    {
        if (open)
        {
            t += 1 / openTime * Time.deltaTime;
        }
        else
        {
            t -= 1 / openTime * Time.deltaTime;
        }
        t = Mathf.Clamp01(t);
        panel.localRotation = Quaternion.Slerp(Quaternion.Euler(0, closedYRot, 0), Quaternion.Euler(0, openedYRot, 0), t);
    }

    private void OnValidate()
    {
        if (open)
        {
            panel.localRotation = Quaternion.Euler(0, openedYRot, 0);
        }
        else
        {
            panel.localRotation = Quaternion.Euler(0, closedYRot, 0);
        }
    }

    public bool Toggle(ItemObject requirement = null)
    {
        if (requiredItem != null && requirement != null && requirement.itemName == requiredItem.itemName)
            locked = false;

        if (locked)
            return false;

        GameManager.instance.enemyController.NoiseHeardNav(transform.position);

        open = !open;
        doorHandle.SetTrigger("PullHandle");

        return true;
    }

    public void ForceOpen()
    {
        locked = false;
        Toggle();
    }
}
