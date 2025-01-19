using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services.Servants
{
    public interface ITransitiveDependencyFactory
    {
        Task<IReadOnlyCollection<TransitiveDependency>> CreateAsync(
            string netVersion,
            NugetIdentifier nugetIdentifier);
    }
}