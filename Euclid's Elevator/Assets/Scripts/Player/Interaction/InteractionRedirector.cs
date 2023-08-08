using UnityEngine;

public class InteractionRedirector : MonoBehaviour, IInteractable
{
    [SerializeField] MonoBehaviour redirectTo;

    IInteractable x;

    void OnValidate()
    {
        if (redirectTo != null && !(redirectTo is IInteractable))
        {
            Debug.Log("Not IInteractable.");
            redirectTo = null;
        }
        else if (redirectTo != null)
        {
            x = redirectTo as IInteractable;
        }
    }

    public bool IsInteractable => true;

    public string InteractionPrompt()
    {
        return x.InteractionPrompt();
    }

    public bool OnInteractCanceled()
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return x.OnInteractCanceled();
    }

    public bool OnInteractPerformed()
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return x.OnInteractPerformed();
    }
}