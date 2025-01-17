namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models
{
    public record PackageIdentifier
    {
        public string Description => $"{PackageName} ({Version})";
        public string PackageName { get; }
        public string Version { get; }

        public PackageIdentifier(string packageName, string version)
        {
            PackageName = packageName;
            Version = version;
        }

        public static PackageIdentifier Parse(string value)
        {
            // f.e. Microsoft.Data.SqlClient/2.1.4
            var vals = value.Split('/');

            return new PackageIdentifier(vals[0], vals[1]);
        }
    }
}