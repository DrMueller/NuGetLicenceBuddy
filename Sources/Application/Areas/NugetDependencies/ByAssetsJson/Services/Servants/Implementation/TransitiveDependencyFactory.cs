using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services.Servants.Implementation
{
    public class TransitiveDependencyFactory : ITransitiveDependencyFactory
    {
        public async Task<IReadOnlyCollection<TransitiveDependency>> CreateAsync(
            string netVersion,
            NugetIdentifier nugetIdentifier)
        {
            var framework = NuGetFramework.ParseFolder(netVersion);
            var logger = new NullLogger();
            var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

            var resource = await repository.GetResourceAsync<FindPackageByIdResource>();
            var packageIdentity = new PackageIdentity(nugetIdentifier.Name, NuGetVersion.Parse(nugetIdentifier.Version));

            var dependencies = await GetTransitiveDependencies(resource, packageIdentity, framework, logger, cache);
            var depsWithoutOriginalPackage = dependencies.Where(f => f != packageIdentity).ToList();

            var transDeps = depsWithoutOriginalPackage
                .Select(f => new TransitiveDependency(new PackageIdentifier(f.Id, f.Version.ToFullString())))
                .ToList();

            return transDeps;
        }

        private static async Task GetDependenciesRecursive(
            FindPackageByIdResource resource,
            PackageIdentity packageIdentity,
            NuGetFramework framework,
            ILogger logger,
            SourceCacheContext cacheContext,
            HashSet<PackageIdentity> allDependencies)
        {
            if (!allDependencies.Add(packageIdentity))
            {
                return;
            }

            var packageDependencyInfo = await resource.GetDependencyInfoAsync(packageIdentity.Id, packageIdentity.Version, cacheContext, logger, default);

            if (packageDependencyInfo == null)
            {
                return;
            }

            foreach (var group in packageDependencyInfo.DependencyGroups)
            {
                if (group.TargetFramework == framework || group.TargetFramework == NuGetFramework.AnyFramework)
                {
                    foreach (var packageDependency in group.Packages)
                    {
                        var dependencyIdentity = new PackageIdentity(packageDependency.Id, packageDependency.VersionRange.MinVersion);
                        await GetDependenciesRecursive(resource, dependencyIdentity, framework, logger, cacheContext, allDependencies);
                    }
                }
            }
        }

        private static async Task<HashSet<PackageIdentity>> GetTransitiveDependencies(
            FindPackageByIdResource resource,
            PackageIdentity packageIdentity,
            NuGetFramework framework,
            ILogger logger,
            SourceCacheContext cacheContext)
        {
            var allDependencies = new HashSet<PackageIdentity>(PackageIdentityComparer.Default);
            await GetDependenciesRecursive(resource, packageIdentity, framework, logger, cacheContext, allDependencies);

            return allDependencies;
        }
    }
}