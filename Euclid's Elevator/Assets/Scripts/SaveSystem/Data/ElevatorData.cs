namespace DeathFloor.SaveSystem
{
    public class ElevatorData : SaveData
    {
        public bool Broken
        {
            get => broken;
            set => broken = value;
        }

        public bool Waiting
        {
            get => waiting;
            set => waiting = value;
        }

        public ElevatorData(bool broken, bool waiting)
        {
            Broken = broken;
            Waiting = waiting;
        }
    }
}