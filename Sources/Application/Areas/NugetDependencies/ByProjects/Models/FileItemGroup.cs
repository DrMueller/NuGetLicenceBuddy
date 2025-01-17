namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public record FileItemGroup(IReadOnlyCollection<FileItemGroupEntry> Entries);
}