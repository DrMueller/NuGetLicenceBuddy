using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models
{
    [PublicAPI("Next feature")]
    public record TransitiveDependency(PackageIdentifier PackageIdentifier);
}