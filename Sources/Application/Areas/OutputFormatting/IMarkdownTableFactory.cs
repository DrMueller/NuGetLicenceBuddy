using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting
{
    public interface IMarkdownTableFactory
    {
        string CreateTable(IReadOnlyCollection<NugetLicence> licences);
    }
}