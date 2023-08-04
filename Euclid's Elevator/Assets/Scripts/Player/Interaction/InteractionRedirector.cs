using UnityEngine;

public class InteractionRedirector : MonoBehaviour, IInteractable
{
    [SerializeField] IInteractable redirectTo;

    public bool OnInteractCanceled(Player player, RaycastHit hit)
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return redirectTo.OnInteractCanceled(player, hit);
    }

    public bool OnInteractPerformed(Player player, RaycastHit hit)
    {
        if (redirectTo == null)
        {
            Debug.Log("Nothing to redirect to.");
            return false;
        }

        return redirectTo.OnInteractPerformed(player, hit);
    }
}