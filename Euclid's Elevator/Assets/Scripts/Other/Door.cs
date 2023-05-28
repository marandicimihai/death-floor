using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool locked;
    public Transform middle;

    public bool Open { get; private set; }
    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] float closedSoundThreshold;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;

    [SerializeField] ItemObject requiredItem;
    [SerializeField] ItemObject lockItem;

    [SerializeField] AudioSource doorOpen;
    [SerializeField] AudioSource doorClose;
    [SerializeField] AudioSource doorHandleS;
    [SerializeField] AudioSource unlock;

    [Header("Stage settings")]

    [SerializeField] int stageUnlock;

    public bool StageLocked { get; private set; }
    float t;

    IEnumerator doorCloseC;

    private void Awake()
    {
        if (stageUnlock > 1)
        {
            StageLocked = true;
        }
    }

    private void Start()
    {
        GameManager.MakePausable(this);
        GameManager.Instance.OnStageStart += (object sender, StageArgs args) =>
        {
            if (args.stage >= stageUnlock && StageLocked)
            {
                StageLocked = false;
            }
        };
    }
    private void Update()
    {
        if (Open)
        {
            t += 1 / openTime * Time.deltaTime;
            doorOpen.volume = (panel.localEulerAngles.y - openedYRot) / (closedYRot - openedYRot);
        }
        else
        {
            t -= 1 / openTime * Time.deltaTime;
        }
        t = Mathf.Clamp01(t);
        panel.localRotation = Quaternion.Slerp(Quaternion.Euler(0, closedYRot, 0), Quaternion.Euler(0, openedYRot, 0), t);
    }

    private void OnValidate()
    {
        if (Open)
        {
            panel.localRotation = Quaternion.Euler(0, openedYRot, 0);
        }
        else
        {
            panel.localRotation = Quaternion.Euler(0, closedYRot, 0);
        }
    }

    public bool Toggle(ItemObject requirement = null)
    {
        doorHandle.SetTrigger("PullHandle");
        if (requiredItem != null && requirement != null && requirement.itemName == requiredItem.itemName)
            locked = false;

        if (locked || StageLocked)
            return false;

        Open = !Open;
        if (Open)
        {
            doorOpen.Play();
            doorHandleS.Play();
        }
        else
        {
            if (doorCloseC == null)
            {
                doorCloseC = DoorCloseSound();
                StartCoroutine(doorCloseC);
            }
        }

        return true;
    }

    IEnumerator DoorCloseSound()
    {
        yield return new WaitUntil(() => 
        {
            return Mathf.Abs(panel.localEulerAngles.y - closedYRot) < closedSoundThreshold;
        });
        doorClose.Play();
        doorCloseC = null;
    }

    public bool TryLockItem(ItemObject item)
    {
        if (lockItem != null && item != null && lockItem.name == item.name)
        {
            return true;
        }
        return false;
    }

    public void ForceOpen()
    {
        locked = false;
        if (!Open)
        {
            Toggle();
            unlock.Play();
        }
    }

    public void ForceLock()
    {
        if (Open)
        {
            Toggle();
        }
        locked = true;
    }
}
