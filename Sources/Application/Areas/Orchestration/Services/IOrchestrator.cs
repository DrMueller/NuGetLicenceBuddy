namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services
{
    public interface IOrchestrator
    {
        Task OrchestrateAsync(string sourcePath);
    }
}