using DeathFloor.Utilities;

namespace DeathFloor.Journal
{
    public interface IJournalManager : IToggleable
    {
        public void AddPage(PageProperties page);
    }
}