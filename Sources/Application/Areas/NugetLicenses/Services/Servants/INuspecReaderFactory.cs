using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using NuGet.Packaging;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants
{
    public interface INuspecReaderFactory
    {
        Task<IReadOnlyCollection<NuspecReader>> CreateAllAsync(IReadOnlyCollection<PackageIdentifier> packages);
    }
}