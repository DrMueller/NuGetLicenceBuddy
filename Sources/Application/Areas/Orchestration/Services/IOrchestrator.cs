using Mmu.NuGetLicenceBuddy.Areas.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services
{
    public interface IOrchestrator
    {
        Task OrchestrateAsync(
            ToolOptions options);
    }
}