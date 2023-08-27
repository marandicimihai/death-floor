namespace DeathFloor.Utilities.Logger
{
    public interface ILogger
    {
        public bool EnableLogging { set; }

        public void Log(string message);

        public void LogWarning(string message);

        public void LogError(string message);

        public void Ping();

        public void Success(string message = null);

        public ILogger ToggleLogging(bool value);
    }
}