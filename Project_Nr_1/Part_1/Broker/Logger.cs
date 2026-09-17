using System;
using System.IO;

namespace Broker
{
    static class Logger
    {
        private static readonly string LogDirectory = "Logs";
        private static readonly string LogFile = Path.Combine(LogDirectory, "broker.log");

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Warning(string message)
        {
            Write("WARNING", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        private static void Write(string level, string message)
        {
            Directory.CreateDirectory(LogDirectory);

            string logMessage =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} {message}";

            File.AppendAllText(
                LogFile,
                logMessage + Environment.NewLine
            );

            Console.WriteLine(logMessage);
        }
    }
}