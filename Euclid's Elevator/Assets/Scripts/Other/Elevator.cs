using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool Open { get; private set; }
    public bool Broken { get; private set; }
    [SerializeField] new Collider collider;
    [SerializeField] Animator elevator;
    [SerializeField] ItemObject keyCard;

    private void Awake()
    {
        collider.enabled = false;
    }

    public void OpenElevator()
    {
        collider.enabled = false;
        elevator.SetBool("Open", true);
        Open = true;
    }

    public void CloseElevator()
    {
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
        else if (Broken)
        {
            LineManager.instance.SayLine("Elevator Broken");
        }
        return false;
    }

    public void Repair()
    {
        Broken = false;
    }

    public void BreakDown()
    {
        Broken = true;
    }
}
