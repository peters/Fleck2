using System;

namespace Fleck2
{
    public enum LogLevel
    {
        None,
        Debug,
        Info,
        Warn,
        Error
    }

    public class FleckLog
    {
        public static LogLevel Level = LogLevel.Info;

        public static Fleck2Extensions.Action<LogLevel, string, Exception> LogAction = (level, message, ex) =>
        {
            if (Level == LogLevel.None) return;
            if (level >= Level)
                Console.WriteLine("{0} [{1}] {2} {3}", DateTime.Now, level, message, ex);
        };

        public static void Warn(string message, Exception ex)
        {
            LogAction(LogLevel.Warn, message, ex);
        }

        public static void Error(string message, Exception ex)
        {
            LogAction(LogLevel.Error, message, ex);
        }

        public static void Debug(string message, Exception ex)
        {
            LogAction(LogLevel.Debug, message, ex);
        }

        public static void Info(string message, Exception ex)
        {
            LogAction(LogLevel.Info, message, ex);
        }

    }
}
