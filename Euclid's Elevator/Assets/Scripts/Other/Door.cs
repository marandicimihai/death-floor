using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Locked { get; private set; }
    public bool Open { get; private set; }
    public bool StageLocked { get; private set; }

    [SerializeField] ItemProperties key;
    [SerializeField] Transform panel;
    [SerializeField] Vector3 closedAngles;
    [SerializeField] Vector3 openAngles;
    [SerializeField] float openTime;
    [SerializeField] int unlockStage;

    float interpolation;

    private void Awake()
    {
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
        }
        else
        {
            interpolation -= Time.deltaTime / openTime;
        }
        interpolation = Mathf.Clamp01(interpolation);
        panel.localEulerAngles = Vector3.Lerp(closedAngles, openAngles, interpolation);
    }

    public bool TryUnlock(Player player)
    {
        bool a = player.inventory.Contains(key);
        if (a && !StageLocked)
        {
            Locked = false;
            player.inventory.DecreaseDurability(player.inventory.GetItemIndex(key));
        }
        return a;
    }

    public bool MatchesRequirement(Player player)
    {
        return player.inventory.Contains(key);
    }

    public void Toggle()
    {
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
            Locked = false;
            Open = true;
        }
    }

    public void CloseDoor()
    {
        if (Open)
        {
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
