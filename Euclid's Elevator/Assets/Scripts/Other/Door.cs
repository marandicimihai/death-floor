using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open;

    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;
    [SerializeField] bool locked;

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

    public bool Toggle()
    {
        if (locked)
            return false;

        open = !open;
        doorHandle.SetTrigger("PullHandle");

        return true;
    }
}
