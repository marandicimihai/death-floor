using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ItemProperties keycard;
    [SerializeField] Collider doorCollider;
    [SerializeField] float waitForPlayerRadius;
    [SerializeField] float elevatorRideTime;
    [SerializeField] float elevatorAccelTime;

    Animator animator;

    bool waiting;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        InitiateElevatorRide();
    }

    private void Start()
    {
        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
        {
            CloseElevator(true);
            InitiateElevatorRide();
        };
    }

    private void Update()
    {
        if (waiting && Vector3.Distance(transform.position, player.transform.position) <= waitForPlayerRadius)
        {
            GameManager.Instance.PlayerEnteredElevator();

            CloseElevator();
            InitiateElevatorRide();

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

    void InitiateElevatorRide()
    {
        Invoke(nameof(OpenElevator), elevatorRideTime);
        player.vfxmanager.CameraShake(AnimationAction.FadeAppear, elevatorAccelTime);
        Invoke(nameof(DeaccelerateElevator), elevatorRideTime - elevatorAccelTime);
    }

    void DeaccelerateElevator()
    {
        player.vfxmanager.CameraShake(AnimationAction.FadeDisappear, elevatorAccelTime);
    }

    void OpenElevator()
    {
        animator.SetBool("Open", true);
        doorCollider.enabled = false;
    }

    void CloseElevator(bool instant = false)
    {
        animator.SetBool("Open", false);
        if (instant)
        {
            animator.SetTrigger("InstaClose");
        }
        doorCollider.enabled = true;
    }
}
