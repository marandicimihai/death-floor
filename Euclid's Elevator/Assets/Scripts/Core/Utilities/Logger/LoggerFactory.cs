using UnityEngine;

namespace DeathFloor.Utilities.Logger
{
    internal class LoggerFactory : MonoBehaviour, ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            return new DefaultLogger();
        }

        public ILogger CreateLogger(bool enableLogging)
        {
            return new DefaultLogger(enableLogging);
        }

        public ILogger CreateLogger(LoggerProfile profile)
        {
            return new DefaultLogger(profile);
        }

        public ILogger CreateLogger(LoggerProfile profile, bool enableLogging)
        {
            return new DefaultLogger(profile, enableLogging);
        }
    }
}