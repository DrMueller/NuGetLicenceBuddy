using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services
{
    public interface IAllowedLicencesChecker
    {
        void CheckLicences(
            IReadOnlyCollection<NugetLicence> licences,
            string allowedLicences);
    }
}
