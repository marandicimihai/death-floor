using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ItemProperties keycard;
    [SerializeField] Collider doorCollider;
    [SerializeField] float waitForPlayerRadius;
    [SerializeField] float elevatorRideTime;

    Animator animator;

    bool waiting;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Invoke(nameof(OpenElevator), elevatorRideTime);
    }

    private void Update()
    {
        if (waiting && Vector3.Distance(transform.position, player.transform.position) <= waitForPlayerRadius)
        {
            GameManager.Instance.PlayerEnteredElevator();

            CloseElevator();
            Invoke(nameof(OpenElevator), elevatorRideTime);

            waiting = false;
        }
    }

    public bool CheckItem(ItemProperties req)
    {
        if (req.name == keycard.name)
        {
            InsertKeycard();
            return true;
        }
        return false;
    }

    void InsertKeycard()
    {
        GameManager.Instance.KeycardInserted();
        waiting = true;
    }

    void OpenElevator()
    {
        animator.SetBool("Open", true);
        doorCollider.enabled = false;
    }
    
    void CloseElevator()
    {
        animator.SetBool("Open", false);
        doorCollider.enabled = true;
    }
}
