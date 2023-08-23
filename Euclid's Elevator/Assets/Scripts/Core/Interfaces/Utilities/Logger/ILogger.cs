namespace DeathFloor.Utilities.Logger
{
    public interface ILogger
    {
        public bool EnableLogging { set; }

        public void Log(string message);

        public void LogWarning(string message = null);

        public void LogError(string message = null);

        public void Ping();

        public void Success(string message = null);

        public ILogger ToggleLogging(bool value);
    }
}