public interface IInteractable
{
    public bool IsInteractable { get; }

    public string InteractionPrompt();
    public bool OnInteractPerformed();
    public bool OnInteractCanceled();
}