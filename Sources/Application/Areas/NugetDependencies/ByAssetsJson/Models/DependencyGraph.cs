using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models
{
    [PublicAPI]
    public class DependencyGraph(string targetVersion, IReadOnlyCollection<NugetPackage> packages)
    {
        public IReadOnlyCollection<NugetPackage> Packages { get; } = packages;
        public string TargetVersion { get; } = targetVersion;
    }
}