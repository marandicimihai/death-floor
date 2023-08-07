using UnityEngine;

public class Elevator : MonoBehaviour, IInteractable
{
    public bool Broken { get; private set; }

    public bool IsInteractable => isInteractable;

    [SerializeField] Player player;
    [SerializeField] ItemProperties keycard;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] Collider doorCollider;
    [SerializeField] Transform waitForPlayerCenter;
    [SerializeField] float waitForPlayerRadius;
    [SerializeField] float elevatorRideTime;
    [SerializeField] float elevatorAccelTime;
    [SerializeField] bool isInteractable;

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

    VFXManager vfxmanager;
    Inventory inventory;
    AudioJob movejob;
    Animator animator;

    bool riding;
    bool waiting;
    bool canClose;
    bool elevatorDoorClosed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inventory = FindObjectOfType<Inventory>();
        vfxmanager = FindObjectOfType<VFXManager>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
            {
                Broken = true;
                CloseElevator(true);
                InitiateElevatorRide();
            };
        }
        else
        {
            Debug.Log("No game manager.");
        }

        if (SaveSystem.Instance != null)
        {
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
            else
            {
                InitiateElevatorRide();
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
        else
        {
            Debug.Log("No save system.");
        }
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
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ElevatorRideInitialized();
                }
                else
                {
                    Debug.Log("No game manager.");
                }

                InitiateElevatorRide();

                waiting = false;
            }
        }
    }

    public string InteractionPrompt()
    {
        return "Insert keycard";
    }

    public bool OnInteractPerformed()
    {
        if (inventory == null)
        {
            Debug.Log("No inventory.");
            return true;
        }

        if (inventory.Items[inventory.Index] != null)
        {
            if (Broken)
            {
                bool b = inventory.Items[inventory.Index].properties.name == toolbox.name;
                if (b)
                {
                    Broken = false;
                    AudioManager.Instance.PlayClip(repair);
                    inventory.DecreaseDurability(inventory.Index);
                }
                return true;
            }

            bool a = inventory.Items[inventory.Index].properties.name == keycard.name;
            if (a)
            {
                animator.SetTrigger("Keycard");
                AudioManager.Instance.PlayClip(insert);
                InsertKeycard();
                inventory.DecreaseDurability(inventory.Index);
            }
        }
        return true;
    }

    public bool OnInteractCanceled()
    {
        return true;
    }

    public bool MatchesRequirement(Player player)
    {
        if (inventory == null)
        {
            Debug.Log("No inventory.");
            return false;
        }

        if (inventory.Items[inventory.Index] != null)
        {
            if (Broken)
            {
                return inventory.Items[inventory.Index].properties.name == toolbox.name;
            }
            return inventory.Items[inventory.Index].properties.name == keycard.name;
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

        if (vfxmanager != null)
        {
            vfxmanager.CameraShake(AnimationAction.FadeAppear, elevatorAccelTime);
        }
        else
        {
            Debug.Log("No vfx manager.");
        }

        Invoke(nameof(DeaccelerateElevator), elevatorRideTime - elevatorAccelTime);
    }

    void DeaccelerateElevator()
    {
        if (vfxmanager != null)
        {
            vfxmanager.CameraShake(AnimationAction.FadeDisappear, elevatorAccelTime);
        }
        else
        {
            Debug.Log("No vfx manager.");
        }
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
