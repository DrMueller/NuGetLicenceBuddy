using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services
{
    public interface IOrchestrator
    {
        Task OrchestrateAsync(
            ToolOptions options);
    }
}