using DeathFloor.Utilities;

namespace DeathFloor.Dialogue
{
    public interface IDialogueManager : IToggleable
    {
        public void Say(LineProperties lineProperties);
        public void SayAdditive(LineProperties lineProperties);
    }
}