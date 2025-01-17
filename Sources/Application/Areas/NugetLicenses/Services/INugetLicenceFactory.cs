using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services
{
    public interface INugetLicenceFactory
    {
        Task<IReadOnlyCollection<NugetLicence>> CreateAllAsync(IReadOnlyCollection<NugetPackage> packages);
    }
}