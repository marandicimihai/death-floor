namespace DeathFloor.Utilities.Logger
{
    public interface ILogger : IToggleable
    {
        public void Debug(string message);
        public void Warning(string message);
        public void Error(string message);
        public void Fatal(string message);

        public string Bold(string message);
        public string Italic(string message);
        public string Color(string message, string color);
    }
}