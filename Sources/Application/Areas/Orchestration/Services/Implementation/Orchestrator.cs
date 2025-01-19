using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services;
using Mmu.NuGetLicenceBuddy.Areas.OutputFormatting;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services.Implementation
{
    [UsedImplicitly]
    public class Orchestrator(
        INugetLicencesFetcher licencesFetcher,
        IPackageReader packageReader,
        IMarkdownTableFactory markdownTableFactory,
        IOutputWriter outputWriter,
        ILoggingService logger,
        IAllowedLicencesChecker licencesChecker)
        : IOrchestrator
    {
        public async Task OrchestrateAsync(ToolOptions options)
        {
            var nugetLicences = await packageReader
                .TryReadingAsync(options.SourcesPath, options.IncludeTransitiveDependencies)
                .MapAsync(licencesFetcher.FetchAsync);

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