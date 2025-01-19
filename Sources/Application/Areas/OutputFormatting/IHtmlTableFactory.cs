using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting
{
    public interface IHtmlTableFactory
    {
        string CreateTable(IReadOnlyCollection<NugetLicence> licences);
    }
}