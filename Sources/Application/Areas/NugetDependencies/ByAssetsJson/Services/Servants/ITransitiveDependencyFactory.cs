using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services.Servants
{
    public interface ITransitiveDependencyFactory
    {
        Task<IReadOnlyCollection<TransitiveDependency>> CreateAsync(
            string netVersion,
            NugetIdentifier nugetIdentifier);
    }
}