using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services
{
    public interface IDependencyGraphFactory
    {
        Task<DependencyGraph> CreateFromJsonAsync(string json);
    }
}