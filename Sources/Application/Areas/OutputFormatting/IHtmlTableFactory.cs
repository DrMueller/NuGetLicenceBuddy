using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting
{
    public interface IHtmlTableFactory
    {
        string CreateTable(IReadOnlyCollection<NugetLicence> licences);
    }
}
