namespace Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogException(Exception ex);
        void LogError(string message);
        void LogInfo(string message);
    }
}