using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Models;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services.Implementation
{
    [UsedImplicitly]
    public class LoggingService : ILoggingService
    {
        private static readonly IDictionary<LogLevel, string> _prefixMap = new Dictionary<LogLevel, string>
        {
            { LogLevel.Debug, "##[debug]" },
            { LogLevel.Info, "##[section]" },
            { LogLevel.Warning, "##[warning]" },
            { LogLevel.Error, "##[error]" }
        };

        public void LogDebug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }

        private static void Log(LogLevel level, string message)
        {
            var azureDevOpsPrefix = _prefixMap[level];
            Console.WriteLine($"{azureDevOpsPrefix}{message}");
        }
    }
}