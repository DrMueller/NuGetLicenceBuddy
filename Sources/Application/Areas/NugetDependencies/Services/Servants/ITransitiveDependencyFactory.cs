using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services.Servants
{
    public interface ITransitiveDependencyFactory
    {
        Task<IReadOnlyCollection<TransitiveDependency>> CreateAsync(
            string netVersion,
            NugetIdentifier nugetIdentifier);
    }
}