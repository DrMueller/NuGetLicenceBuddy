using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting
{
    public interface IMarkdownTableFactory
    {
        string CreateTable(IReadOnlyCollection<NugetLicence> licences);
    }
}