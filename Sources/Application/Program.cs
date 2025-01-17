using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.DependencyInjection;

namespace Mmu.NuGetLicenceBuddy
{
    [PublicAPI]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var sourcePath = "C:\\MyGit\\Personal\\WindowsBuddies\\Wb.TimeBuddy";
            var container = ContainerFactory.Create();
            var orchestrator = container.GetInstance<IOrchestrator>();
            await orchestrator.OrchestrateAsync(sourcePath);
        }
    }
}