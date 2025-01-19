namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models
{
    public record PackageIdentifier(string PackageName, string Version)
    {
        public static PackageIdentifier Parse(string value)
        {
            // f.e. Microsoft.Data.SqlClient/2.1.4
            var vals = value.Split('/');

            return new PackageIdentifier(vals[0], vals[1]);
        }
    }
}