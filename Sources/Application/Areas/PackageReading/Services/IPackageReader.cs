using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services
{
    public interface IPackageReader
    {
        Task<Maybe<IReadOnlyCollection<PackageIdentifier>>> TryReadingAsync(
            string sourcePath,
            bool includeTransitiveDependencies);
    }
}