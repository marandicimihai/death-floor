using UnityEngine;

public interface IInteractable
{
    bool OnInteractPerformed(Player player, RaycastHit hit);
    bool OnInteractCanceled(Player player, RaycastHit hit);
}