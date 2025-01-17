namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models
{
    public class PackageDependency
    {
        public PackageDependency(PackageIdentifier identifier)
        {
            Identifier = identifier;
        }

        public PackageIdentifier Identifier { get; }
    }
}