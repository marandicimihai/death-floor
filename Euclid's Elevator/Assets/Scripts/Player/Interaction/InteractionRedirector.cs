using UnityEngine;

public class InteractionRedirector : MonoBehaviour, IInteractable
{
    [SerializeField] IInteractable redirectTo;

    public bool IsInteractable => true;

    public string InteractionPrompt()
    {
        return redirectTo.InteractionPrompt();
    }

    public bool OnInteractCanceled()
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return redirectTo.OnInteractCanceled();
    }

    public bool OnInteractPerformed()
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return redirectTo.OnInteractPerformed();
    }
}