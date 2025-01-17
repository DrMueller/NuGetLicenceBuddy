using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.Options.Services;
using Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.DependencyInjection;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy
{
    [PublicAPI]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var container = ContainerFactory.Create();

            var optionsFactory = container.GetInstance<IOptionsFactory>();
            await optionsFactory
                .TryCreating(args)
                .WhenSomeAsync(async opt =>
                {
                    var orchestrator = container.GetInstance<IOrchestrator>();
                    await orchestrator.OrchestrateAsync(opt);
                });
        }
    }
}