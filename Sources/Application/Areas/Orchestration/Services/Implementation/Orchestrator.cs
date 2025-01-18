using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services.Implementation
{
    [UsedImplicitly]
    public class Orchestrator(
        INugetLicenceFactory licenceFactory,
        IPackageIdentifierFactory dependecyGraphFactory,
        IMarkdownTableFactory markdownTableFactory,
        IOutputWriter outputWriter,
        ILoggingService logger)
        : IOrchestrator
    {
        public async Task OrchestrateAsync(ToolOptions options)
        {
            await dependecyGraphFactory
                .TryCreatingAsync(options.SourcesPath, options.IncludeTransitiveDependencies)
                .MapAsync(licenceFactory.CreateAllAsync)
                .MapAsync(markdownTableFactory.CreateTable)
                .TapAsync(logger.LogInfo)
                .TapAsync(outputWriter.WriteToFileAsync);
        }
    }
}