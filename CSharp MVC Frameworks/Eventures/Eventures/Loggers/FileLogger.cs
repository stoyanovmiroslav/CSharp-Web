using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.IO;

namespace Eventures.Loggers
{
    public class FileLogger : ILogger
    {
        public async void LogMessage(string message, LogMessageLevel level)
        {
            await File.AppendAllTextAsync("Loggers/log.txt", $"{level.ToString()}{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public async void LogMessage(string message)
        {
            await File.AppendAllTextAsync("Loggers/log.txt", $"{message}{Environment.NewLine}");
        }
    }
}