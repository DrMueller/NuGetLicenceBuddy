namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models
{
    public record TransitiveDependency
    {
        public TransitiveDependency(PackageIdentifier packageIdentifier)
        {
            PackageIdentifier = packageIdentifier;
        }

        public PackageIdentifier PackageIdentifier { get; }
    }
}