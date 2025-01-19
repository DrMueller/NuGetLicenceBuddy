using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services
{
    public interface IAllowedLicencesChecker
    {
        void CheckLicences(
            IReadOnlyCollection<NugetLicence> licences,
            string allowedLicences);
    }
}