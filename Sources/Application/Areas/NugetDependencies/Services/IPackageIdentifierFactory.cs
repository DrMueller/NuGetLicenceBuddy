using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services
{
    public interface IPackageIdentifierFactory
    {
        Task<Maybe<IReadOnlyCollection<PackageIdentifier>>> TryCreatingAsync(
            string sourcePath,
            bool includeTransitiveDependencies);
    }
}