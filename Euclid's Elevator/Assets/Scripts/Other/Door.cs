using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    public bool open;

    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;
    [SerializeField] bool locked;

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
        panel.rotation = Quaternion.Slerp(Quaternion.Euler(0, closedYRot, 0), Quaternion.Euler(0, openedYRot, 0), t);
    }

    private void OnValidate()
    {
        if (open)
        {
            panel.rotation = Quaternion.Euler(0, openedYRot, 0);
        }
        else
        {
            panel.rotation = Quaternion.Euler(0, closedYRot, 0);
        }
    }

    public bool Toggle(ItemObject requirement = null)
    {
        if (requiredItem != null && requirement != null && requirement.itemName == requiredItem.itemName)
            locked = false;

        if (locked)
            return false;

        if (GameManager.instance.enemy.TryGetComponent(out Enemy enemy))
            enemy.NoiseHeardNav(transform.position);

        open = !open;
        doorHandle.SetTrigger("PullHandle");

        return true;
    }
}
