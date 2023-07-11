using UnityEngine.AI;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Locked { get; private set; }
    public bool Open { get; private set; }
    public bool StageLocked { get; private set; }

    [SerializeField] ItemProperties key;
    [SerializeField] NavMeshObstacle obstacle;
    [SerializeField] Transform panel;
    [SerializeField] Animator animator;
    [SerializeField] Vector3 closedAngles;
    [SerializeField] Vector3 openAngles;
    [SerializeField] float openTime;
    [SerializeField] int unlockStage;

    [Header("Sounds")]
    [SerializeField] GameObject handle;
    [SerializeField] GameObject panelObj;
    [SerializeField] string openDoorName;
    [SerializeField] string closeDoorName;
    [SerializeField] string skrtDoorName;
    [SerializeField] float doorCloseInterpolation;

    AudioJob skrtjob;

    bool playedCloseSound;
    float interpolation;

    private void Awake()
    {
        obstacle.carving = false;
        Locked = true;

        if (Open)
        {
            interpolation = 1;
        }
        else
        {
            interpolation = 0;
        }

        if (unlockStage > 1)
        {
            StageLocked = true;
        }

        playedCloseSound = true;
    }

    private void Start()
    {
        GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) =>
        {
            if (GameManager.Instance.Stage == unlockStage)
            {
                StageLocked = false;
                OpenDoor(true);
            }
        };
    }

    private void Update()
    {
        if (StageLocked)
        {
            Locked = true;
        }

        if (Open && !Locked && !StageLocked)
        {
            interpolation += Time.deltaTime / openTime;
            if (skrtjob != null)
            {
                skrtjob.source.volume = 1 - interpolation;
            }
            playedCloseSound = false;
        }
        else
        {
            if (interpolation <= doorCloseInterpolation && !playedCloseSound)
            {
                animator.SetTrigger("PullHandle");
                AudioManager.Instance.PlayClip(handle, closeDoorName);
                playedCloseSound = true;
            }
            interpolation -= Time.deltaTime / openTime;
        }
        interpolation = Mathf.Clamp01(interpolation);
        panel.localEulerAngles = Vector3.Lerp(closedAngles, openAngles, interpolation);
    }

    public bool TryUnlock(Player player)
    {
        bool a;
        if (player.inventory.Items[player.inventory.Index] != null)
        {
            a = player.inventory.Items[player.inventory.Index].properties.name == key.name;
            if (a && !StageLocked)
            {
                Locked = false;
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
            return player.inventory.Items[player.inventory.Index].properties.name == key.name;
        }
        return false;
    }

    public void Toggle()
    {
        AudioManager.Instance.StopClip(skrtjob);
        if (Open)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor(bool forced = false)
    {
        if ((!Open && !Locked) || forced)
        {
            obstacle.carving = true;
            animator.SetTrigger("PullHandle");
            skrtjob = AudioManager.Instance.PlayClip(panelObj, skrtDoorName);
            AudioManager.Instance.PlayClip(handle, openDoorName);
            Locked = false;
            Open = true;
        }
    }

    public void CloseDoor()
    {
        if (Open)
        {
            obstacle.carving = false;
            Open = false;
        }
    }

    /// <summary>
    /// Pass in player variable to decrease durability of key
    /// </summary>
    /// <param name="player"></param>
    public void LockDoor(Player player = null)
    {
        Locked = true;
        if (Open)
        {
            Open = false;
        }
        if (player != null)
        {
            player.inventory.DecreaseDurability(player.inventory.GetItemIndex(key));
        }
    }
}
