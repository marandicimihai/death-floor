using DeathFloor.Dialogue;
using DeathFloor.Utilities;
using System;

namespace DeathFloor.HUD
{
    public interface IDialogueDisplayer : IToggleable
    {
        public void DisplayDialogue(LineProperties lineProperties, Action<LineProperties> cb);
        public void DisplayAdditive(LineProperties lineProperties, Action<LineProperties> cb);
    }
}