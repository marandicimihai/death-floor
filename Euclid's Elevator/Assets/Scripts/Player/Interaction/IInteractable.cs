public interface IInteractable
{
    public bool IsInteractable { get; }

    public string InteractionPrompt();
    public bool OnInteractPerformed(IBehaviourService behaviourRequest);
    public bool OnInteractCanceled(IBehaviourService behaviourRequest);
}