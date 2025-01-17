using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Services.Servants
{
    public interface IVsSolutionFactory
    {
        VsSolution Create(string sourcesFilePath);
    }
}