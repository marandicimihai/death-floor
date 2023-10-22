using UnityEngine;

namespace DeathFloor.Interactions
{
    public interface IInteractionHelper
    {
        public InteractionTag TargetInteractionTag { get; }

        public void InteractionExtension(GameObject rootObject);

        public void EndInteractionExtension(GameObject rootObject);
    }
}