namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models
{
    public class DependencyGraph(string targetVersion, IReadOnlyCollection<NugetPackage> packages)
    {
        public IReadOnlyCollection<NugetPackage> Packages { get; } = packages;
        public string TargetVersion { get; } = targetVersion;
    }
}