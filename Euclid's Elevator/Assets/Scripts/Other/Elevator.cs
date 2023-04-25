using System.Collections;
using UnityEngine;
using System;

public class Elevator : MonoBehaviour
{
    public bool Open { get; private set; }
    public bool Broken { get; private set; }
    [SerializeField] AnimationClip close;
    [SerializeField] new Collider collider;
    [SerializeField] Animator elevator;
    [SerializeField] ItemObject keyCard;
    [SerializeField] ItemObject toolbox;

    private void Awake()
    {
        collider.enabled = false;
    }

    public void OpenElevator()
    {
        SoundManager.Instance.PlaySound("Hum");
        SoundManager.Instance.PlaySound("ElevatorOpen");
        collider.enabled = false;
        elevator.SetBool("Open", true);
        Open = true;
    }

    public void CloseElevator()
    {
        SoundManager.Instance.PlaySound("ElevatorClose");
        collider.enabled = true;
        elevator.SetBool("Open", false);
        Open = false;

        StartCoroutine(WaitAndExec(close.length, () =>
        {
            GameManager.Instance.ElevatorDoorClosed();
        }));
    }

    public bool InsertItem(ItemObject obj)
    {
        if (!Broken && obj.name == keyCard.name)
        {
            return true;
        }
        return false;
    }

    public bool Repair(ItemObject obj)
    {
        if (Broken && obj.name == toolbox.name)
        {
            Broken = false;
            return true;
        }

        return false;
    }

    public void BreakDown()
    {
        Broken = true;
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}
