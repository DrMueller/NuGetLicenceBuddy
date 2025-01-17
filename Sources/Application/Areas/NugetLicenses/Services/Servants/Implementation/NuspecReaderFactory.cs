﻿using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants.Implementation
{
    [UsedImplicitly]
    public class NuspecReaderFactory : INuspecReaderFactory
    {
        public async Task<IReadOnlyCollection<NuspecReader>> CreateAllAsync(IReadOnlyCollection<PackageIdentifier> packages)
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
            PackageIdentifier package)
        {
            var nugetVersion = NuGetVersion.Parse(package.Version);
            var packageIdentity = new PackageIdentity(package.PackageName, nugetVersion);

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