﻿using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services;
using Mmu.NuGetLicenceBuddy.Areas.OutputFormatting;
using Mmu.NuGetLicenceBuddy.Areas.OutputReading.Models;
using Mmu.NuGetLicenceBuddy.Areas.OutputReading.Services;
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
        IAllowedLicencesChecker licencesChecker,
        IAssemblyInfoReader assemblyInfoReader,
        ITaskOutputService taskOutputService)
        : IOrchestrator
    {
        public async Task OrchestrateAsync(ToolOptions options)
        {
            try
            {
                var nugetLicences = await packageReader
                    .TryReadingAsync(options.SourcesPath, options.IncludeTransitiveDependencies, options.ExcludePackagesFilterOption)
                    .MapAsync(packages => licencesFetcher.FetchAsync(packages.FlatPackages))
                    .MapAsync(lic => FilterByOutputVersionAsync(
                        lic,
                        options.MatchOutputVersion,
                        options.OutputPath!));

                await CreateOutputAsync(nugetLicences);

                nugetLicences.Tap(lic => licencesChecker.CheckLicences(lic, options.AllowedLicences));
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                taskOutputService.FailTask();
            }

            logger.LogDebug("Finished.");
        }

        private async Task CreateOutputAsync(Maybe<IReadOnlyCollection<NugetLicence>> nugetLicences)
        {
            await nugetLicences
                .Map(markdownTableFactory.CreateTable)
                .Tap(logger.LogInfo)
                .TapAsync(outputWriter.WriteToFileAsync);
        }

        private void DebugDllInfos(IReadOnlyCollection<AssemblyInfo> dllInfos)
        {
            foreach (var dllInfo in dllInfos)
            {
                logger.LogDebug($"DllInfo: {dllInfo.AssemblyName} - {dllInfo.AssemblyVersion}");
            }
        }

        private async Task<IReadOnlyCollection<NugetLicence>> FilterByOutputVersionAsync(
            IReadOnlyCollection<NugetLicence> licences,
            bool matchOutputVersion,
            string outputPath)
        {
            if (!matchOutputVersion)
            {
                return licences;
            }

            var dllInfos = await assemblyInfoReader.ReadAllAsync(outputPath);
            logger.LogDebug($"Found {dllInfos.Count} infos from dlls..");
            DebugDllInfos(dllInfos);

            var result = licences
                .GroupBy(f => f.NugetIdentifier)
                .Where(f => f.Count() == 1)
                .Select(f => f.Single())
                .ToList();

            var multipleLicences = licences.Except(result)
                .GroupBy(f => f.NugetIdentifier)
                .ToList();

            foreach (var licenceGroup in multipleLicences)
            {
                logger.LogDebug($"Multiple licences found for {licenceGroup.Key}..");
                var dllInfo = dllInfos.SingleOrDefault(f => f.AssemblyName == licenceGroup.Key);

                if (dllInfo == null)
                {
                    logger.LogDebug($"No dll info found for {licenceGroup.Key}..");
                    result.AddRange(licenceGroup);

                    continue;
                }

                var matchingNugetVersion = licenceGroup.SingleOrDefault(f => f.NugetVersion == dllInfo.AssemblyVersion);

                if (matchingNugetVersion == null)
                {
                    logger.LogDebug($"No matching nuget version found for {licenceGroup.Key} and version {dllInfo.AssemblyVersion}..");
                    result.AddRange(licenceGroup);

                    continue;
                }

                result.Add(matchingNugetVersion);
            }

            return result;
        }
    }
}