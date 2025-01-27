using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services
{
    public interface INugetLicencesFetcher
    {
        Task<IReadOnlyCollection<NugetLicence>> FetchAsync(IReadOnlyCollection<NugetPackage> packages);
    }
}