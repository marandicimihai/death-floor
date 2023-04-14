using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool Open { get; private set; }
    public bool Broken { get; private set; }
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
}
