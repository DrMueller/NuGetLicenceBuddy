namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models
{
    public class NugetIdentifier(string fullName)
    {
        public string Name => fullName.Split('/').ElementAt(0);

        public string Version => fullName.Split('/').ElementAt(1);
    }
}