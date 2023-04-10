using UnityEngine;

public class JumpscareBase : MonoBehaviour
{
    [SerializeField] bool oneTime;

    bool happened;

    protected virtual bool CheckContext()
    {
        return false;
    }

    protected virtual void OnJumpscareTriggered()
    {

    }

    private void Update()
    {
        if (oneTime && happened)
            return;

        if (CheckContext())
        {
            OnJumpscareTriggered();
            happened = true;
        }
    }
}
