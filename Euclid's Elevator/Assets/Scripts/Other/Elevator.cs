using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool Broken { get; private set; }
    [SerializeField] Player player;
    [SerializeField] ItemProperties keycard;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] Collider doorCollider;
    [SerializeField] Transform waitForPlayerCenter;
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
    [SerializeField] string insert;

    AudioJob movejob;
    Animator animator;

    bool riding;
    bool waiting;
    bool canClose;
    bool elevatorDoorClosed;

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

        if (SaveSystem.Instance.currentSaveData != null)
        {
            if (SaveSystem.Instance.currentSaveData.stage < 0)
            {
                InitiateElevatorRide();
            }
            else
            {
                OpenElevator(true);
            }
            Broken = SaveSystem.Instance.currentSaveData.broken;
            waiting = SaveSystem.Instance.currentSaveData.waiting;
            canClose = SaveSystem.Instance.currentSaveData.canClose;
        }

        SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            data.broken = Broken;
            data.waiting = waiting;
            if (waiting)
            {
                data.canClose = true;
            }
            SaveSystem.Instance.CanSave = !riding;
        };
    }

    private void Update()
    {
        if (!Broken && waiting && Vector3.Distance(waitForPlayerCenter.position, player.transform.position) <= waitForPlayerRadius)
        {
            doorCollider.enabled = true;
            if (canClose)
            {
                CloseElevator();
                canClose = false;
            }
            if (elevatorDoorClosed)
            {
                GameManager.Instance.ElevatorRideInitialized();

                InitiateElevatorRide();

                waiting = false;
            }
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
                animator.SetTrigger("Keycard");
                AudioManager.Instance.PlayClip(insert);
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
        riding = true;
        if (player.Deaths == 0)
        {
            movejob = AudioManager.Instance.PlayClip(move1);
        }
        else if (player.Deaths == 1)
        {
            movejob = AudioManager.Instance.PlayClip(move2);
        }
        else if (player.Deaths == 2)
        {
            movejob = AudioManager.Instance.PlayClip(move3);
        }
        else if (player.Deaths == 3)
        {
            movejob = AudioManager.Instance.PlayClip(move4);
        }
        Invoke(nameof(OpenElevatorWithAnimation), elevatorRideTime);
        player.vfxmanager.CameraShake(AnimationAction.FadeAppear, elevatorAccelTime);
        Invoke(nameof(DeaccelerateElevator), elevatorRideTime - elevatorAccelTime);
    }

    void DeaccelerateElevator()
    {
        player.vfxmanager.CameraShake(AnimationAction.FadeDisappear, elevatorAccelTime);
        movejob.StopPlaying();
        AudioManager.Instance.PlayClip(stop);
        riding = false;
    }

    void OpenElevatorWithAnimation()
    {
        animator.SetBool("Open", true);
        AudioManager.Instance.PlayClip(open);
        doorCollider.enabled = false;
    }

    void OpenElevator(bool instant = false)
    {
        animator.SetBool("Open", true);
        if (instant)
        {
            animator.SetTrigger("InstaOpen");
        }
        else
        {
            AudioManager.Instance.PlayClip(open);
        }
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

    public void ElevatorDoorClosed()
    {
        elevatorDoorClosed = true;
    }

    public void ElevatorDoorOpen()
    {
        elevatorDoorClosed = false;
    }

    public void CanClose()
    {
        canClose = true;
    }
}
