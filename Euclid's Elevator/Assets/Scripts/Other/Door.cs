using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Locked { get; private set; }
    public bool Open { get; private set; }
    
    [SerializeField] ItemProperties key;
    [SerializeField] Transform panel;
    [SerializeField] Vector3 closedAngles;
    [SerializeField] Vector3 openAngles;
    [SerializeField] float openTime;
    [SerializeField] int unlockStage;

    bool stageLocked;

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
            stageLocked = true;
        }

        GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) =>
        {
            if (GameManager.Instance.Stage == unlockStage)
            {
                stageLocked = false;
                OpenDoor(true);
            }
        };
    }

    private void Update()
    {
        if (stageLocked)
        {
            Locked = true;
        }

        if (Open && !Locked && !stageLocked)
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

    public bool CheckItem(ItemProperties req)
    {
        if (req.name == key.name && !stageLocked)
        {
            Locked = false;
            return true;
        }
        return false;
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

    public void LockDoor()
    {
        Locked = true;
        if (Open)
        {
            Open = false;
        }
    }
}
