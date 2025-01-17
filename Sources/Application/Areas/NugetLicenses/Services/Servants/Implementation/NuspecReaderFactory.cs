using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using modan.FK2.Common.Extensions;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants.Implementation
{
    public class NuspecReaderFactory : INuspecReaderFactory
    {
        public async Task<IReadOnlyCollection<NuspecReader>> CreateAllAsync(IReadOnlyCollection<NugetPackage> packages)
        {
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<FindPackageByIdResource>();
            var cache = new SourceCacheContext();
            var logger = new NullLogger();

            return await packages
                .SelectAsync(async package => await TryCreatingAsync(resource, cache, logger, package))
                .SelectSomeAsync();
        }

        private static async Task<Maybe<NuspecReader>> TryCreatingAsync(
            FindPackageByIdResource resource,
            SourceCacheContext cache,
            NullLogger logger,
            NugetPackage package)
        {
            var nugetVersion = NuGetVersion.Parse(package.Identifier.Version);
            var packageIdentity = new PackageIdentity(package.Identifier.PackageName, nugetVersion);

            using var memoryStream = new MemoryStream();
            var res = await resource.CopyNupkgToStreamAsync(
                packageIdentity.Id,
                nugetVersion,
                memoryStream,
                cache,
                logger,
                CancellationToken.None);

            if (!res)
            {
                return None.Value;
            }

            memoryStream.Position = 0;
            using var packageReader = new PackageArchiveReader(memoryStream);
            var nuspecReader = await packageReader.GetNuspecReaderAsync(CancellationToken.None);

            return nuspecReader;
        }
    }
}