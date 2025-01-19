using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services.Servants;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services.Implementation
{
    [UsedImplicitly]
    public class NugetLicencesFetcher(
        ILicenceFileReader fileReader,
        ILoggingService logger,
        INuspecReaderFactory nuspecReaderFactory)
        : INugetLicencesFetcher
    {
        public async Task<IReadOnlyCollection<NugetLicence>> FetchAsync(IReadOnlyCollection<PackageIdentifier> packages)
        {
            var result = new List<NugetLicence>();

            var nuspecReaders = await nuspecReaderFactory.CreateAllAsync(packages);

            foreach (var nuspecReader in nuspecReaders)
            {
                var nuspecIdentity = nuspecReader.GetIdentity();
                var nuspecVersion = nuspecReader.GetVersion();
                var licenceUrl = nuspecReader.GetLicenseUrl();
                logger.LogDebug($"Getting licence for package {nuspecIdentity.Id}..");

                var licence = await fileReader
                    .TryReadingAsync(licenceUrl)
                    .ReduceAsync(() => Licence.None);

                result.Add(new NugetLicence(
                    nuspecIdentity.Id,
                    nuspecVersion.ToFullString(),
                    licenceUrl,
                    licence));
            }

            return result
                .OrderBy(f => f.NugetIdentifier)
                .ToList();
        }
    }
}