namespace DeathFloor.Utilities.Logger
{
    public interface ILoggerFactory
    {
        public ILogger CreateLogger();

        public ILogger CreateLogger(bool enableLogging);

        public ILogger CreateLogger(LoggerProfile profile);

        public ILogger CreateLogger(LoggerProfile profile, bool enableLogging);
    }
}