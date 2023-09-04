using DeathFloor.Journal;
using DeathFloor.Utilities;

namespace DeathFloor.HUD
{
    internal interface IJournalDisplayer : IToggleable
    {
        public void OpenJournal();
        public void CloseJournal();
        public void DisplayPages(PageProperties left, PageProperties right, int leftPageIndex, int rightPageIndex);
        public void Clear();
    }
}