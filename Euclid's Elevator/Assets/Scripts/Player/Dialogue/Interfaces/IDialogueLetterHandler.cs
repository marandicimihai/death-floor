namespace DeathFloor.Dialogue
{
    public interface IDialogueLetterHandler
    {
        public void OnLetter(char letter, LineProperties current = null);
    }
}