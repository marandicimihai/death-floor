using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked;

    [SerializeField] bool open;
    [SerializeField] float closedYRot;
    [SerializeField] float openedYRot;
    [SerializeField] float openTime;
    [SerializeField] Transform panel;
    [SerializeField] Animator doorHandle;

    [SerializeField] ItemObject requiredItem;

    [Header("Stage settings")]

    [SerializeField] int stageUnlock;

    public bool StageLocked { get; private set; }
    float t;

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

        if (GameManager.instance.spawnEnemy)
            GameManager.instance.enemyController.NoiseHeardNav(transform.position);

        open = !open;
        doorHandle.SetTrigger("PullHandle");

        return true;
    }

    public void ForceOpen()
    {
        locked = false;
        if (!open)
            Toggle();
    }
}
