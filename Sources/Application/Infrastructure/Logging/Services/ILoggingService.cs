namespace Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogError(string message);
        void LogInfo(string message);
    }
}