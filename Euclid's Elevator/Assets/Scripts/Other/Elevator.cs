using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool Open { get; private set; }
    [SerializeField] new Collider collider;
    [SerializeField] Animator elevator;

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
}
