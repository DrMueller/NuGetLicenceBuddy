namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class EmbeddedResource(string updatePath, string dependendantUpon, string generator)
    {
        public string DependendantUpon { get; } = dependendantUpon;
        public string Generator { get; } = generator;
        public string UpdatePath { get; } = updatePath;
    }
}