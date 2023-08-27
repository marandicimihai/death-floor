using System;

namespace DeathFloor.Dialogue
{
    public interface IDialogueDisplayer
    {
        public void DisplayDialogue(LineProperties lineProperties, Action<LineProperties> cb);
    }
}