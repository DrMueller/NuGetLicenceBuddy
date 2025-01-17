using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services
{
    public interface IDependencyGraphFactory
    {
        Task<DependencyGraph> CreateFromJsonAsync(string json);
    }
}