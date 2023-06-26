using System;
using System.IO;

namespace WPFTracker.Utilities
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger("log.txt"));
        private string _logFilePath;

        public static Logger Instance { get { return lazy.Value; } }

        public event EventHandler? ExceptionLogged;

        private Logger(string filePath)
        {
            _logFilePath = filePath;
        }

        public void LogException(Exception ex)
        {
            var logMessage = $"Unhandled exception {ex.Message}";

            this.Log(logMessage, "  Stack Trace: " + ex.StackTrace);

            OnExceptionLogged();
        }

        private void OnExceptionLogged()
        {
            ExceptionLogged?.Invoke(this, EventArgs.Empty);
        }

        private void Log(string message, string details)
        {
            using (StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now.ToString("g")}] {message}");
                writer.WriteLine(details);
                writer.WriteLine();
            }
        }
    }
}
