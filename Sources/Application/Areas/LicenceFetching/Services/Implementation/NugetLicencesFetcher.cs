using System.IO.Abstractions;
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
        INuspecReaderFactory nuspecReaderFactory,
        IFileSystem fileSystem)
        : INugetLicencesFetcher
    {
        public async Task<IReadOnlyCollection<NugetLicence>> FetchAsync(IReadOnlyCollection<NugetPackage> packages)
        {
            var result = new List<NugetLicence>();
            var nuspecReaders = await nuspecReaderFactory.CreateAllAsync(packages);

            foreach (var nuspec in nuspecReaders)
            {
                var nuspecIdentity = nuspec.Reader.GetIdentity();
                var nuspecVersion = nuspec.Reader.GetVersion();
                var licenceUrl = nuspec.Reader.GetLicenseUrl();
                logger.LogDebug($"Getting licence for package {nuspecIdentity.Id}..");

                var licence = await fileReader
                    .TryReadingAsync(licenceUrl)
                    .ReduceAsync(() => Licence.None);

                result.Add(new NugetLicence(
                    nuspecIdentity.Id,
                    nuspecVersion.ToFullString(),
                    licenceUrl,
                    fileSystem.Path.GetFileNameWithoutExtension(nuspec.Package.DllName),
                    licence));
            }

            return result
                .OrderBy(f => f.NugetIdentifier)
                .ToList();
        }
    }
}