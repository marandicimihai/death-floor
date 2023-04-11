using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool locked;
    public Transform middle;

    [SerializeField] bool open;
    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] float closedSoundThreshold;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;

    [SerializeField] ItemObject requiredItem;

    [SerializeField] AudioSource doorOpen;
    [SerializeField] AudioSource doorClose;
    [SerializeField] AudioSource doorHandleS;

    [Header("Stage settings")]

    [SerializeField] int stageUnlock;

    public bool StageLocked { get; private set; }
    float t;

    IEnumerator doorCloseC;

    private void Awake()
    {
        StageLocked = true;
    }
    private void Start()
    {
        GameManager.instance.OnStageStart += (object sender, StageArgs args) =>
        {
            if (args.stage >= stageUnlock && StageLocked)
            {
                StageLocked = false;
            }
        };
    }
    private void Update()
    {
        if (open)
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
        if (open)
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
        if (requiredItem != null && requirement != null && requirement.itemName == requiredItem.itemName)
            locked = false;

        if (locked || StageLocked)
            return false;

        open = !open;
        if (open)
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
        doorHandle.SetTrigger("PullHandle");

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

    public void ForceOpen()
    {
        locked = false;
        if (!open)
            Toggle();
    }
}
