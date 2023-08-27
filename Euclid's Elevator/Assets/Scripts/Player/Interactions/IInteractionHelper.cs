namespace DeathFloor.Interactions
{
    internal interface IInteractionHelper
    {
        public bool CheckInteractable(IInteractable interactable);
        public void OnInteract();
    }
}