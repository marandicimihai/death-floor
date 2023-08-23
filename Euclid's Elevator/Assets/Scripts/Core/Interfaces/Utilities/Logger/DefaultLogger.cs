using UnityEditor;
using UnityEngine;

namespace DeathFloor.Utilities.Logger
{
    public class DefaultLogger : ILogger
    {
        public bool EnableLogging { set => _enableLogging = value; }

        private static bool _canLog = true;
        private readonly LoggerProfile _profile = new()
        {
            MessageColor = "teal",
            WarningColor = "orange",
            ErrorColor = "red",
            SuccessColor = "green",
            PingColor = "lightblue",
            MessageTextColor = "white",
            WarningTextColor = "white",
            ErrorTextColor = "white",
            SuccessTextColor = "green",
        };

        private bool _enableLogging = true;

        public DefaultLogger()
        {
            
        }

        public DefaultLogger(bool enableLogging)
        {
            _enableLogging = enableLogging;
        }

        public DefaultLogger(LoggerProfile profile)
        {
            _profile = profile;
        }

        public void Log(string message)
        {
            if (!_enableLogging || !_canLog) return;

            Debug.Log($"<size=14><color={_profile.MessageColor}>Message: </color></size><color={_profile.MessageTextColor}>{message}</color>");
        }

        public void LogWarning(string message = null)
        {
            if (!_enableLogging || !_canLog) return;

            if (message != null)
            {
                Debug.Log($"<size=14><color={_profile.WarningColor}>Warning: </color></size><color={_profile.WarningTextColor}>{message}</color>");
            }
            else
            {
                Debug.Log($"<size=14><color={_profile.WarningColor}>Warning!</color></size>");
            }
        }

        public void LogError(string message = null)
        {
            if (!_enableLogging || !_canLog) return;

            if (message != null)
            {
                Debug.Log($"<size=14><color={_profile.ErrorColor}>Error: </color></size><color={_profile.ErrorTextColor}>{message}</color>");
            }
            else
            {
                Debug.Log($"<size=14><color={_profile.ErrorColor}>Error!</color></size>");
            }
        }

        public void Ping()
        {
            if (!_enableLogging || !_canLog) return;

            Debug.Log($"<size=14><color={_profile.PingColor}>Ping</color></size>");
        }

        public void Success(string message = null)
        {
            if (!_enableLogging || !_canLog) return;

            if (message != null)
            {
                Debug.Log($"<size=14><color={_profile.SuccessColor}>Success: </color></size><color={_profile.SuccessTextColor}>{message}</color>");
            }
            else
            {
                Debug.Log($"<size=14><color={_profile.SuccessColor}>Success!</color></size>");
            }
        }

        public ILogger ToggleLogging(bool value)
        {
            _enableLogging = value;
            return this;
        }

        [MenuItem("Logging/Enable Log")]
        private static void EnableLog()
        {
            _canLog = true;
        }

        [MenuItem("Logging/Disable Log")]
        private static void DisableLog()
        {
            _canLog = false;
        }

        [MenuItem("Logging/Log Status")]
        private static void LogStatus()
        {
            if (_canLog)
            {
                Debug.Log("Logging is enabled");
            }
            else
            {
                Debug.Log("Logging is disabled");
            }
        }
    }
}