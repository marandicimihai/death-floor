namespace DeathFloor.Dialogue
{
    public interface IDialogueManager
    {
        public void Say(LineProperties lineProperties);
        public void SayAdditive(LineProperties lineProperties);
    }
}