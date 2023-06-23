using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool Broken { get; private set; }
    [SerializeField] Player player;
    [SerializeField] ItemProperties keycard;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] Collider doorCollider;
    [SerializeField] float waitForPlayerRadius;
    [SerializeField] float elevatorRideTime;
    [SerializeField] float elevatorAccelTime;

    [Header("Sounds")]
    [SerializeField] string close;
    [SerializeField] string open;
    [SerializeField] string move1;
    [SerializeField] string move2;
    [SerializeField] string move3;
    [SerializeField] string move4;
    [SerializeField] string stop;
    [SerializeField] string repair;

    AudioJob movejob;
    Animator animator;

    bool waiting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
        {
            Broken = true;
            CloseElevator(true);
            InitiateElevatorRide();
        };

        InitiateElevatorRide();
    }

    private void Update()
    {
        if (!Broken && waiting && Vector3.Distance(transform.position, player.transform.position) <= waitForPlayerRadius)
        {
            GameManager.Instance.PlayerEnteredElevator();

            CloseElevator();
            InitiateElevatorRide();

            waiting = false;
        }
    }

    public bool TryInsert(Player player)
    {
        if (player.inventory.Items[player.inventory.Index] != null)
        {
            if (Broken)
            {
                bool b = player.inventory.Items[player.inventory.Index].properties.name == toolbox.name;
                if (b)
                {
                    Broken = false;
                    AudioManager.Instance.PlayClip(repair);
                    player.inventory.DecreaseDurability(player.inventory.Index);
                }
                return b;
            }

            bool a = player.inventory.Items[player.inventory.Index].properties.name == keycard.name;
            if (a)
            {
                InsertKeycard();
                player.inventory.DecreaseDurability(player.inventory.Index);
            }
            return a;
        }
        return false;
    }

    public bool MatchesRequirement(Player player)
    {
        if (player.inventory.Items[player.inventory.Index] != null)
        {
            if (Broken)
            {
                return player.inventory.Items[player.inventory.Index].properties.name == toolbox.name;
            }
            return player.inventory.Items[player.inventory.Index].properties.name == keycard.name;
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
        if (GameManager.Instance.Stage == 1)
        {
            movejob = AudioManager.Instance.PlayClip(move1);
        }
        else if (GameManager.Instance.Stage == 2)
        {
            movejob = AudioManager.Instance.PlayClip(move2);
        }
        else if (GameManager.Instance.Stage == 3)
        {
            movejob = AudioManager.Instance.PlayClip(move3);
        }
        else if (GameManager.Instance.Stage == 4)
        {
            movejob = AudioManager.Instance.PlayClip(move4);
        }
        Invoke(nameof(OpenElevator), elevatorRideTime);
        player.vfxmanager.CameraShake(AnimationAction.FadeAppear, elevatorAccelTime);
        Invoke(nameof(DeaccelerateElevator), elevatorRideTime - elevatorAccelTime);
    }

    void DeaccelerateElevator()
    {
        player.vfxmanager.CameraShake(AnimationAction.FadeDisappear, elevatorAccelTime);
        movejob.StopPlaying();
        AudioManager.Instance.PlayClip(stop);
    }

    void OpenElevator()
    {
        AudioManager.Instance.PlayClip(open);
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
        else
        {
            AudioManager.Instance.PlayClip(close);
        }
        doorCollider.enabled = true;
    }
}
