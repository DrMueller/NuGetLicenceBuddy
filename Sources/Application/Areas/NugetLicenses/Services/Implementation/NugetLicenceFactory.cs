using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Implementation
{
    [UsedImplicitly]
    public class NugetLicenceFactory(
        ILicenceFileReader fileReader,
        ILoggingService logger,
        INuspecReaderFactory nuspecReaderFactory)
        : INugetLicenceFactory
    {
        public async Task<IReadOnlyCollection<NugetLicence>> CreateAllAsync(IReadOnlyCollection<PackageIdentifier> packages)
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

            return result;
        }
    }
}