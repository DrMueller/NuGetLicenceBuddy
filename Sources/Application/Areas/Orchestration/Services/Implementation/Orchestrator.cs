using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services;
using Mmu.NuGetLicenceBuddy.Areas.OutputFormatting;
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
        ILoggingService logger,
        IAllowedLicencesChecker licencesChecker)
        : IOrchestrator
    {
        public async Task OrchestrateAsync(ToolOptions options)
        {
            var nugetLicences = await dependecyGraphFactory
                .TryCreatingAsync(options.SourcesPath, options.IncludeTransitiveDependencies)
                .MapAsync(licenceFactory.CreateAllAsync);

            await CreateOutputAsync(nugetLicences);

            nugetLicences.Tap(lic => licencesChecker.CheckLicences(lic, options.AllowedLicences));
        }

        private async Task CreateOutputAsync(Maybe<IReadOnlyCollection<NugetLicence>> nugetLicences)
        {
            await nugetLicences
                .Map(markdownTableFactory.CreateTable)
                .Tap(logger.LogInfo)
                .TapAsync(outputWriter.WriteToFileAsync);
        }
    }
}