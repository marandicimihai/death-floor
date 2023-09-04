using UnityEditor;
using UnityEngine;

namespace DeathFloor.Utilities.Logger
{
    internal class DefaultLogger : ILogger
    {
        public bool CanLog { set => _canLog = value; }

        private static bool _staticLogEnabled = true;
        private bool _canLog = true;

        public DefaultLogger()
        {
            
        }

        public DefaultLogger(bool enableLogging)
        {
            _canLog = enableLogging;
        }

        public void Debug(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Error(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new System.NotImplementedException();
        }

        public string Bold(string message)
        {
            throw new System.NotImplementedException();
        }

        public string Italic(string message)
        {
            throw new System.NotImplementedException();
        }

        public string Color(string message, string color)
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        [MenuItem("Logging/Enable Log")]
        private static void EnableLog()
        {
            _staticLogEnabled = true;
        }

        [MenuItem("Logging/Disable Log")]
        private static void DisableLog()
        {
            _staticLogEnabled = false;
        }

        [MenuItem("Logging/Log Status")]
        private static void LogStatus()
        {
            if (_staticLogEnabled)
            {
                UnityEngine.Debug.Log("Logging is enabled");
            }
            else
            {
                UnityEngine.Debug.Log("Logging is disabled");
            }
        }
    }
}