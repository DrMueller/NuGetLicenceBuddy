using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Implementation
{
    [UsedImplicitly]
    public class LoggingService : ILoggingService
    {
        public void LogDebug(string message)
        {
            Console.WriteLine($"##vso[task.debug]{message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"##vso[task.logissue type=error;]Error occurred: {message}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}