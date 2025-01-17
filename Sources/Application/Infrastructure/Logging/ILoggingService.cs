namespace Mmu.NuGetLicenceBuddy.Infrastructure.Logging
{
    public interface ILoggingService
    {
        void LogDebug(string message);

        void LogError(string message);
        void LogInfo(string message);
    }
}