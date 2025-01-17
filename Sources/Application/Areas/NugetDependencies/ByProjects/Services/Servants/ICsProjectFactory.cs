using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Services.Servants
{
    public interface ICsProjectFactory
    {
        CsProj Create(string filePath);
    }
}